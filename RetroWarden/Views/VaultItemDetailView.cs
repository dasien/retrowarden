using Terminal.Gui;
using Retrowarden.Models;

namespace Retrowarden.Views 
{
    public partial class VaultItemDetailView
    {
        private readonly VaultItem _item;
        private readonly List<VaultFolder> _folders;
        private readonly VaultItemDetailViewState _viewState;
        private bool _okPressed;
        public VaultItemDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
        {
            // Set private variables.
            _item = item;
            _viewState = state;
            _folders = folders;
            _okPressed = false;
            
            InitializeComponent();
            
            // Update controls based on view state.
            SetupView();
        }
        
        private void SetupView()
        { 
            // Create combobox data source.
            List<string> folderNames = new List<string>();
            
            // Load combobox.
            for (int cnt = 0; cnt < _folders.Count; cnt++)
            {
                // Move to a simple collection the combo box can understand.
                folderNames.Add(_folders[cnt].Name);
                
                // Check to see if the folder id matches.
                if (_item.FolderId == _folders[cnt].Id)
                {
                    // Set the selected item to this folder.
                    cboFolder.SelectedItem = cnt;
                }
            }
            
            cboFolder.SetSource(folderNames);
            
            // Based on state, determine if we need to load controls with data.
            if (_viewState != VaultItemDetailViewState.Create)
            {
                // Set current item values to controls.
                txtName.Text = _item.ItemName;
                txtUserName.Text = _item.Login.UserName;
                txtPassword.Text = _item.Login.Password;
                tvwNotes.Text = _item.Notes == null ? "" : _item.Notes;
                
                // Check to make sure there is a URL.
                if (_item.Login.URLs != null)
                {
                    txtURI.Text = _item.Login.URLs[1].URI;
                }
            }
            
            // If we are in view only mode, need to disable input controls.
            if (_viewState == VaultItemDetailViewState.View)
            {
                txtName.Enabled = false;
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                txtURI.Enabled = false;
                tvwNotes.Enabled = false;
                cboFolder.Enabled = false;
                cboMatch.Enabled = false;
                chkFavorite.Enabled = false;
                btnSave.Enabled = false;
            }
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
        
        private void CancelButton_Clicked()
        {
            // Close dialog.
            Application.RequestStop();
        }

        private void SaveButtonClicked()
        {
            // Check to see if anything actually changed.
            _item.IsDirty =
                (txtName.Text != _item.ItemName ||
                 txtUserName.Text != _item.Login.UserName ||
                 txtPassword.Text != _item.Login.Password);
            
            // Set values from controls to object.
            if (_item.IsDirty)
            {
                // Check to see which save mode we are in.
                switch (_viewState)
                {
                    case VaultItemDetailViewState.Create:
                        break;
                    
                    case VaultItemDetailViewState.Edit:
                        break;
                }
            }
            
            // Flag that the save button was pressed.
            _okPressed = true;
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
    }
}
