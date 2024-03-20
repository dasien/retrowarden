
using Terminal.Gui;
using Retrowarden.Models;
using Retrowarden.Utils;

namespace Retrowarden.Views 
{
    public partial class SecureNoteDetailView : ItemDetailView
    {
        public SecureNoteDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
            : base (item, folders, state)
        {
            InitializeComponent();
            
            // Update controls based on view state.
            SetupView();
        }
        
        private void SetupView()
        {
            // Base setup what kind of view state we are in.
            if (_viewState == VaultItemDetailViewState.View || _viewState == VaultItemDetailViewState.Edit)
            {
                // Load controls with current data only.
                LoadView();
            }
            
            // Set our main view to the view area of the parent view.
            //base.DetailView = vwCard;

            // Setup common view parts.
            base.SetupView();
        }
        
        #region Event Handlers
        protected override void SaveButtonClicked()
        {
            // Perform validations on item data.

            // Update item to current control values.
            base.UpdateItem();

            // Check to see which save mode we are in.
            switch (_viewState)
            {
                case VaultItemDetailViewState.Create:
                    break;

                case VaultItemDetailViewState.Edit:
                    break;
            }

            // Flag that the save button was pressed.
            OkPressed = true;
        }
        #endregion
    }
}
