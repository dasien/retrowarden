using Terminal.Gui;
using Retrowarden.Utils;

namespace Retrowarden.Dialogs
{
    public class ConnectDialog
    {
        // Controls.
        private Dialog _dialog;
        private TextField _userText;
        private TextField _passwordText;

        // Other values.
        private String _userId = "";
        private String _password = "";
        private bool _okPressed = false;

        public ConnectDialog()
        {
            // Create Ok button.
            Button okButton = new Button(8, 6, "_Connect");
            okButton.Clicked += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button(24, 6, "Cance_l");
            cancelButton.Clicked += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog("Connect to Vault", 40, 10, okButton, cancelButton);

            // Create labels.
            Label serverLabel = ViewUtils.CreateControl<Label>(3,2,10,1, 0, 0, 
                false, true, "*User Id:", null);
            
            Label handleLabel = ViewUtils.CreateControl<Label>(3,4,10,1, 0, 0, 
                false, true, "*Password:", null);

            // Create text inputs.
            _userText = ViewUtils.CreateControl<TextField>(15, 2, 20, 0, 0, 0, 
                true, true, null, null);
            _passwordText = ViewUtils.CreateControl<TextField>(15, 4, 20, 0, 0, 0, 
                true, true, null, null);
            _passwordText.Secret = true;
            
            // Add controls to view.
            _dialog.Add(serverLabel, handleLabel, _userText, _passwordText);

            // Set default control.
            _userText.SetFocus();
        }

        public void Show()
        {
            Application.Run(_dialog);
        }

        private void OkButton_Clicked()
        {
            // Check to see if required values are present.
            if (_userText.Text.TrimSpace().Length == 0 || _passwordText.Text.TrimSpace().Length == 0)
            {
                MessageBox.ErrorQuery("Values Missing", "Both Server Address and User Handle required.", "Ok");
            }

            else
            {
                // Set flag for ok button and values.
                _okPressed = true;
                _userId = _userText.Text.ToString();
                _password = _passwordText.Text.ToString();

                // Close dialog.
                Application.RequestStop(_dialog);
            }
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

        public String UserId
        {
            get { return _userId; }
        }

        public String Password
        {
            get { return _password; }
        }
    }
}