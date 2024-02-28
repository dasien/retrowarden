using System.ComponentModel;
using Retrowarden.Utils;
using Retrowarden.Dialogs;
using Retrowarden.Models;

namespace Retrowarden.Workers;

public class GetOrganizationsWorker : RetrowardenWorker
{
    // Member variables.
    private List<Organization> _organizations;
    
    public GetOrganizationsWorker(VaultProxy proxy) : base(proxy, "Syncing Organizations...")
    {
        // Add work event handler.
        _worker.DoWork += (s, e) => 
        {
            // Try to fetch organizations.
            e.Result = _vaultProxy.ListOrganizations();
        };
            
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Set member variable to result.
            _organizations = (List<Organization>)e.Result;
            
            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop) 
            {
                //Close the dialog
                _workingDialog.Hide();
            }

            _worker = null;
        };    
    }

    public List<Organization> Organizations
    {
        get
        {
            return _organizations;
        }
    }
}