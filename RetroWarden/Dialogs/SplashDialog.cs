using Terminal.Gui;
using Retrowarden.Utils;

namespace Retrowarden.Dialogs
{
    public class SplashDialog
    {
        // Controls.
        private readonly Dialog _dialog;
        private readonly Label _message;
        
        public SplashDialog(string message)
        {
            // Create modal view.
            _dialog = new Dialog("", 85, 12);
            
            // Create labels.
            _message = ViewUtils.CreateControl<Label>(0,0,85,9, 0, 0, 
                false, true, message, "lblMessage");
            
            // Add controls to view.
            _dialog.Add(_message);
        }

        public void Show()
        {
            Application.MainLoop.AddTimeout (TimeSpan.FromMilliseconds(3000), Hide);
            Application.Run(_dialog);
        }

        private bool Hide(MainLoop arg)
        {
            Application.RequestStop(_dialog);
            return false;
        }
    }
}