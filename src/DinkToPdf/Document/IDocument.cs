using System.Collections.Generic;

namespace DinkToPdf.Document
{
    public interface IDocument : IPdfObject
    {
        IEnumerable<IPdfContent> GetObjects();
    }
}