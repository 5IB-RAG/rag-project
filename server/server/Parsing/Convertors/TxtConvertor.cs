namespace server.Parsing.Convertors
{
    public class TxtConvertor : DocumentConvertor
    {
        public override async Task<string> GetTextAsync(Stream stream)
        {
            string text = "";
            try
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    text = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException(ex.Message);
            }

            return text;
        }
    }
}
