using System;
using DinkToPdf.Document;
using DinkToPdf.EventArgs;

namespace DinkToPdf
{
    public interface IPdfConverter
    {
        byte[] Convert(IDocument document);

        event EventHandler<PhaseChangedArgs> PhaseChanged;

        event EventHandler<ProgressChangedArgs> ProgressChanged;

        event EventHandler<FinishedArgs> Finished;

        event EventHandler<ErrorArgs> Error;

        event EventHandler<WarningArgs> Warning;
    }
}