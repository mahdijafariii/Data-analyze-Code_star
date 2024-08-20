﻿using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.NodeRepository.Abstraction;

public interface IEntityNodeRepository
{
    Task AddAsync(EntityNode entity);
    Task<IEnumerable<EntityNode>> GetAllAsync();
    Task<EntityNode> GetEntityByNameAsync(string id);
    Task<List<EntityNode>> GetNodesOfEdgeList(List<string> nodeIdes);
    Task DeleteAsync(int id);
}