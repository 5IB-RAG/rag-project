using Pgvector;
using server.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using server.Model.Dto;

namespace server.Model;

public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? DataText { get; set; } //this can contain text that rag uses to give information to the ai
    public string Text { get; set; } = null!;
    public ChatRole Role { get; set; }
    public int ChatId { get; set; }
    public int? ResponseId { get; set; }
    public UserChat UserChat { get; set; } = null!;

    [Column(TypeName = "vector(1536)")]
    public Vector? Embedding { get; set; }

    public List<Document> UsedDocuments { get; set; } = null!;

    public MessageDto ToDto()
    {
        return new MessageDto() { Text = Text, Role = Role, ChatId = ChatId, UsedDocument = UsedDocuments.Select(doc => doc.ToDto()).ToList()};
    }
}