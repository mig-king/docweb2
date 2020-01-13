using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;
using fileweb.Services;
using Microsoft.AspNetCore.Mvc;

namespace fileweb.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAnnouncementService _announcementService;

        public AdminController(IAnnouncementService announcementService)
        {
            Ensure.That(announcementService, nameof(announcementService)).IsNotNull();
            _announcementService = announcementService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Announcement")]
        public IActionResult Announcement()
        {

            return View("Announcement");
        }
    }
}