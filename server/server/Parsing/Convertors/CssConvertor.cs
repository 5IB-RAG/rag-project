
namespace server.Parsing.Convertors
{
    public class CssConvertor : DocumentConvertor
    {
        public async override Task<string> GetTextAsync(Stream stream)
        {
            try
            {
                using (StreamReader reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new FileLoadException(ex.Message);
            }
        }
    }
}
