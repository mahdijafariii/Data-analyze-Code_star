using AnalysisData.Graph.Model.Node;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface INodeValidator
{
    Task ValidateNodesExistenceAsync(EntityNode? fromNode, EntityNode? toNode, string entityFrom, string entityTo);
}