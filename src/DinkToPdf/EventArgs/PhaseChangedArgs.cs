using DinkToPdf.Document;

namespace DinkToPdf.EventArgs
{
    public class PhaseChangedArgs : System.EventArgs
    {
        public IDocument Document { get; set; }

        public int PhaseCount { get; set; }

        public int CurrentPhase { get; set; }

        public string Description { get; set; }
    }
}