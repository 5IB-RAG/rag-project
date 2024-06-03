namespace client.Model.Dto
{
    public sealed class UploadDTO
    {
        public IFormFileCollection FormFiles { get; set; }
        public string? MetaData { get; set; }

    }
}
