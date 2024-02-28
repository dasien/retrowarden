using System.Text;
using Terminal.Gui;
using Terminal.Gui.Trees;
using Retrowarden.Dialogs;
using Retrowarden.Models;
using Retrowarden.Utils;
using Retrowarden.Config;
using Retrowarden.Workers;

namespace Retrowarden.Views 
{
    public partial class MainView 
    {
        // Vault proxy reference.
        private VaultProxy _vaultProxy;
        private SortedDictionary<string, VaultItem> _vaultItems;
        private List<VaultFolder> _folders;
        private List<VaultCollection> _collections;
        private List<Organization> _organizations;
        private List<string> _selectedRows;
        private StringBuilder _aboutMessage;
        private VaultItem _tempItem;
        
        public MainView() 
        {
            // Initialize Application Stack
            Application.Init();
            
            // Load the configuration file.
            ConfigurationManager manager = ConfigurationManager.Instance;
            
            // Check to see if exe location has been set.
            if (string.IsNullOrEmpty(manager.GetConfig().CLILocation))
            {
                // The allowed list. 
                List<string> fileType = new List<string>();
                fileType.Add("exe");
        
                // Show file dialog.
                OpenDialog finder = new OpenDialog("Setup Retrowarden", "Please locate the bw.exe file to continue.",
                    fileType, OpenDialog.OpenMode.File);

                finder.AllowsMultipleSelection = false;
        
                Application.Run(finder);
                if (!finder.Canceled)
                {
                    // Check to see if a file was found.
                     if (finder.FilePath != null)
                    {
                        // Save exe location in config.
                        manager.GetConfig().CLILocation = (string)finder.FilePath;
                        manager.WriteConfig(manager.GetConfig());
                    }

                    else
                    {
                        // GTFO.
                        Environment.Exit(1);
                    }
                }

                else
                {
                    // GTFO.
                    Environment.Exit(1);
                }
            }
            
            // Initialize vault proxy.
            _vaultProxy = new VaultProxy(manager.GetConfig().CLILocation);
            
            // Initialize selected rows list.
            _selectedRows = new List<string>();
            
            // Create about message.
            CreateAboutMessageASCII();
            
            // Setup screen controls.
            InitializeComponent();

            // Run the application loop.
            Application.Run(this);
        }

        private void CreateAboutMessageASCII()
        {
            _aboutMessage = new StringBuilder ();
            _aboutMessage.AppendLine (@"A terminal.gui based client for Bitwarden");
            _aboutMessage.AppendLine (@"");			
            _aboutMessage.AppendLine (@"______     _                                  _            ");
            _aboutMessage.AppendLine (@"| ___ \   | |                                | |           ");
            _aboutMessage.AppendLine (@"| |_/ /___| |_ _ __ _____      ____ _ _ __ __| | ___ _ __  ");
            _aboutMessage.AppendLine (@"|    // _ \ __| '__/ _ \ \ /\ / / _` | '__/ _` |/ _ \ '_ \ ");
            _aboutMessage.AppendLine (@"| |\ \  __/ |_| | | (_) \ V  V / (_| | | | (_| |  __/ | | |");
            _aboutMessage.AppendLine (@"\_| \_\___|\__|_|  \___/ \_/\_/ \__,_|_|  \__,_|\___|_| |_|");
            _aboutMessage.AppendLine (@""); 
        }

        #region UI Control Helpers

        private void LoadItemListView(SortedDictionary<string, VaultItem> items)
        {
            // Get list of vault items from dictionary.
            List<VaultItem> itemList = items.Values.ToList();
            
            // Create list data source for listview.
            ItemListDataSource listSource = new ItemListDataSource(itemList);
            
            // Set the data to the listview.
            lvwItems.Source = (listSource);
        }
        private void LoadTreeView()
        {
            // Create root node.
            TreeNode root = new TreeNode("Bitwarden");
            root.Tag = new Tuple<NodeType, string>(NodeType.Root, null);
            
            // Create folders node.
            TreeNode folderNode = ViewUtils.CreateFoldersNode(_folders, _vaultItems);
            
            // Create collection node.
            TreeNode collectionNode = ViewUtils.CreateCollectionsNode(_collections, _vaultItems);
        
            // Create item nodes.
            TreeNode itemNodes = ViewUtils.CreateAllItemsNodes(_vaultItems);
        
            // Add nodes to root.
            root.Children.Add(itemNodes);
            root.Children.Add(folderNode);
            root.Children.Add(collectionNode);
        
            // Add nodes to control.
            this.tvwItems.AddObject(root);
        }
        
