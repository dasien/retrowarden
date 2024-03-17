using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using Terminal.Gui;
using Retrowarden.Models;
using Retrowarden.Utils;

namespace Retrowarden.Views 
{
    public partial class CardDetailView : ItemDetailView
    {
        private List<CodeListItem> _cardBrands;
        private List<CodeListItem> _expMonths;
        
        public CardDetailView(VaultItem item, List<VaultFolder> folders, VaultItemDetailViewState state) 
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
            base.DetailView = vwCard;

            // Setup common view parts.
            base.SetupView();
        }

        private new void LoadView()
        {
            // Set values.
            txtCardholderName.Text = _item.Card.CardholderName ?? "";
            txtCardNumber.Text = _item.Card.CardNumber ?? "";
            txtExpYear.Text = _item.Card.ExpiryYear ?? "";
            txtCVV.Text = _item.Card.SecureCode ?? "";
            
            // Set combox sources.
            cboCardBrand.SetSource(_cardBrands);
            cboExpMonth.SetSource(_expMonths);
            
            // Set combo box default values.
            cboCardBrand.SelectedItem = _cardBrands.FindIndex(o => o.Index == _item.Card.Brand);
            cboExpMonth.SelectedItem = _expMonths.FindIndex(o => o.Index == _item.Card.ExpiryMonth);
        }

        private new void DisableView()
        {
            //btnSave.Enabled = false;
            
            // Loop through the list of URIs and disable the delete buttons.
        }
        
        private void InitializeLists()
        {
            // Create list of card brands.
            _cardBrands = new List<CodeListItem>
            {
                new CodeListItem("Visa", "Visa"),
                new CodeListItem("Mastercard", "Mastercard"),
                new CodeListItem("American Express", "American Express"),
                new CodeListItem("Discover", "Discover"),
                new CodeListItem("Diners Club", "Diners Club"),
                new CodeListItem("JCB", "JCB"),
                new CodeListItem("Maestro", "Maestro"),
                new CodeListItem("UnionPay", "UnionPay"),
                new CodeListItem("RuPay", "RuPay"),
                new CodeListItem("Other", "Other"),
            };
            
            _expMonths = new List<CodeListItem>
            {
                new CodeListItem("1", "1 - January"),
                new CodeListItem("2", "2 - February"),
                new CodeListItem("3", "3 - March"),
                new CodeListItem("4", "4 - April"),
                new CodeListItem("5", "5 - May"),
                new CodeListItem("6", "6 - June"),
                new CodeListItem("7", "7 - July"),
                new CodeListItem("8", "8 - August"),
                new CodeListItem("9", "9 - September"),
                new CodeListItem("10", "10 - October"),
                new CodeListItem("11", "11 - November"),
                new CodeListItem("12", "12 - December")
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

        private void ShowCardButtonClicked()
        {
            // Toggle Flag.
            txtCardNumber.Secret = !txtCardNumber.Secret;
            
            // Flip button text to opposite action.
            btnShowCardNumber.Text = txtCardNumber.Secret ? "View" : "Hide";
        }

        private void CopyCardButtonClicked()
        {
            // Copy username to clipboard.
            Clipboard.TrySetClipboardData(txtCardNumber.Text.ToString());

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Card number copied to clipboard.", "Ok");
        }

        private void ShowCVVButtonClicked()
        {
            // Toggle Flag.
            txtCVV.Secret = !txtCVV.Secret;
            
            // Flip button text to opposite action.
            btnShowCVV.Text = txtCVV.Secret ? "View" : "Hide";
        }

        private void CopyCVVButtonClicked()
        {
            // Copy username to clipboard.
            Clipboard.TrySetClipboardData(txtCVV.Text.ToString());

            // Indicate data copied.
            MessageBox.Query("Action Completed", "Card CVV copied to clipboard.", "Ok");
        }

        private void HandleControlEnter(FocusEventArgs obj)
        {
            // Get the currently focused view.
            View view = FindFocusedControl(base.Focused);
            
            // Check to make sure it isn't null.
            if (view != null)
            {
                // Check to see if 
                if (view.GetType() == typeof(TextField))
                {
                    // Downcast to textfield and select all text.
                    TextField focus = (TextField)view;
                    focus.SelectAll();
                }
            }
        }
    }
}
