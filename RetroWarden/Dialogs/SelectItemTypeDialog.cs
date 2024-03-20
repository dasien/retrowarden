using NStack;
using Terminal.Gui;

namespace Retrowarden.Dialogs
{

    public class SelectItemTypeDialog
    {
        private readonly Dialog _dialog;
        private readonly RadioGroup _rdoItemType;
        private bool _okPressed = false;
        private int _itemType = 0;
        
        public SelectItemTypeDialog()
        {
            // Create Ok button.
            Button okButton = new Button(5, 6, "Ok");
            okButton.Clicked += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button(14, 6, "Cancel");
            cancelButton.Clicked += CancelButton_Clicked;

            ustring[] types = new ustring[] { "Login", "Secure Note", "Card", "Identity" };
        
            // Create radio group.
            _rdoItemType = new RadioGroup(8,1, types);
                
            // Create modal view.
            _dialog = new Dialog("Select Item Type", 30, 9, okButton, cancelButton);
            _dialog.Add(_rdoItemType);
        }
        
        public void Show()
        {
            Application.Run(_dialog);
        }

        private void OkButton_Clicked()
        {
            // Set flag for ok button and values.
            _okPressed = true;
            _itemType = _rdoItemType.SelectedItem;
            // Close dialog.
            Application.RequestStop(_dialog);
        }

        private void CancelButton_Clicked()
        {
            // Close dialog.
            Application.RequestStop(_dialog);
        }

        public bool OkPressed
        {
            get { return _okPressed; }
        }

        public int ItemType
        {
            get { return _itemType; }
        }
    }
}