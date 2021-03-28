using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
		private readonly IMailService mailService;
		private readonly IDutchRepository repository;

		public AppController(IMailService mailService, IDutchRepository repository)
		{
			this.mailService = mailService;
			this.repository = repository;
		}
        public IActionResult Index()
		{
           // throw new InvalidProgramException("Bad things happen to good developers :)");
            return View();
		}

        [HttpGet("contact")]
        public IActionResult Contact()
		{
            return View();
		}
       
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
			if (ModelState.IsValid)
			{
                //send email
                mailService.SendMessage("a@a.a", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent!";
                ModelState.Clear();
			}
			
            return View();
        }

        public IActionResult About()
		{
            ViewBag.Title = "About Us";
            return View();
        }

        public IActionResult Shop()
		{
            //ViewBag.Title = "Shop";
            var results = repository.GetAllProducts();
            return View(results);
		}
    }
}
