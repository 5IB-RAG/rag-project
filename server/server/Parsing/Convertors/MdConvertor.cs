﻿
namespace server.Parsing.Convertors
{
    public class MdConvertor : DocumentConvertor
    {
        public override async Task<string> GetTextAsync(Stream stream)
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
