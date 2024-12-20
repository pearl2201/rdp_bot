using Quartz;

namespace RdpBotApp.Jobs
{
    public class DailyMeetJob : IJob
    {
        private readonly TelegramHttpClient _httpClient;

        private readonly string BOT_ID;
        private readonly string ROOM_ID;
        private DateTime startDailyMeetDay;
        public DailyMeetJob(TelegramHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            startDailyMeetDay = DateTime.Parse(configuration["StartRetroDate"]);
            BOT_ID = configuration["BotId"];
            ROOM_ID = configuration["DailyMeeting"];
        }

        public Task Execute(IJobExecutionContext context)
        {
            var today = DateTime.UtcNow;

            if (today.DayOfWeek == DayOfWeek.Sunday || today.DayOfWeek == DayOfWeek.Saturday)
            {
                return Task.CompletedTask;

            }
            else if ((today.Date - startDailyMeetDay.Date).TotalDays % 14 == 0)
            {
                return _httpClient.SendMessageToBot(BOT_ID, ROOM_ID, "Hôm nay theo lịch là start sprint mọi người ơi. (Với tí mọi người nhớ log jira nhé)");
            }
            return _httpClient.SendMessageToBot(BOT_ID, ROOM_ID, "Đi họp daily thôi mọi người ơi. (Với tí mọi người nhớ log jira nhé)");
        }
    }
}
