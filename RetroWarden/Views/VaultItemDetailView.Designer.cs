using System;
using System.Net.Mime;
using System.Runtime.InteropServices.Marshalling;
using Terminal.Gui;
using Retrowarden.Utils;

namespace Retrowarden.Views
{
    public partial class VaultItemDetailView : Terminal.Gui.Dialog 
    {
        private Terminal.Gui.Label lblName;
        private Terminal.Gui.Label lblFolder;
        private Terminal.Gui.TextField txtName;
        private Terminal.Gui.ComboBox cboFolder;
        private Terminal.Gui.CheckBox chkFavorite;
        private Terminal.Gui.Label lblUserName;
        private Terminal.Gui.Label lblPassword;
        private Terminal.Gui.TextField txtUserName;
        private Terminal.Gui.Button btnCopyUserName;
        private Terminal.Gui.TextField txtPassword;
        private Terminal.Gui.Button btnViewPassword;
        private Terminal.Gui.Button btnCopyPassword;
        private Terminal.Gui.Label lblTOTPKey;
        private Terminal.Gui.TextField txtTOTPKey;
        private Terminal.Gui.Label lblURI;
        private Terminal.Gui.Label lblMatch;
        private Terminal.Gui.TextField txtURI;
        private Terminal.Gui.ComboBox cboMatch;
        private Terminal.Gui.FrameView fraNotes;
        private Terminal.Gui.TextView tvwNotes;
        private Terminal.Gui.Button btnSave;
        private Terminal.Gui.Button btnCancel;
        


        
        private void InitializeComponent() 
        {
            this.tvwNotes = ViewUtils.CreateControl<TextView>(1, 0, 71, 7, 0, 0,
                true, true, "", "tvwNotes");
            this.fraNotes = ViewUtils.CreateControl<FrameView>(5, 13, 75, 8, 0, 0,
                false, true, "", "fraNotes");
            this.cboMatch = ViewUtils.CreateControl<ComboBox>(41, 11, 20, 2, 0, 0,
                true, true, "", "cboMatch");
            this.txtURI = ViewUtils.CreateControl<TextField>(5, 11, 29, 1, 0, 0,
                true, true, "", "TxtURI");
            this.btnCancel = ViewUtils.CreateControl<Button>(45, 22, 10, 1, 0, 0,
                true, true, "Cancel", "btnCancel", TextAlignment.Centered);
            this.btnSave = ViewUtils.CreateControl<Button>(30, 22, 8, 1, 0, 0,
                true, true, "Save", "btnSave", TextAlignment.Centered);
            this.lblMatch = ViewUtils.CreateControl<Label>(41, 10, 4, 1, 0, 0,
                false, true, "Match Detection", "lblMatch");
            this.lblURI = ViewUtils.CreateControl<Label>(5, 10, 4, 1, 0, 0,
                false, true, "URI List", "lblURI");
            this.txtTOTPKey = ViewUtils.CreateControl<TextField>(5, 8, 29, 1, 0, 0,
                true, true, "", "txtTOTPKey");
            this.lblTOTPKey = ViewUtils.CreateControl<Label>(5, 7, 29, 1, 0, 0,
                false, true, "Authenticator Key (TOTP)", "lblTOTPKey");
            this.btnCopyPassword = ViewUtils.CreateControl<Button>(71, 5, 9, 1, 0, 0,
                true, true, "Copy", "btnCopyPassword", TextAlignment.Centered);
            this.btnViewPassword = ViewUtils.CreateControl<Button>(62, 5, 8, 1, 0, 0,
                true, true, "View", "btnViewPassword",TextAlignment.Centered);
            this.txtPassword = ViewUtils.CreateControl<TextField>(41,5,20,1,0,0,
                true,true,"","txtPassword");
            this.btnCopyUserName = ViewUtils.CreateControl<Button>(26,5,8,1,0,0,
                true, true,"Copy", "btnCopyUserName", TextAlignment.Centered);
            this.lblUserName = ViewUtils.CreateControl<Label>(5, 4, 4, 1, 0, 0,
                false, true, "UserName", "lblUserName");
            this.txtUserName = ViewUtils.CreateControl<TextField>(5, 5, 20, 1, 0, 0,
                true, true, null, "txtUserName");
            this.lblPassword = ViewUtils.CreateControl<Label>(41, 4, 4, 1, 0, 0,
                false, true, "Password", "lblPassword");
            this.chkFavorite = ViewUtils.CreateControl<CheckBox>(69,2,6,1,0,0,
                true, true, "Favorite", "chkFavorite");
            this.lblFolder = ViewUtils.CreateControl<Label>(41,1,5,3,0,0,
                false, true, "Folder", "lblFolder");
            this.cboFolder = ViewUtils.CreateControl<ComboBox>(41, 2, 20, 5, 0, 0,
                true, true, null, "cboFolder");
            this.lblName = ViewUtils.CreateControl<Label>(4, 1, 4, 1, 0, 0,
                false, true, "Name", "lblName");
            this.txtName = ViewUtils.CreateControl<TextField>(4,2,20,3,0,0,
                true, true, null, "txtName");

            this.Width = Dim.Percent(75f);
            this.Height = Dim.Percent(85f);
            this.X = Pos.Center();
            this.Y = Pos.Center();
            this.Visible = true;
            this.Modal = true;
            this.IsMdiContainer = false;
            this.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
            this.Border.Effect3D = true;
            this.Border.Effect3DBrush = null;
            this.Border.DrawMarginFrame = true;
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Title = "";
            
            this.Add(this.lblName);
            this.Add(this.lblFolder);
            this.Add(this.txtName);
            this.Add(this.cboFolder);
            
            this.chkFavorite.Checked = false;
            this.Add(this.chkFavorite);
            this.Add(this.lblUserName);
            this.Add(this.lblPassword);
            this.Add(this.txtUserName);
            
            btnCopyUserName.Clicked += CopyUserNameButtonClicked;
            this.Add(this.btnCopyUserName);
            
            this.txtPassword.Secret = true;
            this.Add(this.txtPassword);
            
            btnViewPassword.Clicked += ViewPasswordButtonClicked;
            this.Add(this.btnViewPassword);
            
            btnCopyPassword.Clicked += CopyPasswordButtonClicked;
            this.Add(this.btnCopyPassword);
            this.Add(this.lblTOTPKey);
            this.Add(this.txtTOTPKey);
            this.Add(this.lblURI);
            this.Add(this.lblMatch);
            this.Add(this.txtURI);
            this.Add(this.cboMatch);
            
            this.fraNotes.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
            this.fraNotes.Border.Effect3D = false;
            this.fraNotes.Border.Effect3DBrush = null;
            this.fraNotes.Border.DrawMarginFrame = true;
            this.fraNotes.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.fraNotes.Title = "Notes";
            this.Add(this.fraNotes);
            
            this.tvwNotes.AllowsTab = true;
            this.tvwNotes.AllowsReturn = true;
            this.tvwNotes.WordWrap = true;
            this.fraNotes.Add(this.tvwNotes);
            
            this.btnSave.Clicked += SaveButtonClicked;
            this.Add(this.btnSave);
            
            btnCancel.Clicked += CancelButton_Clicked;
            this.Add(this.btnCancel);
        }
    }
}
