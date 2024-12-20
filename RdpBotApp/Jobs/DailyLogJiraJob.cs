using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdpBotApp.Jobs
{
    public class DailyLogJiraJob : IJob
    {
        private readonly TelegramHttpClient _httpClient;

        private string BOT_ID;
        private string ROOM_ID;

        public DailyLogJiraJob(TelegramHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            BOT_ID = configuration["BotId"];
            ROOM_ID = configuration["DailyLogJiraJob"];

        }

        public Task Execute(IJobExecutionContext context)
        {
            var today = DateTime.UtcNow;
            if (today.DayOfWeek == DayOfWeek.Sunday || today.DayOfWeek == DayOfWeek.Saturday)
            {
                return Task.CompletedTask;

            }
            return _httpClient.SendMessageToBot(BOT_ID, ROOM_ID, "Log jira hoặc 50k.");
        }
    }
}
