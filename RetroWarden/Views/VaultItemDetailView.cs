using System.Collections;
using Terminal.Gui;
using Retrowarden.Models;

namespace Retrowarden.Views 
{
    public partial class VaultItemDetailView
    {
        private VaultItem _item;
        private List<VaultFolder> _folders;
        private VaultItemDetailViewState _viewState;
        
        public VaultItemDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
        {
            // Set private variables.
            _item = item;
            _viewState = state;
            _folders = folders;
            
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
                
                // Check to make sure there is a URL.
                if (_item.Login.URLs != null)
                {
                    txtURI.Text = _item.Login.URLs[1].URI;
                }
                
                tvwNotes.Text = _item.Notes == null ? "" : _item.Notes;
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
            }
        }
        
        public void Show()
        {
            Application.Run(this);
        }
    }
}
