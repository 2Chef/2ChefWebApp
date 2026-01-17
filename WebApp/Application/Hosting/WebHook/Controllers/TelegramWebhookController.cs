using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace WebApp.Application.Hosting.WebHook.Controllers
{
    [ApiController]
    [Route("api/telegram")]
    public class TelegramWebhookController(UpdateDistributor updateDistr) : ControllerBase
    {
        private UpdateDistributor UpdateDistr { get; } = updateDistr;

        [HttpPost("update")]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            try
            {
                await UpdateDistr.HandleUpdateAsync(update, new CancellationToken());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}