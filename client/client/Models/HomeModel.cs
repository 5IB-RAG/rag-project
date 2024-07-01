using client.Model.Dto;
using System;

namespace client.Models
{
    public class HomeModel
    {
        public List<UserChatDto> Chats { get; set; }
        public List<DocumentDto> Documents { get; set; }
        public UploadDto UploadDocument { get; set; } 
        public UserChatDto SelectedChat { get; set; }
        public MessageDto Message { get; set; }
        public string NewChatName { get; set; }
    }
}
