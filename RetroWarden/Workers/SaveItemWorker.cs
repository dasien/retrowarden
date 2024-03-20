using Retrowarden.Utils;
using Retrowarden.Models;

namespace Retrowarden.Workers
{
    public class SaveItemWorker : RetrowardenWorker
    {
        private readonly List<VaultItem> _items;
        private List<VaultItem> _results;
        
        public SaveItemWorker(VaultProxy proxy, VaultItemSaveAction action, List<VaultItem> items, string message) 
            : base(proxy, message)
        {
            // Store items list.
            _items = items;
            
            // Work results.
            _results = new List<VaultItem>();
            
            // Check to see what kind of action we are performing.
            switch (action)
            {
                case VaultItemSaveAction.Create:
                    HandleItemCreate();
                    break;
                
                case VaultItemSaveAction.Update:
                case VaultItemSaveAction.CollectionMove:
                case VaultItemSaveAction.FolderMove:
                    HandleItemEdit();
                    break;
                
                case VaultItemSaveAction.Delete:
                    HandleItemDelete();
                    break;
            }
                        
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

        private void HandleItemCreate()
        {
            _worker.DoWork += (s, e) => 
            {
                // Get first vault item in list (should be only one for this action.
                VaultItem item = _items[0];
                
                // Get encoded item string.
                string encodedItem = item.ToBase64EncodedString();

                // Execute the create method.
                _results.Add(_vaultProxy.CreateVaultItem(encodedItem));
            };
        }

        private void HandleItemEdit()
        {
            // In this case, we may have multiple actions.
            _worker.WorkerReportsProgress = true;
            
            _worker.DoWork += (s, e) =>
            {
                // Loop through items (should be only one in this case.
                for (int cnt = 0; cnt < _items.Count; cnt++)
                {
                    // Get the item.
                    VaultItem item = _items[cnt];
                    
                    // Get encoded item string.
                    string encodedItem = item.ToBase64EncodedString();

                    // Execute the edit method.
                    _results.Add(_vaultProxy.UpdateVaultItem(item.Id, encodedItem));
                    
                    // Update progress.
                    _worker.ReportProgress(cnt + 1);
                }
            };

            _worker.ProgressChanged += (s, e) =>
            {
                // Update label in working dialog.
                _workingDialog.ProgressMessage = $"{e.ProgressPercentage} of {_items.Count} completed.";
            };
        }

        private void HandleItemDelete()
        {
            _worker.DoWork += (s, e) =>
            {
                // Get first vault item in list (should be only one for this action.
                VaultItem item = _items[0];

                // Execute the delete method.
                _vaultProxy.DeleteVaultItem(item.Id);
                
                // Set results to item that was deleted.
                _results.Add(item);
            };
        }

        public List<VaultItem> Results
        {
            get
            {
                return _results;
            }
        }
    }    
}

