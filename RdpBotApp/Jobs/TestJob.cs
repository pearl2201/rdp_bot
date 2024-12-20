using Quartz;

namespace RdpBotApp.Jobs
{
    public class TestJob : IJob
    {
        private readonly TelegramHttpClient _httpClient;

        private string BOT_ID;
        private string ROOM_ID;
        private readonly IConfiguration _configuration;
        private DateTime startDailyMeetDay;
        public TestJob(TelegramHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            startDailyMeetDay = DateTime.Parse(configuration["StartRetroDate"]);
            BOT_ID = configuration["BotId"];
            ROOM_ID = configuration["Test"];
        }

        public Task Execute(IJobExecutionContext context)
        {

            if (_configuration["Test"] == "false")
            {
                return Task.CompletedTask;
            }
            var today = DateTime.UtcNow;
            var diff = (today.Date - startDailyMeetDay.Date).TotalDays % 14;
            if (today.DayOfWeek == DayOfWeek.Sunday || today.DayOfWeek == DayOfWeek.Saturday)
            {
                return Task.CompletedTask;

            }
            return _httpClient.SendMessageToBot(BOT_ID, ROOM_ID, "Đi họp daily thôi mọi người ơi. (Với tí mọi người nhớ log jira nhé)");
        }
    }
}
