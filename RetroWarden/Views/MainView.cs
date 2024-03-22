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
        private readonly VaultProxy _vaultProxy;
        private SortedDictionary<string, VaultItem> _vaultItems;
        private List<VaultFolder> _folders;
        private List<VaultCollection> _collections;
        private List<Organization> _organizations;
        private StringBuilder _aboutMessage;
        private VaultItem _tempItem;
        private bool _splashShown;
        private bool _boomerMode;
        
        public MainView() 
        {
            // Initialize Application Stack
            Application.Init();
            
            // Create about message.
            _aboutMessage = ViewUtils.CreateAboutMessageAscii();
            
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
            
            // Setup screen controls.
            InitializeComponent();
            
            // Set timer to launch splash.
            object timerToken = Application.MainLoop.AddTimeout (TimeSpan.FromMilliseconds(80), ShowSplashScreen);
            
            // Run the application loop.
            Application.Run(this);
        }

        private bool ShowSplashScreen(MainLoop arg)
        {
            if (!_splashShown)
            {
                // Show splash screen.
                SplashDialog splash = new SplashDialog(_aboutMessage.ToString());
                splash.Show();
                _splashShown = false;
            }

            return false;
        }
        
        #region UI Control Helpers
        private void LoadItemListView(SortedDictionary<string, VaultItem> items)
        {
            // Clear out any existing items.
            lvwItems.RemoveAll();
            
            // Get list of vault items from dictionary.
            List<VaultItem> itemList = items.Values.ToList();
            
            // Create list data source for listview.
            ItemListDataSource listSource = new ItemListDataSource(itemList);
            
            // Create handler for the OnSetMark event.
            listSource.OnSetMark += HandleListviewItemMarked;
            
            // Set the data to the listview.
            lvwItems.Source = (listSource);
            
            // Enable visibility of column header labels.
            lblItemName.Visible = true;
            lblUserId.Visible = true;
            lblOwner.Visible = true;
            
            // Set the first row as the selected row.
            lvwItems.SelectedItem = 0;
            lvwItems.SetFocus();
            
            // Set statusbar menus.
            UpdateStatusBarOptions();
        }
        private void LoadTreeView()
        {
            // Clear out any items.
            tvwItems.RemoveAll();
            
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

        private void SetOwnerNameForItems()
        {
            // Loop through organizations.
            foreach (Organization org in _organizations)
            {   
                // Find each item that belongs to that org.
                var orgItems = _vaultItems.Values.Where(i => i.OrganizationId == org.Id).ToList();
                
                // Update the owner name.
                orgItems.ForEach(i => i.ItemOwnerName = org.Name);
            }
            
            // Find items with no org.
            var userItems = _vaultItems.Values.Where(i => i.OrganizationId == null).ToList();
            
            // Update to show user is the owner.
            userItems.ForEach(i => i.ItemOwnerName = "Me");
        }
        
        private void ShowDetailForm(VaultItemDetailViewState state)
        {
            ItemDetailView view = CreateDetailView(state);
            
            // Show the view modal.
            view.Show();
            
            // Check to see if there is anything to do.
            if (view.OkPressed)
            {
                // Create list to hold item.
                List<VaultItem> items = new List<VaultItem> {view.Item};

                switch (state)
                {
                    case VaultItemDetailViewState.Create:
                        // Run the save worker.
                        RunSaveItemWorker(items, VaultItemSaveAction.Create, "Creating new vault item.");
                        break;
                    
                    case VaultItemDetailViewState.Edit:
                        // Run the save worker.
                        RunSaveItemWorker(items, VaultItemSaveAction.Update, "Updating vault item.");
                        break;
                }
            }
        }

        private ItemDetailView CreateDetailView(VaultItemDetailViewState state)
        {
            ItemDetailView retVal = null;
            
            // Check to see what type of item we have.
            switch (_tempItem.ItemType)
            {
                // Login
                case 1:
                    // Create item detail dialog.
                    retVal = new LoginDetailView(_tempItem, _folders, state);
                    break;
                
                // Note
                case 2:
                    retVal = new SecureNoteDetailView(_tempItem, _folders, state);
                    break;
                
                // Card
                case 3:
                    retVal = new CardDetailView(_tempItem, _folders, state);
                    break;
                
                // Identity
                case 4:
                    retVal = new IdentityDetailView(_tempItem, _folders, state);
                    break;
            }
            
            // Return the view.
            return retVal;
        }

        private void UpdateStatusBarOptions()
        {
            // Get the data source for list.
            ItemListDataSource items = (ItemListDataSource) lvwItems.Source;
            
            // Check to see how many rows have been selected.
            switch (items.MarkedItemCount)
            {
                // If 0 or 1, enable single item menu items.
                case 0:
                case 1:
                    // Add them to the status bar.
                    staMain.Items =
                        [stiNew, stiView, stiEdit, stiCopyUser, stiCopyPwd, stiFolderMove, stiCollectionMove];
                    break;

                // If > 1, enable only bulk menu items (add to folder/collection).
                default:
                    // Add them to the status bar.
                    staMain.Items = [stiNew, stiFolderMove, stiCollectionMove];
                    break;
            }

                // Redraw menu bar.
                staMain.SetNeedsDisplay();
        }
        #endregion
        
        #region Top Menu Handlers
        private void HandleConnectionRequest()
        {
            // Check to make sure we are not logged in already.
            if (_vaultProxy.IsLoggedIn)
            {
                MessageBox.ErrorQuery("Action failed","You are already logged in.", "Ok");
            }

            else
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
                            // Check to see if we have a list.
                            if (_vaultItems.Count > 0)
                            {
                                // Enable controls.
                                tvwItems.Enabled = true;
                                lvwItems.Enabled = true;
                                
                                // Run workers for folders and collections.
                                RunGetFoldersWorker();
                                RunGetCollectionsWorker();
                                RunGetOrganizationsWorker();

                                // Set item owner name.
                                SetOwnerNameForItems();

                                // Load controls with data.
                                LoadTreeView();
                                LoadItemListView(_vaultItems);
                            }
                        }
                    }

                    else
                    {
                        // Show error dialog.
                        MessageBox.ErrorQuery("Login failed.", _vaultProxy.ErrorMessage, "Ok");
                    }
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

        private void HandleBoomerMode()
        {
            // Toggle boomer mode.
            _boomerMode = !_boomerMode;
            
            // Set menu state.
            mnuMain.Menus[1].Children[0].Checked = _boomerMode;
            
            // Set this to the desired state
            Application.IsMouseDisabled = _boomerMode;
        }
        #endregion

        #region Treeview Event Handlers
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
        
        private void HandleTreeviewNodeActivated(ObjectActivatedEventArgs<ITreeNode> obj)
        {
            // Get the node that was double-clicked.
            ITreeNode activated = obj.ActivatedObject;
            
            // Get the node data for this node.
            Tuple<NodeType, string> nodeData = (Tuple<NodeType, string>) activated.Tag;
            
            // Make sure this is a leaf node.
            if (nodeData.Item1 == NodeType.Item)
            {
                // Update the selected item.
                _tempItem = _vaultItems[nodeData.Item2];
                
                // Call the detail form show.
                ShowDetailForm(VaultItemDetailViewState.Edit);
            }
        }
        #endregion
        
        #region Statusbar Menu Handlers
        private void HandleCreateKeypress()
        { 
            // Create new temp item.
            _tempItem = new VaultItem();
                
            // Try to establish context from the tree view.
            if (tvwItems.SelectedObject != null)
            {
                // Get the node which is selected.
                ITreeNode node = tvwItems.SelectedObject;

                // Check the node type from tag.
                Tuple<NodeType, string> nodeData = (Tuple<NodeType, string>)node.Tag;

                // Check to see if it is an item group.
                if (nodeData.Item1 == NodeType.ItemGroup)
                {
                    // Based on the string we know what type of item to create.
                    switch (node.Text)
                    {
                        case "Logins":
                            _tempItem.ItemType = 1;
                            break;
                        case "Secure Notes":
                            _tempItem.ItemType = 2;
                            break;
                        case "Cards":
                            _tempItem.ItemType = 3;
                            break;
                        case "Identites":
                            _tempItem.ItemType = 4;
                            break;
                    }
                }

                else if (nodeData.Item1 == NodeType.Item)
                {
                    // Get item.
                    _tempItem = _vaultItems[nodeData.Item2];
                }

                else
                {
                    // Have the user pick the item type.
                    SelectItemTypeDialog dialog = new SelectItemTypeDialog();
                    dialog.Show();

                    if (dialog.OkPressed)
                    {
                        _tempItem.ItemType = dialog.ItemType + 1;
                    }

                    else
                    {
                        // Just default to Login.
                        _tempItem.ItemType = 1;
                    }
                }
            }
            
            else
            {
                // Have the user pick the item type.
                SelectItemTypeDialog dialog = new SelectItemTypeDialog();
                dialog.Show();

                if (dialog.OkPressed)
                {
                    _tempItem.ItemType = dialog.ItemType + 1;
                }

                else
                {
                    // Just default to Login.
                    _tempItem.ItemType = 1;
                }
            }
            
            ShowDetailForm(VaultItemDetailViewState.Create);
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
            // Get the item data source.
            ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
            // Get the current item.
            VaultItem item = items.ItemList[lvwItems.SelectedItem];
            
            // Copy user name to clipboard.
            Clipboard.TrySetClipboardData(item.Login.UserName);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Username copied to clipboard.", "Ok");
        }
        
        private void HandlePwdCopyKeypress()
        {
            // Get the item data source.
            ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
            // Get the current item.
            VaultItem item = items.ItemList[lvwItems.SelectedItem];

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
            // Create folder list dialog
            SelectFolderDialog sfDialog = new SelectFolderDialog(_folders);
            sfDialog.Show();

            if (sfDialog.OkPressed)
            {
                // Get the datasource from the listview.
                ItemListDataSource markedItems = (ItemListDataSource)lvwItems.Source;
                
                // Get folder.
                VaultFolder folder = _folders[sfDialog.SelectedFolderIndex];
                
                // Save all items.
                
                // Redraw the current listview (because things may have moved).
                
                // Uncheck current selected rows.
            }
        }
        #endregion

        #region Listview Event Handlers
        private void HandleListViewOpenItem(ListViewItemEventArgs obj)
        {
            ShowDetailForm(VaultItemDetailViewState.Edit);
        }
        
        private void HandleListviewItemMarked(object sender, EventArgs e)
        {
            // Get the item data source.
            ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
            // Get the current item.
            _tempItem = items.ItemList[lvwItems.SelectedItem];
            
            // Update the status bar options.
            UpdateStatusBarOptions();
        }

        private void HandleListViewSelectedItemChanged(ListViewItemEventArgs obj)
        {
            // Get the item data source.
            ItemListDataSource items = (ItemListDataSource)lvwItems.Source;
            
            // Get the current item.
            _tempItem = items.ItemList[lvwItems.SelectedItem];
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

        private void RunSaveItemWorker(List<VaultItem>items , VaultItemSaveAction saveType, string dialogMessage)
        {
            SaveItemWorker worker = new SaveItemWorker(_vaultProxy, saveType, items, dialogMessage);
            
            // Run the worker.
            worker.Run();
            
            // Check the save type.
            switch (saveType)
            {
                case VaultItemSaveAction.Create:
                
                    // For create, put new item in item list.
                    _vaultItems.Add(worker.Results[0].Id, worker.Results[0]);
                    break;
                
                case VaultItemSaveAction.Update:
                        
                    // For update, loop through items to replace existing items in list.
                    foreach (VaultItem item in worker.Results)
                    {
                        // Replace existing item.
                        _vaultItems[item.Id] = item;
                    }
                    break;
                
                case VaultItemSaveAction.Delete:
                    
                    // For delete, remove item from list.
                    _vaultItems.Remove(worker.Results[0].Id);
                    break;
            } 
            
            // Update controls with new source data.
            ITreeNode node = this.tvwItems.SelectedObject;
            
            LoadTreeView();
            LoadItemListView(_vaultItems);
            this.tvwItems.SelectedObject = node;
        }
        #endregion
    }
}
