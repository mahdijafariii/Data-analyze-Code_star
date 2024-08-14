namespace AnalysisData.JwtService;
public static class ActionRole
{
    public static List<string> GetActionByRole(string role)
    {
        var actions = new Dictionary<string, List<string>>
        {
            { "admin", new List<string> { "Create User", "Delete User", "Update User", "Get User",
                "Data analysis", "Add Data", "Update Data", "Delete Data", "Read Data" } },
            { "Manager", new List<string> { "Data analysis", "Add Data", "Update Data", "Delete Data", "Read Data" } },
            { "data analyst", new List<string> { "Data analysis", "Read Data" } }
        };
        return actions.TryGetValue(role, out var actionList) ? actionList : new List<string>();
    }
}