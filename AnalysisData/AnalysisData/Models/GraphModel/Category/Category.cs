using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Models.GraphModel.Category;

public class Category
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
}
