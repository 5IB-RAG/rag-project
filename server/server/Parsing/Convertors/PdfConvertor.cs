namespace client.Parsing.Convertors;

public class PdfConvertor : DocumentConvertor
{
    public override async Task<string> GetTextAsync(FileStream stream)
    {
        //Logica per prendere il testo;

        return "ciao";
    }
}