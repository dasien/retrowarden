
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
            
            // Set combo box default values.
            cboTitle.SelectedItem = _titles.FindIndex(o => o.Index == _item.Identity.Title);
        }
        
        private void InitializeLists()
        {
            // Load list.
            _titles = CodeListManager.GetInstance().GetList("Titles");
            
            // Load titles combo box.
            cboTitle.SetSource(_titles);
        }

        protected override void UpdateItem()
        {
            // Check to see if the sub object is null (create mode).
            _item.Identity ??= new Identity();

            // Set values.
            _item.Identity.FirstName =  txtFirstName.Text.ToString() ?? "";
            _item.Identity.MiddleName = txtMiddleName.Text.ToString() ?? "";
            _item.Identity.LastName = txtLastName.Text.ToString() ?? "";
            _item.Identity.UserName = txtUserName.Text.ToString() ?? "";
            _item.Identity.Company = txtCompany.Text.ToString() ?? "";
            _item.Identity.Ssn = txtSSN.Text.ToString() ?? "";
            _item.Identity.PassportNumber = txtPassportNumber.Text.ToString() ?? "";
            _item.Identity.LicenseNumber = txtLicenseNumber.Text.ToString() ?? "";
            _item.Identity.Email = txtEmailAddress.Text.ToString() ?? "";
            _item.Identity.Phone = txtPhoneNumber.Text.ToString() ?? "";
            _item.Identity.Address1 = txtAddress1.Text.ToString() ?? "";
            _item.Identity.Address2 = txtAddress2.Text.ToString() ?? "";
            _item.Identity.Address3 = txtAddress3.Text.ToString() ?? "";
            _item.Identity.City = txtCity.Text.ToString() ?? "";
            _item.Identity.State = txtState.Text.ToString() ?? "";
            _item.Identity.PostalCode = txtZipCode.Text.ToString() ?? "";
            _item.Identity.Country = txtCountry.Text.ToString() ?? "";
            _item.Identity.Title = _titles.ElementAt(cboTitle.SelectedItem).Index;
            
            // Call base method.
            base.UpdateItem();
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
