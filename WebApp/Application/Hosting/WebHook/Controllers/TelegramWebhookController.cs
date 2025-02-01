using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApp.Application.Hosting.WebHook.Controllers
{
    [ApiController]
    [Route("api/telegram")]
    public class TelegramWebhookController : ControllerBase
    {
        private readonly ITelegramBotClient BotClient;
        private readonly UpdateDistributor UpdateDistr;

        public TelegramWebhookController(ITelegramBotClient botClient, UpdateDistributor updateDistr)
        {
            BotClient = botClient;
            UpdateDistr = updateDistr;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            try
            {
                await UpdateDistr.HandleUpdateAsync(update, new CancellationToken());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }

            return Ok();
        }
    }
}