using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HenryKam.SlackWhereIs.Model;
using HenryKam.SlackWhereIs.Application;
using HenryKam.SlackWhereIs.Infrastructure.Exchange;
using HenryKam.SlackWhereIs.Infrastructure.Slack;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Net.Http;
using HenryKam.SlackWhereIs.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.IO;
using System.Text;

namespace HenryKam.SlackWhereIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;
        private IBackgroundTaskQueue _taskQueue { get; }
        private ExchangeProvider _exchangeProvider { get; }

        private SlackAuthorizationActionFilter _slackVerificationProvider { get; }

        public BotController(ILogger<BotController> logger, IBackgroundTaskQueue taskQueue, SlackAuthorizationActionFilter slackVerificationProvider, ExchangeProvider exchangeProvider, BotProcessingService service)
        {
            _logger = logger;
            _slackVerificationProvider = slackVerificationProvider;
            _exchangeProvider = exchangeProvider;
            _taskQueue = taskQueue;
            service.StartAsync(new CancellationToken());
        }

        // POST: Search
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [SlackAuthorization]
        //[ValidateAntiForgeryToken]
        public IActionResult Search([FromForm] SlackRequest request, [FromServices] IServiceScopeFactory scopeFactory)
        {
            //Request.EnableBuffering();
            //Request.Body.Position = 0;
            //using (var reader = new StreamReader(
            //    Request.Body,
            //    encoding: Encoding.UTF8,
            //    detectEncodingFromByteOrderMarks: false,
            //    bufferSize: 1024,
            //    leaveOpen: true))
            //        {
            //            var body = reader.ReadToEnd();
            //            // Do some processing with body

            //            // Reset the request body stream position so the next middleware can read it
            //            Request.Body.Position = 0;
            //}

            //bool getTimestamp = Request.Headers.TryGetValue("X-Slack-Request-Timestamp", out var timestamps);
            //if (!getTimestamp)
            //{
            //    return BadRequest("Timestamp missing from request");
            //}
            //else
            //{
            //    if (!_slackVerificationProvider.VerifySlackRequest(Request))
            //    {
            //        return Unauthorized("Request cannot be verified");
            //    }   
            //}

            // Validate empty
            if (string.IsNullOrEmpty(request.Text))
            {
                return Ok($"Please enter the name of the location to find.");
            }

            // Validate minimum length
            if (request.Text.Length < 3)
            {
                return Ok($"Please enter a name with at least 3 characters.");
            }

            _logger.LogInformation($"Queuing search task for {request.Text}");

            var searchTask = GetTaskForSearch(request.Text, new Uri(request.Response_Url), scopeFactory);
            //service.StartAsync(new CancellationToken());
            _taskQueue.QueueBackgroundWorkItem(searchTask);          
            // Processing, acknowledge receipt
            return Ok();                      
        }

        private Func<CancellationToken, Task> GetTaskForSearch(string searchText, Uri callbackEndpoint, IServiceScopeFactory scopeFactory)
        {
            Func<CancellationToken, Task> myFunc = (cancellationToken) => {

                using (var scope = scopeFactory.CreateScope())
                {

                    var locationRepository = scope.ServiceProvider.GetService<ILocationRepository>();
                    var httpClientFactory = scope.ServiceProvider.GetService<IHttpClientFactory>();

                    Console.WriteLine($"Starting search task for {searchText}");
                    var locations = locationRepository.GetLocationByName(searchText);
                    if (!locations.Any())
                    {
                        SlackCallbackHelper.PostCallback(callbackEndpoint, httpClientFactory, $"I couldn't find {searchText}");
                        return new Task(() => { });
                    }

                    SimpleSlackBlockResponse r = new SimpleSlackBlockResponse();
                    r.AddBlock(new SectionBlock() { Text = new PlainSectionBlockText("I found the following:", true) });
                    foreach (var location in locations)
                    {
                        bool available = false;
                        string availabilityDetails = " ";
                        if (location != null)
                        {
                            string locationType = "Other";
                            switch (location)
                            {
                                case MeetingRoom meetingRoom:
                                    locationType = "Meeting Room";

                                    if (meetingRoom.EmailAddress != null)
                                    {
                                        available = _exchangeProvider.GetAvailability(meetingRoom.EmailAddress, out availabilityDetails);
                                    }
                                    break;
                            }

                            var s = SlackResponseFactory.CreateWhereIsResponse(location.Name, location.Office, location.MapImageUrl == null ? null : new Uri(location.MapImageUrl), locationType, availabilityDetails == " " ? UseStatus.None : (available ? UseStatus.Available : UseStatus.InUse), availabilityDetails, floor: location.Floor);
                            r.Append(s);
                        }
                    }
                    r.AddBlock(new DividerBlock()).AddBlock(new ContextBlock().AddElement(new MarkdownContextElement("For any questions or issues, contact <@Henry>")));
                    SlackCallbackHelper.PostCallback(callbackEndpoint, httpClientFactory, r);
                    return new Task(() => { });
                }
            };

            return myFunc;
           
        }
    }
}