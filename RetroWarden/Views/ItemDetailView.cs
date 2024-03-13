using System.Diagnostics;
using Terminal.Gui;
using Retrowarden.Models;
using Retrowarden.Utils;

namespace Retrowarden.Views 
{
    public abstract partial class ItemDetailView
    {
        private View _subView;
        protected VaultItem _item;
        private readonly List<VaultFolder> _folders;
        private List<CodeListItem> _folderNames;
        protected readonly VaultItemDetailViewState _viewState;
        private bool _okPressed;

        protected ItemDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
        {
            // Set private variables.
            _item = item == null ? new VaultItem() : item;
            _viewState = state;
            _folders = folders;
            _okPressed = false;
            _folderNames = new List<CodeListItem>();
            
            InitializeComponent();
        }
        
        protected void SetupView()
        {
            // Add subview to view.
            scrMain.Add(_subView);
            
            // Update location of 'footer' controls to below the particular detail view.
            RelocateFooterControls();
            
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

        protected void LoadView()
        {
            // Set current item values to controls.
            txtItemName.Text = _item.ItemName;
            chkFavorite.Checked = _item.IsFavorite;
            tvwNotes.Text = _item.Notes == null ? "" : _item.Notes;
            
            // Set folder comboxbox source.
            cboFolder.SetSource(_folderNames);
            
            // Set the folder to the current folder (or "No Folder" as a null default.
            cboFolder.SelectedItem = _folderNames.FindIndex(o => o.Index == _item.FolderId);
        }

        protected void DisableView()
        {
            //btnSave.Enabled = false;
        }
        
        private void InitializeLists()
        {
            // Load combobox.
            for (int cnt = 0; cnt < _folders.Count; cnt++)
            {
                // Move to a simple collection the combo box can understand.
                _folderNames.Add(new CodeListItem(_folders[cnt].Id, _folders[cnt].Name));
            }
        }

        private void RelocateFooterControls()
        {
            // Update Y of the notes frame to detail view bottom + 2
            fraNotes.Y = Pos.Bottom(_subView) + 2;
            
            // Update the save and cancel button locations relative to the new notes frame.
            btnSave.Y = Pos.Bottom(fraNotes) + 2;
            btnCancel.Y = Pos.Bottom(fraNotes) + 2;
            scrMain.SetNeedsDisplay();
        }
        
        protected virtual void UpdateItem()
        {
            // Set item properties.
            _item.ItemName = txtItemName.Text.ToString();
            _item.IsFavorite = chkFavorite.Checked;
            _item.Notes = tvwNotes.Text.ToString();
        }
        
        public void Show()
        {
            Application.Run(this);
        }
        
        public bool OkPressed
        {
            get { return _okPressed; }
            set { _okPressed = value; }
        }

        public VaultItem Item
        {
            get { return _item; }
        }

        protected TextField ItemName
        {
            get { return this.txtItemName; }
        }

        protected View DetailView
        {
            get 
            {
                return _subView;
            }

            set
            {
                _subView = value;
            }
        }
        
        #region Event Handlers
        protected virtual void SaveButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void CancelButtonClicked()
        {
            // Close dialog.
            Application.RequestStop();
        }
        #endregion
    }
}
