using Telegram.Bot;

namespace IShop.Services
{
    public class BotSender
    {
        private ITelegramBotClient _bot { get; set; }
        private long _chatId { get; set; }
        public BotSender(string token, long chatId)
        {
            _bot = new TelegramBotClient(token);
            _chatId = chatId;
        }
        public async void SendMessage(string message)
        {
            await _bot.SendTextMessageAsync(_chatId, message);
        }
    }
}
