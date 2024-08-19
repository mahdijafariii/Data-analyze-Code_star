﻿using System.ComponentModel.DataAnnotations;

namespace AnalysisData.EAV.Model;

public class AttributeNode
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}