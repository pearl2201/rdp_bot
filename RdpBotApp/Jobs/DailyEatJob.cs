using Microsoft.Extensions.Options;
using Quartz;

namespace RdpBotApp.Jobs
{
    public class DailyEatJob : IJob
    {
        private readonly TelegramHttpClient _httpClient;

        private string BOT_ID;
        private string ROOM_ID;
        public DailyEatJob(TelegramHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            BOT_ID = configuration["BotId"];
            ROOM_ID = configuration["DailyEat"];
        }

        public Task Execute(IJobExecutionContext context)
        {
            var options = new List<string> {
                "Bún riêu",
                "bún chả",
                "bún cá chấm",
                "cơm gà đầu ngõ",
                "cơm chị đẹp",
                "bánh mỳ chảo ao sen",
                "bún bò huế",
                "bún ốc",
                "nem nướng Nha Trang",
                "bún trộn nam bộ",
                "cơm đảo gà",
                "bún đậu mắm tôm",
            };
            var rng = new Random();
            var id = rng.Next(options.Count);
            var food = options[id];
            var today = DateTime.UtcNow;
            if (today.DayOfWeek == DayOfWeek.Sunday || today.DayOfWeek == DayOfWeek.Saturday)
            {
                return Task.CompletedTask;

            }
            return _httpClient.SendMessageToBot(BOT_ID, ROOM_ID, $"Nhà mình hôm nay có thể thử món {food}");
        }
    }
}
