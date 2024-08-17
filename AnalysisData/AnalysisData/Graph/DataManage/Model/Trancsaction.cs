using CsvHelper.Configuration.Attributes;
using QuickGraph;

namespace AnalysisData.Graph.DataManage.Model;

public class Transaction 
{
    private IEdge<Account> _edgeImplementation;
    
    [Name("SourceAcount")]
    public string SourceAccount { get; set; }

    [Name("DestiantionAccount")]
    public string DestinationAccount { get; set; }
    
    public decimal Amount { get; set; }
    
    public string Date { get; set; }
    
    public string TransactionID { get; set; }
    
    public string Type { get; set; }

}