namespace server.Model.Dto
{
    public class UploadDTO
    {
        public IFormFileCollection FormFiles { get; set; }
        public string MetaData { get; set; }

        public UploadDTO(IFormFileCollection formFiles, string metaData)
        {
            FormFiles = formFiles;
            MetaData = metaData;
        }
    }
}
