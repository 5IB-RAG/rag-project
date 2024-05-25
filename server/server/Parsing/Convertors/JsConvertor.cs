
namespace server.Parsing.Convertors
{
    public class JsConvertor : DocumentConvertor
    {
        public async override Task<string> GetTextAsync(FileStream stream)
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
