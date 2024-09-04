using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class NodeValidator : INodeValidator
{
    public Task ValidateNodesExistenceAsync(EntityNode? fromNode, EntityNode? toNode, string entityFrom, string entityTo)
    {
        var missingNodeIds = new List<string>();

        if (fromNode == null) missingNodeIds.Add(entityFrom);
        if (toNode == null) missingNodeIds.Add(entityTo);

        if (missingNodeIds.Any())
        {
            throw new NodeNotFoundInEntityEdgeException(missingNodeIds);
        }

        return Task.CompletedTask;
    }
}