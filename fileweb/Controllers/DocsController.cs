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


        [HttpGet("{category1?}")]
        public async Task<IActionResult> Index(string category1)
        {
            var category1List = await this._docAccessor.GetAllCategory1().ConfigureAwait(false);

            if (!category1List.Any())
                return StatusCode(500, "CONFIG ERROR");

            if (string.IsNullOrEmpty(category1))
            {
                category1 = category1List.OrderBy(c => c).FirstOrDefault();

                return Redirect($"~/docs/{category1}");
            }
            else
            {
                var docs = await this._docAccessor.GetDocDtos(category1).ConfigureAwait(false);

                var model = docs.Any() ? docs.GetDocsModel().GetDocsViewModel(category1List) : null;

                return View(model);
            }
        }

    }
}
