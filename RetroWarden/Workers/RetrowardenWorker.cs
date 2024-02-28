using Retrowarden.Utils;
using Retrowarden.Dialogs;
using System.ComponentModel;

namespace Retrowarden.Workers
{
    public class RetrowardenWorker
    {
        // Member variables.
        protected VaultProxy _vaultProxy;
        protected BackgroundWorker _worker;
        protected WorkingDialog _workingDialog;

        protected RetrowardenWorker(VaultProxy proxy, string dialogMessage)
        {
            _vaultProxy = proxy;
            _worker = new BackgroundWorker();
            _workingDialog = new WorkingDialog(dialogMessage);
        }
        public void Run()
        {
            // Run the worker.
            _worker.RunWorkerAsync ();
            
            // Show working dialog.
            _workingDialog.Show();
        }
    }    
}