        private SortedDictionary<string, VaultItem> GetVaultItemsForTreeNode(ITreeNode node)
        {
            SortedDictionary<string, VaultItem> retVal = new SortedDictionary<string, VaultItem>();
            
            // Loop through child nodes.
            foreach (TreeNode child in node.Children)
            {
                // Get node tag.
                Tuple<NodeType, string> nodeData = (Tuple<NodeType, string>) child.Tag;
                
                // Check to see that the child node is a vault item.
                if (nodeData.Item1 == NodeType.Item)
                {
                    // Lookup node in item dictionary.
                    VaultItem item = _vaultItems[nodeData.Item2];

                    // Add to filtered list.
                    retVal.Add(item.Id, item);   
                }
            }
            
            // Return filtered list.
            return retVal;
        }

        private void ShowDetailForm(VaultItemDetailViewState state)
        {
            // Create item detail dialog.
            VaultItemDetailView detailView = new VaultItemDetailView(_tempItem, _folders, state);

            // Update title.
            switch (state)
            {
                case VaultItemDetailViewState.Create:
                    detailView.Title = "Create New Item";
                    break;

                case VaultItemDetailViewState.Edit:
                    detailView.Title = "Edit Item - " + _tempItem.ItemName;
                    break;

                case VaultItemDetailViewState.View:
                    detailView.Title = "View Item - " + _tempItem.ItemName;
                    break;
            }

            // Show the view modal.
            detailView.Show();
        }
        #endregion
        
        #region Proxy Workers
        private void RunLoginWorker(string userId, string password)
        {
            // Create new worker.
            LoginWorker worker = new LoginWorker(_vaultProxy, userId, password);
            
            // Execute task
            worker.Run();
       }
        
        private void RunLogoutWorker()
        {
            // Create new worker.
            LogoutWorker worker = new LogoutWorker(_vaultProxy);
            
            // Execute task.
            worker.Run();
        }
        
        private void RunGetItemsWorker()
        {
            // Create new worker.
            GetItemsWorker worker = new GetItemsWorker(_vaultProxy);
            
            // Execute task.
            worker.Run();
            
            // Get vault items.
            _vaultItems = worker.Items;
        }
        
        private void RunGetFoldersWorker()
        {
            // Create new worker.
            GetFoldersWorker worker = new GetFoldersWorker(_vaultProxy);
            
            // Execute task.
            worker.Run();
            
            // Get folders.
            _folders = worker.Folders;
        }
        
        private void RunGetCollectionsWorker()
        {
            // Create new worker.
            GetCollectionsWorker worker = new GetCollectionsWorker(_vaultProxy);
            
            // Execute task.
            worker.Run();
            
            // Get collections.
            _collections = worker.Collections;
        }
        
        private void RunGetOrganizationsWorker()
        {
            // Create new worker.
            GetOrganizationsWorker worker = new GetOrganizationsWorker(_vaultProxy);
            
            // Execute task.
            worker.Run();
            
            // Get organizations.
            _organizations = worker.Organizations;
        }
        #endregion
        
        #region Event Handlers
        private void HandleConnectionRequest()
        {
            ConnectDialog connectDialog = new ConnectDialog();

            connectDialog.Show();
            
            if (connectDialog.OkPressed)
            {
                // Run login worker and show working dialog.
                RunLoginWorker(connectDialog.UserId, connectDialog.Password);
                
                // Check to see if the login was successful.
                if (_vaultProxy.ExitCode == "0")
                {
                    // Run workers for items, folders, and collections.
                    RunGetItemsWorker();
                    
                    // Check to see if items were found.
                    if (_vaultProxy.ExitCode == "0")
                    {
                        // Run workers for folders and collections.
                        RunGetFoldersWorker();
                        RunGetCollectionsWorker();
                        RunGetOrganizationsWorker();
                        
                        // Load controls with data.
                        LoadTreeView();
                        LoadItemListView(_vaultItems);
                    }
                }

                else
                {
                    // Show error dialog.
                    MessageBox.ErrorQuery("Login failed.", _vaultProxy.ErrorMessage, "Ok");
                }
            }
        }

