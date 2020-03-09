using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace HenryKam.SlackWhereIs.Infrastructure.Slack
{
    public class SlackAuthorizationActionFilter : IAuthorizationFilter
    {
        private ILogger<SlackAuthorizationActionFilter> _logger { get; }
        private SlackConfig _config { get; }
        public SlackAuthorizationActionFilter(ILogger<SlackAuthorizationActionFilter> logger, SlackConfig config)
        {
            _logger = logger;
            _config = config;
        }

        public bool VerifySlackRequest(HttpRequest request)
        {
            _logger.LogInformation("Verifying that request is from Slack.");
            //byte[] keyByte = new ASCIIEncoding().GetBytes(_config.SigningSecret);
            //byte[] keyByte = StringToByteArray(_config.SigningSecret);
            byte[] keyByte = Encoding.UTF8.GetBytes(_config.SigningSecret);
            request.EnableBuffering();

            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                var body = reader.ReadToEndAsync().Result;
                request.Body.Position = 0;

                // Extract timestamp
                var timestamp = request.Headers["X-Slack-Request-Timestamp"].FirstOrDefault();
                var signature = request.Headers["X-Slack-Signature"].FirstOrDefault();
                var sigBaseString = $"v0:{timestamp}:{body}";

                using (HMACSHA256 hmac = new HMACSHA256(keyByte))
                {
                    return VerifyHash(hmac, sigBaseString, signature);
                }
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            var encoding = new UTF8Encoding();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(encoding.GetBytes(input));
            var comp = BitConverter.ToString(data).Replace("-", "").ToLower();

            // Return the hexadecimal string.
            return comp;
        }

        // Verify a hash against a string.
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = "v0=" + GetHash(hashAlgorithm, input);            

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {            
            bool isAuthorized = VerifySlackRequest(context.HttpContext.Request);
            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
