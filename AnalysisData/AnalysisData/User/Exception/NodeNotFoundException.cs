namespace AnalysisData.Exception;

public class NodeNotFoundException : System.Exception
{
    public NodeNotFoundException() : base(Resources.NodeNotFoundException)
    {
    }
}