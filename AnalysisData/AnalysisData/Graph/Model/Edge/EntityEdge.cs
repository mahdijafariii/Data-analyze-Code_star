﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.Graph.Model.Node;

namespace AnalysisData.Graph.Model.Edge;

public class EntityEdge
{
    [Key] 
    public int Id { get; set; }

    [Required] 
    public int EntityIDSource { get; set; }

    [ForeignKey("EntityIDSource")]
    public EntityNode SourceNode { get; set; }

    [Required] 
    public int EntityIDTarget { get; set; }

    [ForeignKey("EntityIDTarget")]
    public EntityNode TargetNode { get; set; }
}