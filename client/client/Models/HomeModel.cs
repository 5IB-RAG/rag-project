using client.Model.Dto;
using System;

namespace client.Models
{
    public class HomeModel
    {
        public List<UserChat> Chats { get; set; }
        public List<Document> Documents { get; set; }
        public UploadDTO UploadDocument { get; set; } 
        public int selectedChat {  get; set; } = -1;
        public UserChat selectedChata { get; set; }
    }
}
