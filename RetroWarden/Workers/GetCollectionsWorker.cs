using Retrowarden.Utils;
using Retrowarden.Models;

namespace Retrowarden.Workers;

public class GetCollectionsWorker : RetrowardenWorker
{
    private List<VaultCollection> _collections;

    public GetCollectionsWorker(VaultProxy proxy) : base(proxy, "Syncing Collections...")
    {
        // Add work event handler.
        _worker.DoWork += (s, e) =>
        {
            // Try to fetch collections.
            e.Result = _vaultProxy.ListCollections();
        };

        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Set member variable to result.
            _collections = (List<VaultCollection>) e.Result;

            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop)
            {
                //Close the dialog
                _workingDialog.Hide();
            }

            _worker = null;
        };
    }
    
    public List<VaultCollection> Collections
    {
        get { return _collections; }
    }
}