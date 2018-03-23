using DinkToPdf.Document;

namespace DinkToPdf.EventArgs
{
    public class WarningArgs : System.EventArgs
    {
        public IDocument Document { get; set; }

        public string Message { get; set; }
    }
}