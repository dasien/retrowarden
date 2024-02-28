using Retrowarden.Utils;
using Retrowarden.Models;

namespace Retrowarden.Workers;

public class GetFoldersWorker : RetrowardenWorker
{
    // Member variables.
    private List<VaultFolder> _folders;
    
    public GetFoldersWorker(VaultProxy proxy) : base(proxy, "Syncing Folders...")
    {
        // Add work event handler.
        _worker.DoWork += (s, e) => 
        {
            // Try to fetch folders.
            e.Result = _vaultProxy.ListFolders();
        };
            
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Set member variable to result.
            _folders = (List<VaultFolder>) e.Result;
            
            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop) 
            {
                //Close the dialog
                _workingDialog.Hide();
            }

            _worker = null;
        };    
    }

    public List<VaultFolder> Folders
    {
        get
        {
            return _folders;
        }
    }
}