using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;

namespace HenryKam.SlackWhereIs.Infrastructure.Exchange
{
    public class ExchangeProvider
    {
        private Uri _serverUrl { get; }
        private string _domain { get; }
        private string _username { get; }
        private string _password { get; }        

        public ExchangeProvider(ExchangeConfig config)
        {
            _serverUrl = new Uri(config.Server);
            _domain = config.Domain;
            _username = config.Username;
            _password = config.Password;
        }

        public bool GetAvailability(string smtpAddress, out string details)
        {

            DateTime currentDay = DateTime.UtcNow.Date;
            DateTime currentTime = DateTime.UtcNow;
            //"BVAN-Davie@absolute.com"
            ExchangeService service = new ExchangeService(TimeZoneInfo.Utc);
            service.Credentials = new WebCredentials(_username, _password, _domain);
            service.Url = _serverUrl;
            var avail = service.GetUserAvailability(new List<AttendeeInfo>() { new AttendeeInfo(smtpAddress) }, new TimeWindow(currentDay, currentDay.AddDays(1)), AvailabilityData.FreeBusyAndSuggestions).Result;

            if(avail.AttendeesAvailability != null)
            {
                var a = avail.AttendeesAvailability.FirstOrDefault();
                if(a != null)
                {
                    if(!a.CalendarEvents.Any())
                    {
                        details = "Available for the next few hours";
                        return true;
                    }

                    bool isBusy = false;
                    DateTime lastEndTime = currentTime;
                    foreach(var calendarEvent in a.CalendarEvents)
                    {
                        if(calendarEvent.StartTime <= currentTime)
                        {
                            // event is currently in progress
                            if (calendarEvent.EndTime > currentTime)
                            {
                                isBusy = true;
                                lastEndTime = calendarEvent.EndTime;
                            }
                        }
                        else
                        {
                            if(isBusy)
                            {
                                if(lastEndTime.AddSeconds(1) >= calendarEvent.StartTime)
                                {
                                    lastEndTime = calendarEvent.EndTime;
                                    continue;
                                }
                                else
                                {
                                    details = $"Busy until {calendarEvent.EndTime.ToLocalTime().ToString("h:mm tt")}";
                                    return false;
                                }
                            }
                            else
                            {
                                details = $"Available until {calendarEvent.StartTime.ToLocalTime().ToString("h:mm tt")}";
                                return true;
                            }
                        }
                    }

                    if(isBusy)
                    {
                        details = $"Busy until {lastEndTime.ToLocalTime().ToString("h:mm tt")}";
                        return false;
                    }
                    else
                    {
                        details = "Available for the next few hours";
                        return true;
                    }
                }
            }
            details = " ";
            return false;
        }

        public List<string> GetConfRoomsAddresses(string filter)
        {
            List<string> sRooms = new List<string>();
            DirectoryContext dcontext = new DirectoryContext(DirectoryContextType.Domain, _username, _password);
            var domain = Domain.GetDomain(dcontext);
            DirectoryEntry deDomain = domain.GetDirectoryEntry();
            DirectorySearcher dsRooms = new DirectorySearcher(deDomain);

            dsRooms.Filter = string.Format("(&(&(&(mailNickname={0}*)(objectcategory=person)(objectclass=user)(msExchRecipientDisplayType=7))))", filter);

            dsRooms.PropertiesToLoad.Add("sn");
            dsRooms.PropertiesToLoad.Add("mail");

            foreach (SearchResult sr in dsRooms.FindAll())
            {
                sRooms.Add(sr.Properties["mail"][0].ToString());
            }

            return sRooms;
        }

    }
}