        private void HandleQuitRequest()
        {
            int response = MessageBox.Query("Confirm Action", "Quit.  Are you Sure?", "Ok", "Cancel");
            
            // Check to see if the user confirmed action.
            if (response == 0)
            {
                // Logout of vault.
                RunLogoutWorker();
                
                // Clear any clipboard contents.
                Clipboard.TrySetClipboardData("");
                
                // Close application.
                Application.RequestStop(this);
                Environment.Exit(Environment.ExitCode);
            }
        }

       private void HandleTreeviewSelectionChanged(object? sender, SelectionChangedEventArgs<ITreeNode> e)
        {
            // Get node that was selected.
            ITreeNode selected = e.NewValue;
            
            // Get the node data for this node.
            Tuple<NodeType, string> nodeData = (Tuple<NodeType, string>) selected.Tag;
            
            // Check to see if this node has children.
            if (nodeData.Item1 != NodeType.Item)
            {
                // Get the list of children.
                SortedDictionary<string, VaultItem> list = GetVaultItemsForTreeNode(selected);
                
                // Update tableview with scoped list.
                LoadItemListView(list);
            }
        }
        
        private void HandleHelpKeypress()
        {
            Console.WriteLine("Help Fired");
        }

        private void HandleViewItemKeypress()
        {
            // Call helper method.
            ShowDetailForm(VaultItemDetailViewState.View);
        }
        
        private void HandleEditItemKeypress()
        {
            ShowDetailForm(VaultItemDetailViewState.Edit);
        }
        private void HandleUserCopyKeypress()
        {
            // Get the current item from selected item list.
            string itemId = _selectedRows[0];

            // Get the item associated with this row.
            VaultItem item = _vaultItems[itemId];

            // Copy user name to clipboard.
            Clipboard.TrySetClipboardData(item.Login.UserName);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Username copied to clipboard.", "Ok");
        }
        
        private void HandlePwdCopyKeypress()
        {
            // Get the current item from selected item list.
            string itemId = _selectedRows[0];

            // Get the item associated with this row.
            VaultItem item = _vaultItems[itemId];

            // Copy password to clipboard.
            Clipboard.TrySetClipboardData(item.Login.Password);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Password copied to clipboard.", "Ok");
        }
        
        private void HandleCollectionMoveKeypress()
        {
            throw new NotImplementedException();
        }

        private void HandleFolderMoveKeypress()
        {
            throw new NotImplementedException();
        }
        
        private void HandleBoomerMode()
        {
            throw new NotImplementedException();
        }
        #endregion

        private void HandleListViewSelectedItemChanged(ListViewItemEventArgs obj)
        {
            // Hold a temp reference to current item.
            _tempItem = (VaultItem)obj.Value;
        }

        private void HandleListViewKeyUp(KeyEventEventArgs obj)
        {
            // Check to see which key was pressed.
            if (obj.KeyEvent.Key == Key.Space)
            {
                // Check to see if it is in the list of selected items.
                if (_selectedRows.Contains(_tempItem.Id))
                {
                    // Remove it (the user has unselected the row).  
                    _selectedRows.Remove(_tempItem.Id);
                }

                else
                {
                    // If not there, add it.
                    _selectedRows.Add(_tempItem.Id);    
                }
            
                // Check to see how many rows have been selected.
                switch (_selectedRows.Count)
                {
                    // If 0, disable menu items.
                    case 0:
                        // Add them to the status bar.
                        staMain.Items = [stiHelp];
                        break;
                
                    // If 1, enable menu items.
                    case 1:
                        // Add them to the status bar.
                        staMain.Items = [stiHelp, stiView, stiEdit, stiCopyUser, stiCopyPwd, stiFolderMove, stiCollectionMove];
                        break;
                
                    // If > 1, enable only bulk menu items (add to folder).
                    default:
                        // Add them to the status bar.
                        staMain.Items = [stiHelp, stiFolderMove, stiCollectionMove];
                        break;
                }        
                
                // Redraw menu bar.
                staMain.SetNeedsDisplay();
            }
        }

        private void HandleListViewOpenItem(ListViewItemEventArgs obj)
        {
            ShowDetailForm(VaultItemDetailViewState.Edit);
        }

        private void HandleListviewEnter(FocusEventArgs obj)
        {
            // Set the first row as the selected row.
            lvwItems.SelectedItem = 0;
        }
    }
}
