namespace DinkToPdf.Document
{
    public interface IPdfContent : IPdfObject
    {
        byte[] GetContent();
    }
}