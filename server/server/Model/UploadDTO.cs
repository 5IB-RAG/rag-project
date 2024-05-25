namespace server.Model
{
    public class UploadDTO
    {
        public IFormFileCollection FormFiles { get; set; }
        public List<string> MetaData { get; set; }

        public UploadDTO(IFormFileCollection formFiles, List<string> metaData)
        {
            FormFiles = formFiles;
            MetaData = metaData;
        }
    }
}
