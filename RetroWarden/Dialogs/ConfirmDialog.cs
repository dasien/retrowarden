
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    public class ConfirmDialog
    {
         private Dialog _dialog;

        // Other values.
        private bool _okPressed = false;

        public ConfirmDialog(string title, int height, int width, string okButtonText, string cancelButtonText)
        {
            // Create Ok button.
            Button okButton = new Button(8, 2, okButtonText);
            okButton.Clicked += OkButton_Clicked;
            
            // Create Cancel button.
            Button cancelButton = new Button(20, 2, cancelButtonText);
            cancelButton.Clicked += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog(title, width, height, okButton, cancelButton);
        }

        public void Show()
        {
            Application.Run(_dialog);
        }

        private void OkButton_Clicked()
        {
            // Set flag for ok button and values.
            _okPressed = true;

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
    }    
}
