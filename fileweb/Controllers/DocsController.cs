using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using EnsureThat;
using fileweb.Models;

namespace fileweb.Controllers
{
    [Route("/docs")]
    public class DocsController
        : Controller
    {
        private readonly IDocAccessor _docAccessor;
        public DocsController(IDocAccessor docAccessor)
        {
            Ensure.That(docAccessor, nameof(docAccessor)).IsNotNull();

            this._docAccessor = docAccessor;
        }

        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            var category1List = await this._docAccessor.GetAllCategory1().ConfigureAwait(false);

            return View("List", category1List.ToArray());
        }


        [HttpGet("{category1}")]
        public async Task<IActionResult> Index(string category1)
        {

            var docs = await this._docAccessor.GetDocDtos(category1).ConfigureAwait(false);

            var model = docs.GetDocsModel().GetDocsViewModel();

            return View(model);
        }

    }
}
