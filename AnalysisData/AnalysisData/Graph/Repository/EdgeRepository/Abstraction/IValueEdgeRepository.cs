﻿using AnalysisData.Graph.Model.Edge;

namespace AnalysisData.Graph.Repository.EdgeRepository.Abstraction;

public interface IValueEdgeRepository
{
    Task AddAsync(ValueEdge entity);
    Task AddRangeAsync(IEnumerable<ValueEdge> valueEdges);
    Task<IEnumerable<ValueEdge>> GetAllAsync();
    Task<ValueEdge> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}