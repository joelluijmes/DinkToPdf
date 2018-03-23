using DinkToPdf.Document;

namespace DinkToPdf.EventArgs
{
    public class ErrorArgs : System.EventArgs
    {
        public IDocument Document { get; set; }

        public string Message { get; set; }
    }
}