namespace AnalysisData.Dtos.GraphDto.FileDto;

public class AccessFileToUserDto
{
    public IEnumerable<string> UserGuidIds { get; set; }
    public int FileId { get; set; }
}