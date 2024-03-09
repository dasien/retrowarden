using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using Terminal.Gui;
using Retrowarden.Models;
using Retrowarden.Utils;

namespace Retrowarden.Views 
{
    public partial class VaultItemDetailView
    {
        private VaultItem _item;
        private readonly List<VaultFolder> _folders;
        private List<string> _matchDetections;
        private List<string> _folderNames;
        private List<View[]> _rowControls;
        private readonly VaultItemDetailViewState _viewState;
        private bool _okPressed;
        public VaultItemDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
        {
            // Set private variables.
            _item = item == null ? new VaultItem() : item;
            _viewState = state;
            _folders = folders;
            _okPressed = false;
            _folderNames = new List<string>();
            _matchDetections = new List<string>();
            
            InitializeComponent();
            
            // Update controls based on view state.
            SetupView();
        }
        
        private void SetupView()
        {
            // Initialize any list controls.
            InitializeLists();
            
            // Base setup what kind of view state we are in.
            if (_viewState == VaultItemDetailViewState.View)
            {
                // Load controls with current data only.
                LoadView();
                
                // Disable control state.
                DisableView();
            }
        }

        private void LoadView()
        {
            // Set current item values to controls.
            txtItemName.Text = _item.ItemName == null ? "" : _item.ItemName;
            txtUserName.Text = _item.Login.UserName == null ? "" : _item.Login.UserName;
            txtPassword.Text = _item.Login.Password == null ? "" : _item.Login.Password;
            txtTOTP.Text = _item.Login.TOTP == null ? "" : _item.Login.TOTP;
            tvwNotes.Text = _item.Notes == null ? "" : _item.Notes;
            
            // Check to see if there is a URI at all.
            if (_item.Login.URIs != null)
            {
                // Handle loading the list of other URIs.
                CreateUriListRows();
            }
        }

        private void DisableView()
        {
            //btnSave.Enabled = false;
            
            // Loop through the list of URIs and disable the delete buttons.
        }
        
        private void InitializeLists()
        {
            // Create list of match types.
            _matchDetections = new List<string>
            {
                "Default", "Base Domain", "Host", "Starts With", "Regular Expression",
                "Exact Match", "Never"
            };
            
            // Load combobox.
            for (int cnt = 0; cnt < _folders.Count; cnt++)
            {
                // Move to a simple collection the combo box can understand.
                _folderNames.Add(_folders[cnt].Name);
            }
        }
        
        private void CreateUriListRows()
        {
            // The List of an array of controls for each row.
            _rowControls = new List<View[]>();
            
            // Loop through the URI list.
            for (int cnt = 0; cnt < _item.Login.URIs.Count; cnt++)
            {
                // Get item reference.
                LoginURI uri = _item.Login.URIs[cnt];
                
                // Create controls for row.
                TextView txtURI = ViewUtils.CreateControl<TextView>(0, cnt, 30, 1, 0, 0,
                    true, true, uri.URI, null);
                Button btnCopyURI = ViewUtils.CreateControl<Button>(31, cnt, 8, 1, 0, 0,
                    true, true, "Copy", null, TextAlignment.Centered);
                Button btnGoURI = ViewUtils.CreateControl<Button>(40, cnt, 6, 1, 0, 0,
                    true, true, "Go", null, TextAlignment.Centered);
                ComboBox cboMatchURI = ViewUtils.CreateControl<ComboBox>(47, cnt, 30, 2, 0, 0,
                    true, true, "", null);
                Button btnDeleteURI = ViewUtils.CreateControl<Button>(78, cnt, 10, 1, 0, 0,
                    true, true, "Delete", null, TextAlignment.Centered);
                
                // For the delete button, assign an identifier for the row of controls
                btnDeleteURI.Data = new Guid().ToString();
                
                // Create event handlers for the buttons.
                btnCopyURI.Clicked += () =>
                {
                    // Copy password to clipboard.
                    Clipboard.TrySetClipboardData(txtURI.Text.ToString());

                    // Indicate data copied.
                    MessageBox.Query("Action Completed", "Copied URI to clipboard.", "Ok");
                };

                btnGoURI.Clicked += () =>
                {
                    Uri result;
                    // Check to see if we have a valid URI.
                    if (Uri.TryCreate(txtURI.Text.ToString(), UriKind.Absolute, out result))
                    {
                        Process.Start(new ProcessStartInfo(txtURI.Text.ToString()) { UseShellExecute = true });    
                    }

                    else
                    {
                        MessageBox.ErrorQuery("Action failed.", "This does not appear to be a valid URI", "Ok");
                    }
                    
                };

                btnDeleteURI.Clicked += () =>
                {
                    int rowIndex = 0;
                    
                    // Loop through list to find the index of the clicked on row.
                    for (int row = 0; row < _rowControls.Count; row++)
                    {
                        // Check the delete button GUID.
                        Button del = (Button) _rowControls.ElementAt(row)[4];
                        
                        // Check to see if the guids match.
                        if (string.Equals(del.Data, btnDeleteURI.Data))
                        {
                            // Store row.
                            rowIndex = row;
                        }
                    }
                    
                    // Call delete handler.
                    HandleRowDelete(rowIndex);

                    // Set scroll for redraw.
                    scrURIList.SetNeedsDisplay();
                };
                
                // Set list source and selected item for combo box.
                
                // Add views to row array
                View[] rowctl = [txtURI, btnCopyURI, btnGoURI, cboMatchURI, btnDeleteURI];
                _rowControls.Add(rowctl);
                
                // Add controls to URI scroll.
                scrURIList.Add(rowctl);
            }
            
            // Refresh URI scroll view.
            scrURIList.SetNeedsDisplay();
        }

