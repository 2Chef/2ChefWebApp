using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace WebApp.Application.Hosting.WebHook.Controllers
{
    [ApiController]
    [Route("api/telegram")]
    public class TelegramWebhookController : ControllerBase
    {
        private UpdateDistributor UpdateDistr { get; }

        public TelegramWebhookController(UpdateDistributor updateDistr)
        {
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