using System.ComponentModel;
using Retrowarden.Utils;
using Retrowarden.Dialogs;

namespace Retrowarden.Workers;

public class LogoutWorker : RetrowardenWorker
{
    public LogoutWorker(VaultProxy proxy) : base(proxy, "Logging out...")
    {
        // Run the logout method.
        _worker.DoWork += (s, e) => 
        {
            // Execute the login method.
            _vaultProxy.Logout();
        };
            
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) => 
        {   
            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop) 
            {
                //Close the dialog
                _workingDialog.Hide();
            }

            _worker = null;
        };    
    }
}