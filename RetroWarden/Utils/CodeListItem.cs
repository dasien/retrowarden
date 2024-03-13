namespace Retrowarden.Utils
{
    public class CodeListItem
    {
        public string? Index { get; set; }
        public string DisplayText { get; set; }

        public CodeListItem(string? index, string displayText)
        {
            Index = index;
            
            DisplayText = displayText;
        }
        
        public override string ToString()
        {
            return DisplayText;
        }
    }
}

