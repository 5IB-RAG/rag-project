
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using System.Text;

namespace server.Parsing.Convertors
{
    public class DocxConvertor : DocumentConvertor
    {
        public override async Task<string> GetTextAsync(FileStream stream)
        {
            try
            {
                
                StringBuilder textBuilder = new StringBuilder();

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, false))
                {
                    Body body = wordDoc.MainDocumentPart.Document.Body;
                    foreach (var paragraph in body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
                    {
                        textBuilder.Append(paragraph.InnerText);
                        textBuilder.Append(Environment.NewLine);
                    }
                }
                return textBuilder.ToString();
            }
            catch(Exception ex)
            {
                throw new FileLoadException(ex.Message);
            }
        }
    }
}
