using System.Data;
using Terminal.Gui;
using Terminal.Gui.Trees;
using Retrowarden.Dialogs;
using Retrowarden.Models;
using Retrowarden.Utils;

namespace Retrowarden.Views;

public class MainView2 : Toplevel
{
    // UI Controls.
    private Toplevel _top;
    private Window _window;
    private MenuBar _menu;
    private FrameView _leftPane;
    private TreeView _vaultItemView;
    private FrameView _rightPane;
    private TableView _itemTableView;
    
    // Vault proxy reference.
    private VaultProxy _vaultProxy;
    
    public MainView2()
    {
        // Initialize Application Stack
        Application.Init();
        
        // Initialize vault proxy.
        _vaultProxy = new VaultProxy();
        
        // Setup screen controls.
        SetupScreen();

        // Run the application loop.
        Application.Run(_top);
    }

    private void SetupScreen()
    {
        // Create menus.
        _menu = new MenuBar(new MenuBarItem[]
        {
            new MenuBarItem("_File", new MenuItem[]
            {
                new MenuItem("_Connect...", "Connect to vault", HandleConnectionRequest, null, 
                    null, Key.C | Key.CtrlMask),
                new MenuItem("_Quit", "Quit Application", HandleQuitRequest, null, 
                    null, Key.Q | Key.CtrlMask)
            })
        });
        
        // Create window.
        _window = new Window("Retrowarden");
        
        // Crate frames.
        _leftPane = ViewUtils.CreateControl<FrameView>(0, 1, 20, 0, 0, 3, 
            false, true, null, null);
        _leftPane.Title = "Items";

        _rightPane = ViewUtils.CreateControl<FrameView>(20, 1, 0, 0, 0, 3, 
            false, true, null, null);
        _rightPane.Title = "All Vaults";

        // Create users list control.
        _vaultItemView = ViewUtils.CreateControl<TreeView>(0, 0, 0, 0, 0, 0, 
            true,true, null, null);
        
        // Create messages control.
        _itemTableView = ViewUtils.CreateControl<TableView>(0, 0, 0, 0, 0, 0, 
            true,true, null, null);
        
        _itemTableView.MultiSelect = true;
        _itemTableView.FullRowSelect = true;
        
        // Add controls to frames.
        _leftPane.Add(_vaultItemView);
        _rightPane.Add(_itemTableView);

        // Add containers to window view.
        _top = Application.Top;
        _window.Add(_menu);
        _window.Add(_leftPane);
        _window.Add(_rightPane);
        
        // Add window to top view.
        _top.Add(_window);
    }
    
    private void HandleConnectionRequest()
    {
        ConnectDialog connectDialog = new ConnectDialog();

        connectDialog.Show();

        if (connectDialog.OkPressed)
        {
            // Try to connect to server.
            _vaultProxy.Login(connectDialog.UserId, connectDialog.Password);
            
            // Check to see if the login was successful.
            if (_vaultProxy.ExitCode == "0")
            {
                // Try to fetch vault items.
                List<VaultItem> vaultItems = _vaultProxy.ListVaultItems();
                
                // Check to see if items were found.
                if (_vaultProxy.ExitCode == "0")
                {
                    LoadTreeView(vaultItems);
                    LoadItemTableView(vaultItems);
                }
            }
        }
    }

    private void HandleQuitRequest()
    {
        ConfirmDialog confirmDialog = new ConfirmDialog("Quit.  Are you sure?", 6,40,
            "_Ok", "Cance_l");
        
        confirmDialog.Show();
        
        if (confirmDialog.OkPressed)
        {
            // Logout of vault.
            _vaultProxy.Logout();
            
            // Close application.
            Application.RequestStop(_top);
        }
    }

    private void LoadItemTableView(List<VaultItem> items)
    {
        DataTable itemTable = new DataTable();
        
        // Add columns.
        itemTable.Columns.Add("Site Name");
        itemTable.Columns.Add("User Id");

        foreach (VaultItem item in items)
        {
            DataRow row = itemTable.NewRow();
            row["Site Name"] = item.ItemName;
            row["User Id"] = item.Login.UserName;
            itemTable.Rows.Add(row);
        }
        
        _itemTableView.Table = itemTable;
        _itemTableView.SetFocus();
    }

    private void LoadTreeView(List<VaultItem> items)
    {
        // Create root node.
        TreeNode root = new TreeNode("Bitwarden");
        
        // Get folders.
        List<VaultFolder> folders = _vaultProxy.ListFolders();
        
        // Create folders node.
        TreeNode folderNode = ViewUtils.CreateFoldersNode(folders, items);
        
        // Get collections.
        List<VaultCollection> collection = _vaultProxy.ListCollections();
        
        // Create collection node.
        TreeNode collectionNode = ViewUtils.CreateCollectionsNode(collection, items);
        
        // Create item nodes.
        TreeNode itemNodes = ViewUtils.CreateAllItemsNodes(items);
        
        // Add nodes to root.
        root.Children.Add(itemNodes);
        root.Children.Add(folderNode);
        root.Children.Add(collectionNode);
        
        // Add nodes to control.
        _vaultItemView.AddObject(root);
    }
}