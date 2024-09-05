using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Graph.Model.Category;

public class Category
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
}
