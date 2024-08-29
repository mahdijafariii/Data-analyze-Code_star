namespace AnalysisData.EAV.Dto;

public class AccessFileToUserDto
{
    public IEnumerable<string> UserGuidIds { get; set; }
    public int FileId { get; set; }
}