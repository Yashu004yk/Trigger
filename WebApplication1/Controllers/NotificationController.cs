//using Microsoft.AspNetCore.Mvc;

//namespace WebApplication1.Controllers
//{


//    [ApiController]
//    [Route("api/[controller]")]
//    public class NotificationController : Controller
//    {
//        private readonly NotificationListener _notificationListener;
//        [HttpGet]
//        public IActionResult Index()
//        {
//            return View();
//        }

//        public NotificationController(NotificationListener notificationListener)
//        {
//            _notificationListener = notificationListener;
//        }

//        [HttpGet("start-listening")]
//        public IActionResult StartListening()
//        {
//            _notificationListener.StartListening();
//            return Ok("Listening for notifications...");
//        }
//    }

//}
