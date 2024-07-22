using Microsoft.AspNetCore.Mvc;

namespace CodeNamesAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
