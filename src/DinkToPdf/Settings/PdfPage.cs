using System.Text;
using DinkToPdf.Document;

namespace DinkToPdf.Settings
{
    public class PdfPage : IPdfContent
    {
        /// <summary>
        ///     The URL or path of the web page to convert, if "-" input is read from stdin. Default = ""
        /// </summary>
        [WkHtml("page")]
        public string Page { get; set; }

        /// <summary>
        ///     Should external links in the HTML document be converted into external pdf links. Default = true
        /// </summary>
        [WkHtml("useExternalLinks")]
        public bool? UseExternalLinks { get; set; }

        /// <summary>
        ///     Should internal links in the HTML document be converted into pdf references. Default = true
        /// </summary>
        [WkHtml("useLocalLinks")]
        public bool? UseLocalLinks { get; set; }

        /// <summary>
        ///     Should we turn HTML forms into PDF forms. Default = false
        /// </summary>
        [WkHtml("produceForms")]
        public bool? ProduceForms { get; set; }

        /// <summary>
        ///     Should the sections from this document be included in the outline and table of content. Default = false
        /// </summary>
        [WkHtml("includeInOutline")]
        public bool? IncludeInOutline { get; set; }

        /// <summary>
        ///     Should we count the pages of this document, in the counter used for TOC, headers and footers. Default = false
        /// </summary>
        [WkHtml("pagesCount")]
        public bool? PagesCount { get; set; }

        public string HtmlContent { get; set; }

        public WebSettings WebSettings { get; set; } = new WebSettings();

        public PdfHeader PdfHeader { get; set; } = new PdfHeader();

        public PdfFooter PdfFooter { get; set; } = new PdfFooter();

        public PdfRemotePage PdfRemotePage { get; set; } = new PdfRemotePage();

        public byte[] GetContent()
        {
            return HtmlContent == null 
                ? new byte[0] 
                : Encoding.UTF8.GetBytes(HtmlContent);
        }
    }
}