        private void HandleRowDelete(int index)
        {
            // Check to see if the user is removing the last row in the list.
            if (index + 1 < _rowControls.Count)
            {
                // Loop through control list staring with the index + 1 row
                for (int row = (index + 1); row < _rowControls.Count; row++)
                {
                    // Update any control rows so that they 'move up' in the scroll list.
                    View[] rowControls = _rowControls.ElementAt(row);
                    
                    // Loop through the row controls.
                    foreach (View ctl in rowControls)
                    {
                        // Update the Y position up.
                        ctl.Y -= 1;
                    }
                }
            }
            
            // Remove controls from array.
            _rowControls.Remove(_rowControls.ElementAt(index));

            // Refresh URI list in item.
            UpdateItemUriList();

        }

        private void UpdateItemUriList()
        {
            // Check to see if we need a login object.
            if (_item.Login == null)
            {
                // Create new login object.
                _item.Login = new Login();
            }
            
            // Create new URI list.
            List<LoginURI> uris = new List<LoginURI>();
            
            // Loop through URI list.
            foreach (View[] rowCtrls in _rowControls)
            {
                // Create new login URI object.
                LoginURI loginUri = new LoginURI();
                
                // Get the txt and combo box views.
                TextView uri = (TextView) rowCtrls[0];
                ComboBox match = (ComboBox) rowCtrls[3];
                
                // Set values.
                loginUri.URI = uri.Text.ToString();
                loginUri.Match = match.SelectedItem == 0 ? null : match.SelectedItem - 1;
                
                // Add uri to list.
                uris.Add(loginUri);
            }
            
            // Update URI list.
            _item.Login.URIs = uris;
        }
        
        private void UpdateItem()
        {
            // Set item properties.
            _item.ItemName = txtItemName.Text.ToString();
            _item.IsFavorite = chkFavorite.Checked;
            _item.Login.UserName = txtUserName.Text.ToString();
            _item.Login.Password = txtPassword.Text.ToString();
            _item.Login.TOTP = txtTOTP.Text.ToString();
        }
        
        public void Show()
        {
            Application.Run(this);
        }
        
        public bool OkPressed
        {
            get { return _okPressed; }
        }

        public VaultItem Item
        {
            get { return _item; }
        }
        
        #region Event Handlers
        private void CancelButton_Clicked()
        {
            // Close dialog.
            Application.RequestStop();
        }

        private void SaveButtonClicked()
        {
            // Check to see that an item name is present (it is required).
            if (txtItemName.Text == null)
            {
                MessageBox.ErrorQuery("Action failed.", "Item name must have a value.", "Ok");
            }

            else
            {
                // Update item to current control values.
                UpdateItem();

                // Check to see which save mode we are in.
                switch (_viewState)
                {
                    case VaultItemDetailViewState.Create:
                        break;

                    case VaultItemDetailViewState.Edit:
                        break;
                }

                // Flag that the save button was pressed.
                _okPressed = true;
            }
        }

        private void CopyPasswordButtonClicked()
        {
            // Copy password to clipboard.
            Clipboard.TrySetClipboardData(_item.Login.Password);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Password copied to clipboard.", "Ok");

        }

        private void ViewPasswordButtonClicked()
        {
            // Toggle Flag.
            txtPassword.Secret = !txtPassword.Secret;
            
            // Flip button text to opposite action.
            btnViewPassword.Text = txtPassword.Secret ? "View" : "Hide";
        }

        private void CopyUserNameButtonClicked()
        {
            // Copy password to clipboard.
            Clipboard.TrySetClipboardData(_item.Login.UserName);

            // Indicate data copied.
            MessageBox.Query("Action Completed", "User name copied to clipboard.", "Ok");

        }

        private void GeneratePasswordButtonClicked()
        {
            throw new NotImplementedException();
        }
        #endregion

        private void NewUriButtonClicked()
        {
            throw new NotImplementedException();
        }
    }
}
