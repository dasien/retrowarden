namespace Retrowarden.Utils
{
    public sealed class CodeListManager
    {
        private static CodeListManager _instance;
        private static Dictionary<string, List<CodeListItem>> _codeLists;
        private static readonly object _lock = new object();

        CodeListManager()
        {
            // Create list holder.
            _codeLists = new Dictionary<string, List<CodeListItem>>();
            
            // Load lists.
            LoadLists();
        }

        public static CodeListManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CodeListManager();
                    }
                }
            }
            return _instance;
        }

        private void LoadLists()
        {
            // Create list of card brands.
            _codeLists.Add("CardBrands", new List<CodeListItem>
            {
                new CodeListItem("Visa", "Visa"),
                new CodeListItem("Mastercard", "Mastercard"),
                new CodeListItem("American Express", "American Express"),
                new CodeListItem("Discover", "Discover"),
                new CodeListItem("Diners Club", "Diners Club"),
                new CodeListItem("JCB", "JCB"),
                new CodeListItem("Maestro", "Maestro"),
                new CodeListItem("UnionPay", "UnionPay"),
                new CodeListItem("RuPay", "RuPay"),
                new CodeListItem("Other", "Other"),
            });
            
            // Create list of expiration months.
            _codeLists.Add("ExpiryMonths", new List<CodeListItem>
            {
                new CodeListItem("1", "1 - January"),
                new CodeListItem("2", "2 - February"),
                new CodeListItem("3", "3 - March"),
                new CodeListItem("4", "4 - April"),
                new CodeListItem("5", "5 - May"),
                new CodeListItem("6", "6 - June"),
                new CodeListItem("7", "7 - July"),
                new CodeListItem("8", "8 - August"),
                new CodeListItem("9", "9 - September"),
                new CodeListItem("10", "10 - October"),
                new CodeListItem("11", "11 - November"),
                new CodeListItem("12", "12 - December")
            });
            
            // Create list of titles.
            _codeLists.Add("Titles", new List<CodeListItem>
            {
                new CodeListItem("Mr", "Mr"),
                new CodeListItem("Mrs", "Mrs"),
                new CodeListItem("Ms", "Ms"),
                new CodeListItem("Mx", "Mx"),
                new CodeListItem("Dr", "Dr")
            });
            
            _codeLists.Add("MatchDetections", new List<CodeListItem>
            {
                new CodeListItem("0", "Base Domain"),
                new CodeListItem("1", "Host"),
                new CodeListItem("2", "Starts With"),
                new CodeListItem("3", "Regular Expression"),
                new CodeListItem("4", "Exact Match"),
                new CodeListItem("5", "Never"),
                new CodeListItem(null, "Default")
            });
        }

        public List<CodeListItem> GetList(string listName)
        {
            return _codeLists[listName];
        }
    }
}