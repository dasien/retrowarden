
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
            
            // Set combo box default values.
            cboCardBrand.SelectedItem = _cardBrands.FindIndex(o => o.Index == _item.Card.Brand);
            cboExpMonth.SelectedItem = _expMonths.FindIndex(o => o.Index == _item.Card.ExpiryMonth);
        }
        
        private void InitializeLists()
        {
            // Load lists.
            _cardBrands = CodeListManager.GetInstance().GetList("CardBrands");
            _expMonths = CodeListManager.GetInstance().GetList("ExpiryMonths");
            
            // Set combox sources.
            cboCardBrand.SetSource(_cardBrands);
            cboExpMonth.SetSource(_expMonths);
        }

        protected override void UpdateItem()
        {
            // Check to see if the sub object is null (create mode).
            _item.Card ??= new Card();
            
            // Set values.
            _item.Card.CardholderName =  txtCardholderName.Text.ToString() ?? "";
            _item.Card.CardNumber = txtCardNumber.Text.ToString() ?? "";
            _item.Card.ExpiryYear = txtExpYear.Text.ToString() ?? "";
            _item.Card.SecureCode = txtCVV.Text.ToString() ?? "";
            _item.Card.Brand = _cardBrands.ElementAt(cboCardBrand.SelectedItem).Index;
            _item.Card.ExpiryMonth = _expMonths.ElementAt(cboExpMonth.SelectedItem).Index;
            
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
    }
}
