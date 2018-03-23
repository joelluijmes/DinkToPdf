using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using DinkToPdf.Document;
using DinkToPdf.Settings;

namespace DinkToPdf.TestWebServer.Controllers
{
    [Route("api/[controller]")]
    public class ConvertController : Controller
    {
        private IPdfConverter _converter;

        public ConvertController(IPdfConverter converter)
        {
            _converter = converter;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var doc = new PdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A3,
                    Orientation = Orientation.Landscape,
                },

                Objects = {
                    new PdfPage()
                    {
                        Page = "http://google.com/",
                    },
                     new PdfPage()
                    {
                        Page = "https://github.com/",
                         
                    }
                }
            };
           
            byte[] pdf = _converter.Convert(doc);


            return new FileContentResult(pdf, "application/pdf");
        }
    }
}
