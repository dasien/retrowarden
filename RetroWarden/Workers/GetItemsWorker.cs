using Retrowarden.Utils;
using Retrowarden.Models;

namespace Retrowarden.Workers;

public class GetItemsWorker : RetrowardenWorker
{
    // Member variables.
    private SortedDictionary<string, VaultItem> _items;
    
    public GetItemsWorker(VaultProxy proxy) : base(proxy, "Syncing Vault Items...")
    {
        // Add work event handler.
        _worker.DoWork += (s, e) =>
        {
            // Try to fetch vault items.
            e.Result = _vaultProxy.ListVaultItems();
        };
        
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Set member variable to result.
            _items = (SortedDictionary<string, VaultItem>) e.Result;
            
            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop) 
            {
                //Close the dialog
                _workingDialog.Hide();
            }
            
            // Get rid of worker.
            _worker = null;
        };    
    }
    
    public SortedDictionary<string, VaultItem> Items
    {
        get
        {
            return _items;
        }
    }
}