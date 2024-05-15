using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;


namespace client.Parsing.Convertors;

public class PdfConvertor : DocumentConvertor
{
    public override async Task<string> GetTextAsync(FileStream stream)
    {
        try
        {
            PdfReader reader = new PdfReader(stream);
            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();
            return text;
        }
        catch (Exception ex) 
        {
            throw new FileLoadException(ex.Message);
        }
    }
}