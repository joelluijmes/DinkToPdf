using DinkToPdf.Document;

namespace DinkToPdf.EventArgs
{
    public class FinishedArgs : System.EventArgs
    {
        public IDocument Document { get; set; }

        public bool Success { get; set; }
    }
}