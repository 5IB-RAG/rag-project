﻿
namespace client.Parsing.Convertors
{
    public class MdConvertor : DocumentConvertor
    {
        public override async Task<string> GetTextAsync(FileStream stream)
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
