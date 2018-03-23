using DinkToPdf.Document;

namespace DinkToPdf.EventArgs
{
    public class ProgressChangedArgs : System.EventArgs
    {
        public IDocument Document { get; set; }

        public string Description { get; set; }
    }
}