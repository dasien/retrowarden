using System.ComponentModel;
using Retrowarden.Utils;
using Retrowarden.Dialogs;

namespace Retrowarden.Workers;

public class LoginWorker : RetrowardenWorker
{
    public LoginWorker(VaultProxy proxy, string userId, string password) : base(proxy, "Logging in...")
    {
        // Run the login method.
        _worker.DoWork += (s, e) => 
        {
            // Execute the login method.
            _vaultProxy.Login(userId, password);
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