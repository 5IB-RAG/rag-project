namespace client.Model.Dto
{
    public sealed class UploadDto
    {
        public IFormFileCollection FormFiles { get; set; }
        public string? MetaData { get; set; }

    }
}
