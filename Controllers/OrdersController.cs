using IShop.Model;
using IShop.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace IShop.Controllers
{
    [EnableCors("MyClient", PolicyName = "MyClient")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly BotSender _bot;
        private readonly MailSender _mailSender;
        public OrdersController(BotSender bot, MailSender mailSender)
        {
            _bot = bot;
            _mailSender = mailSender;
        }

        [HttpPost]
        public async Task<ActionResult> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            _bot.SendMessage($"{nameof(Product.Name)}: {order.Product.Name}\n" +
                $"{nameof(Order.FirstName)}: {order.FirstName}\n" +
                $"{nameof(Order.LastName)}: {order.LastName}\n" +
                $"{nameof(Order.Email)}: {order.Email}\n" +
                $"{nameof(Order.Address)}: {order.Address}\n" +
                $"{nameof(Order.Phone)}: {order.Phone}\n");
            await _mailSender.SendEmailAsync(order.Email, "Order details",
                $"<h2>Greetings {order.FirstName} {order.LastName}</h2>" +
                $"<p>We have received your order for {order.Product.Name}</p>" +
                $"<p>If you have any additional info, please contact us by email message on <a href='mailto:archivesexplorermail@gmail.com'>archivesexplorermail@gmail.com</a></p>");
            return NoContent();
        }
    }
}
