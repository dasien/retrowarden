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
        private readonly List<string> _selectedRows;
        private StringBuilder _aboutMessage;
        private VaultItem _tempItem;
        private bool _splashShown;
        private bool _boomerMode;
        
        public MainView() 
        {
            // Initialize Application Stack
            Application.Init();
            
            // Create about message.
            CreateAboutMessageAscii();
            
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
        
        private void CreateAboutMessageAscii()
        {
            _aboutMessage = new StringBuilder();
            _aboutMessage.AppendLine (@" ******************               A terminal.gui based client for Bitwarden");
            _aboutMessage.AppendLine (@" ********       #**     ");			
            _aboutMessage.AppendLine (@" ********       #**     ______     _                                  _            ");
            _aboutMessage.AppendLine (@" ********       #**     | ___ \   | |                                | |           ");
            _aboutMessage.AppendLine (@" ********       ***     | |_/ /___| |_ _ __ _____      ____ _ _ __ __| | ___ _ __  ");
            _aboutMessage.AppendLine (@"  *******     ****      |    // _ \ __| '__/ _ \ \ /\ / / _` | '__/ _` |/ _ \ '_ \ ");
            _aboutMessage.AppendLine (@"   ******   ****        | |\ \  __/ |_| | | (_) \ V  V / (_| | | | (_| |  __/ | | |");
            _aboutMessage.AppendLine (@"    **********          \_| \_\___|\__|_|  \___/ \_/\_/ \__,_|_|  \__,_|\___|_| |_|");
            _aboutMessage.AppendLine (@"       ****             "); 
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
            
            // Enable visibility of column header labels.
            lblItemName.Visible = true;
            lblUserId.Visible = true;
            lblOwner.Visible = true;
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
            ItemDetailView view = null;
            
            // Check to see what type of item we have.
            switch (_tempItem.ItemType)
            {
                // Login
                case 1:
                    // Create item detail dialog.
                    view = new LoginDetailView(_tempItem, _folders, state);
                    break;
                
                // Note
                case 2:
                    view = new SecureNoteDetailView(_tempItem, _folders, state);
                    break;
                
                // Card
                case 3:
                    view = new CardDetailView(_tempItem, _folders, state);
                    break;
                
                // Identity
                case 4:
                    view = new IdentityDetailView(_tempItem, _folders, state);
                    break;
            }

            // Update title.
            switch (state)
            {
                case VaultItemDetailViewState.Create:
                    view.Title = "Create New Item";
                    break;

                case VaultItemDetailViewState.Edit:
                    view.Title = "Edit Item - " + _tempItem.ItemName;
                    break;

                case VaultItemDetailViewState.View:
                    view.Title = "View Item - " + _tempItem.ItemName;
                    break;
            }

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
        
        #region Event Handlers
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
            // Create folder list dialog
            SelectFolderDialog sfDialog = new SelectFolderDialog(_folders);
            sfDialog.Show();

            if (sfDialog.OkPressed)
            {
                // List to hold items to be saved.
                List<VaultItem> items = new List<VaultItem>();
                
                // Get folder.
                VaultFolder folder = _folders[sfDialog.SelectedFolderIndex];
                
                // Loop through ticked items in list.
                foreach (string id in _selectedRows)
                {
                    // Get item reference.
                    VaultItem item = _vaultItems[id];
                    
                    // Update each selected item in list with this folder.
                    item.FolderId = folder.Id;
                    
                    // Stage item to be saved.
                    items.Add(item);
                } 
                
                // Save all items.
                
                // Redraw the current listview (because things may have moved).
                
                // Uncheck current selected rows.
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

        private void HandleListViewSelectedItemChanged(ListViewItemEventArgs obj)
        {
            // Hold a temp reference to current item.
            _tempItem = (VaultItem) obj.Value;
        }

        private void HandleListViewKeyUp(KeyEventEventArgs obj)
        {
            // Check to see which key was pressed.
            if (obj.KeyEvent.Key == Key.Space)
            {
                // Check to make sure we have an item.
                if (_tempItem != null)
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

        private void HandleCreateKeypress()
        {
            ShowDetailForm(VaultItemDetailViewState.Create);
        }

        private void HandleListviewMouseClick(MouseEventArgs obj)
        {
            int rownum = lvwItems.SelectedItem;
            //obj.Handled = true;
            Console.Write("Here");
        }
    }
}
