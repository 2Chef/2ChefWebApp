using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/telegram")]
    internal class TelegramWebhookController : ControllerBase
    {
        private readonly ITelegramBotClient _botClient;

        public TelegramWebhookController(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update?.Message?.From is null) return BadRequest();
            await _botClient.SendMessage(update.Message.From.Id, "Hello");
            return Ok();
        }
    }
}