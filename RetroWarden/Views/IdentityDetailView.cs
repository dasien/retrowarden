using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using Terminal.Gui;
using Retrowarden.Models;
using Retrowarden.Utils;

namespace Retrowarden.Views 
{
    public partial class IdentityDetailView : ItemDetailView
    {
        private List<CodeListItem> _titles;
        public IdentityDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
            : base (item, folders, state)
        {
            // Update controls based on view state.
            SetupView();
        }
        
        private new void SetupView()
        {
            InitializeComponent();
            
            // Initialize any list controls.
            InitializeLists();
            
            // Base setup what kind of view state we are in.
            if (_viewState == VaultItemDetailViewState.View || _viewState == VaultItemDetailViewState.Edit)
            {
                // Load controls with current data only.
                LoadView();
                
                if (_viewState == VaultItemDetailViewState.View)
                {
                    // Disable control state.
                    DisableView();
                }
            }
            
            // Set our main view to the view area of the parent view.
            base.DetailView = vwIdentity;

            // Setup common view parts.
            base.SetupView();
        }
        
        private new void LoadView()
        {
            // Set current item values to controls.
            txtFirstName.Text = _item.Identity.FirstName ?? "";
            txtMiddleName.Text = _item.Identity.MiddleName ?? "";
            txtLastName.Text = _item.Identity.LastName ?? "";
            txtUserName.Text = _item.Identity.UserName ?? "";
            txtCompany.Text = _item.Identity.Company ?? "";
            txtSSN.Text = _item.Identity.Ssn ?? "";
            txtPassportNumber.Text = _item.Identity.PassportNumber ?? "";
            txtLicenseNumber.Text = _item.Identity.LicenseNumber ?? "";
            txtEmailAddress.Text = _item.Identity.Email ?? "";
            txtPhoneNumber.Text = _item.Identity.Phone ?? "";
            txtAddress1.Text = _item.Identity.Address1 ?? "";
            txtAddress2.Text = _item.Identity.Address2 ?? "";
            txtAddress3.Text = _item.Identity.Address3 ?? "";
            txtCity.Text = _item.Identity.City ?? "";
            txtState.Text = _item.Identity.State ?? "";
            txtZipCode.Text = _item.Identity.PostalCode ?? "";
            txtCountry.Text = _item.Identity.Country ?? "";
            
            // Load titles combo box.
            cboTitle.SetSource(_titles);
            
            // Set combo box default values.
            cboTitle.SelectedItem = _titles.FindIndex(o => o.Index == _item.Identity.Title);

            

        }

        private new void DisableView()
        {
            //btnSave.Enabled = false;
            
            // Loop through the list of URIs and disable the delete buttons.
            
        }

        private void InitializeLists()
        {
            // Create new list.
            _titles = new List<CodeListItem>
            {
                new CodeListItem("Mr", "Mr"),
                new CodeListItem("Mrs", "Mrs"),
                new CodeListItem("Ms", "Ms"),
                new CodeListItem("Mx", "Mx"),
                new CodeListItem("Dr", "Dr")
            };
        }

        protected override void UpdateItem()
        {
            // Call base method.
            base.UpdateItem();
            
            // Save any additional properties.
        }
        
        #region Event Handlers
        private void SaveButtonClicked()
        {
            // Perform validations on item data.

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
            OkPressed = true;
        }
        #endregion
    }
}
