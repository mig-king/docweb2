using EnsureThat;
using fileweb.Models;
using fileweb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace fileweb.Controllers
{
    [Route("/")]
    public class DocsController
        : Controller
    {
        private readonly IDocAccessor _docAccessor;
        private readonly IAnnouncementService _announcementService;
        public DocsController(IDocAccessor docAccessor, IAnnouncementService announcementService)
        {
            Ensure.That(docAccessor, nameof(docAccessor)).IsNotNull();
            Ensure.That(announcementService, nameof(announcementService)).IsNotNull();

            this._docAccessor = docAccessor;
            _announcementService = announcementService;
        }

        public async Task<IActionResult> Index()
        {
            var categoryList = await this._docAccessor.GetAllCategories().ConfigureAwait(false);

            if (!categoryList.Any())
                return StatusCode(500, "CONFIG ERROR");

            return View(new DocsHomeViewModel()
            {
                CategoryList = categoryList,
                Anncouncement = await _announcementService.GetLatestAnnouncement()
            });
        }

        [HttpGet("/docs/{category2?}")]
        public async Task<IActionResult> Category(string category2)
        {
            var categoryList = await this._docAccessor.GetAllCategories().ConfigureAwait(false);

            if (!categoryList.Any())
                return StatusCode(500, "CONFIG ERROR");

            if (string.IsNullOrEmpty(category2))
            {
                category2 = categoryList.Select(c => c.Category2).OrderBy(c => c).FirstOrDefault();

                return Redirect($"~/docs/{category2}");
            }
            else
            {
                var docs = await this._docAccessor.GetDocDtos(category2).ConfigureAwait(false);

                var model = docs.Any() ? docs.GetDocsModel().GetDocsViewModel(categoryList) : null;

                return View("Category", model);
            }
        }
    }
}
