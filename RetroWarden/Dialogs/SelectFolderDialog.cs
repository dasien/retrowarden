using Retrowarden.Models;
using Terminal.Gui;
using Retrowarden.Utils;

namespace Retrowarden.Dialogs
{
    public class SelectFolderDialog
    {
        // Controls.
        private readonly Dialog _dialog;
        private readonly ComboBox _cboFolder;
        private int _folderIndex;
        
        // Other values.
        private bool _okPressed = false;

        public SelectFolderDialog(List<VaultFolder> folders)
        {
            // Create Ok button.
            Button okButton = new Button(8, 6, "Ok");
            okButton.Clicked += OkButton_Clicked;

            // Create Cancel button.
            Button cancelButton = new Button(24, 6, "Cancel");
            cancelButton.Clicked += CancelButton_Clicked;

            // Create modal view.
            _dialog = new Dialog("Select Folder", 40, 10, okButton, cancelButton);

            // Create label.
            Label folderLabel = ViewUtils.CreateControl<Label>(3,2,10,1, 0, 0, 
                false, true, "Folder:", null);

            // Create folder dropdown.
            _cboFolder = ViewUtils.CreateControl<ComboBox>(15, 2, 20, 0, 0, 0, 
                true, true, null, null);
            
            // Create event handler for selecting a folder from the list.
            _cboFolder.SelectedItemChanged += HandleFolderSelect;
            
            // Set source for combobox.
            _cboFolder.SetSource(folders);
            
            // Add controls to view.
            _dialog.Add(folderLabel, _cboFolder);

            // Set default control.
            _cboFolder.SetFocus();
        }

        private void HandleFolderSelect(ListViewItemEventArgs obj)
        {
            _folderIndex = obj.Item;
        }

        public bool OkPressed
        {
            get { return _okPressed; }
        }

        public void Show()
        {
            Application.Run(_dialog);
        }

        private void OkButton_Clicked()
        {
            // Check to see if required values are present.
            if (_cboFolder.SelectedItem == -1)
            {
                MessageBox.ErrorQuery("Values Missing", "Please select a folder.", "Ok");
            }

            else
            {
                // Set flag for ok button and values.
                _okPressed = true;

                // Close dialog.
                Application.RequestStop(_dialog);
            }
        }

        private void CancelButton_Clicked()
        {
            // Close dialog.
            Application.RequestStop(_dialog);
        }

        public int SelectedFolderIndex
        {
            get { return _folderIndex; }
        }
    }
}