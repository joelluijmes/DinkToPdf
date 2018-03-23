using System.Collections.Generic;
using DinkToPdf.Settings;

namespace DinkToPdf.Document
{
    public class PdfDocument : IDocument
    {
        public PdfDocument()
        {
            Objects = new List<PdfPage>();
        }

        public List<PdfPage> Objects { get; }

        public GlobalSettings GlobalSettings { get; set; } = new GlobalSettings();

        public IEnumerable<IPdfContent> GetObjects()
        {
            return Objects;
        }
    }
}