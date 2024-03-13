using Terminal.Gui;
using Retrowarden.Utils;

namespace Retrowarden.Dialogs
{
    public class WorkingDialog
    {
        // Controls.
        private readonly Dialog _dialog;
        private readonly Label _animation;
        private readonly Label _message;
        private readonly Label _progress;
        private int _animationIndex = 0;
        private object _timerToken;
        
        // Other values.
        private bool _okPressed = false;
        private readonly string[] _spinner = [
            "01100010", "01101001", "01110100", "01110111", "01100001", "01110010", "01100100", "01100101",
            "01101110"
        ];
        
        public WorkingDialog(string message)
        {
            // Create modal view.
            _dialog = new Dialog("Working...", 40, 6);

            // Create labels.
            _animation = ViewUtils.CreateControl<Label>(3,2,10,1, 0, 0, 
                false, true, "", "lblAnimation");
            
            _message = ViewUtils.CreateControl<Label>(13,2,10,1, 0, 0, 
                false, true, message, "lblMessage");

            _progress = ViewUtils.CreateControl<Label>(13, 3, 10, 1, 0, 0,
                false, true, "", "lblProgress");
            
            // Add controls to view.
            _dialog.Add(_animation, _message, _progress);
        }

        public void Show()
        {
            _timerToken = Application.MainLoop.AddTimeout (TimeSpan.FromMilliseconds(80), UpdateAnimationLabel);
            Application.Run(_dialog);
        }

        public void Hide()
        {
            Application.MainLoop.RemoveTimeout (_timerToken);
            Application.RequestStop(_dialog);
        }

        public string ProgressMessage
        {
            set
            {
                _progress.Text = value;
            }
        }
        public bool IsCurrentTop
        {
            get
            {
                return _dialog.IsCurrentTop;
            }
        }
        
        private bool UpdateAnimationLabel(MainLoop arg)
        {
            // Update text.
            _dialog.Subviews[0].Subviews[0].Text = _spinner[_animationIndex];
            _dialog.Subviews[0].Subviews[0].SetNeedsDisplay();
            
            if (_animationIndex < (_spinner.Length -1))
            {
                _animationIndex++;
            }

            else
            {
                _animationIndex = 0;
            }
            
            // Return true so the timer keeps running.
            return true;
        }
    }
}