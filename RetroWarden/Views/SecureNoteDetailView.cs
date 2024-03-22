
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
        
        private new void SetupView()
        {
            // Base setup what kind of view state we are in.
            if (_viewState == VaultItemDetailViewState.View || _viewState == VaultItemDetailViewState.Edit)
            {
                // Load controls with current data only.
                LoadView();
            }
            
            // Create an empty view so that the base view can resize.
            View empty = new View(new Rect(1, 3, 1, 1));

            // Set to base.
            base.DetailView = empty;
            
            // Setup common view parts.
            base.SetupView();
        }
        
        #region Event Handlers

        protected override void SaveButtonClicked()
        {
            // Check to see that an item name is present (it is required).
            if (ItemName.Text == null)
            {
                MessageBox.ErrorQuery("Action failed.", "Item name must have a value.", "Ok");
            }

            else
            {
                // Update item to current control values.
                UpdateItem();

                // Indicate Save was pressed.
                base.OkPressed = true;

                // Close dialog.
                Application.RequestStop();
            }
        }
        #endregion
    }
}
