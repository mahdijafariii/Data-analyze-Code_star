using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.Model;

namespace AnalysisData.Graph.Model.File;

public class UserFile
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    [ForeignKey("UserId")] public User User { get; set; }

    public int FileId { get; set; }

    [ForeignKey("FileId")] public FileEntity FileEntity { get; set; }
}