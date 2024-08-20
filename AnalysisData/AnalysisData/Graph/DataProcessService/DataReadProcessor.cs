using System.Globalization;
using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Repository.AccountRepository.Abstraction;
using AnalysisData.Repository.TransactionRepository.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.DataProcessService;

public class DataReadProcessor : IDataProcessor
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public DataReadProcessor(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<List<string>> ProcessDataAsync(Stream fileStream, string fileType)
    {
        var reader = new StreamReader(fileStream);
        var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));

        if (fileType == "account")
        {
            var accounts = csv.GetRecords<Account>().ToList();
            await _accountRepository.AddAccountsAsync(accounts);
        }
        else if (fileType == "transaction")
        {
            var transactions = csv.GetRecords<Transaction>().ToList();
            await _transactionRepository.AddTransactionsAsync(transactions);
        }
        else
        {
            throw new ArgumentException("Invalid file type");
        }
        fileStream.Position = 0;
        return await ReadHeader(fileStream);
    }

    private async Task<List<string>> ReadHeader(Stream fileStream)
    {
        var reader = new StreamReader(fileStream);
        var headerLine = await reader.ReadLineAsync();
        if (string.IsNullOrEmpty(headerLine))
        {
            throw new FileNotFoundException();
        }
        var headers = headerLine.Split(',').ToList();
        return headers;
    }
}