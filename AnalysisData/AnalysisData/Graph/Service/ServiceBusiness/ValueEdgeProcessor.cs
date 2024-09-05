﻿using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class ValueEdgeProcessor : IValueEdgeProcessor
{
    private readonly IAttributeEdgeRepository _attributeEdgeRepository;
    private readonly IValueEdgeRepository _valueEdgeRepository;
    private readonly int _batchSize;

    public ValueEdgeProcessor(
        IAttributeEdgeRepository attributeEdgeRepository,
        IValueEdgeRepository valueEdgeRepository,
        int batchSize = 100000)
    {
        _attributeEdgeRepository = attributeEdgeRepository;
        _valueEdgeRepository = valueEdgeRepository;
        _batchSize = batchSize;
    }

    public async Task ProcessEntityEdgeValuesAsync(
        ICsvReader csv, 
        IEnumerable<string> headers, 
        string from, 
        string to, 
        IEnumerable<EntityEdge> entityEdges)
    {
        var valueEdges = new List<ValueEdge>();

        foreach (var entityEdge in entityEdges)
        {
            if (!csv.Read()) continue;

            foreach (var header in headers)
            {
                if (header.Equals(from, StringComparison.OrdinalIgnoreCase) || 
                    header.Equals(to, StringComparison.OrdinalIgnoreCase)) 
                    continue;
                
                var attribute = await _attributeEdgeRepository.GetByNameAsync(header);
                if (attribute == null) continue;

                var valueString = csv.GetField(header);
                valueEdges.Add(new ValueEdge
                {
                    EntityId = entityEdge.Id,
                    AttributeId = attribute.Id,
                    Value = valueString
                });

                // Process batch if the size exceeds the limit
                if (valueEdges.Count >= _batchSize)
                {
                    await ProcessBatchAsync(valueEdges);
                    valueEdges.Clear();
                }
            }
        }

        // Process remaining records
        if (valueEdges.Any())
        {
            await ProcessBatchAsync(valueEdges);
        }
    }

    private async Task ProcessBatchAsync(List<ValueEdge> valueEdges)
    {
        await _valueEdgeRepository.AddRangeAsync(valueEdges);
    }
}