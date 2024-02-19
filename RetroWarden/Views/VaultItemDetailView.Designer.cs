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
            this.btnCancel = new Terminal.Gui.Button();
            this.btnSave = new Terminal.Gui.Button();
            this.tvwNotes = new Terminal.Gui.TextView();
            this.fraNotes = new Terminal.Gui.FrameView();
            this.cboMatch = new Terminal.Gui.ComboBox();
            this.txtURI = new Terminal.Gui.TextField();
            this.lblMatch = new Terminal.Gui.Label();
            this.lblURI = new Terminal.Gui.Label();
            this.txtTOTPKey = new Terminal.Gui.TextField();
            this.lblTOTPKey = new Terminal.Gui.Label();
            this.btnCopyPassword = new Terminal.Gui.Button();
            this.btnViewPassword = new Terminal.Gui.Button();
            this.txtPassword = new Terminal.Gui.TextField();
            this.btnCopyUserName = new Terminal.Gui.Button();
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
            this.Title = "Create New Item";
            
            this.Add(this.lblName);
            this.Add(this.lblFolder);
            this.Add(this.txtName);
            this.Add(this.cboFolder);
            
            this.chkFavorite.Checked = false;
            this.Add(this.chkFavorite);
            
            this.Add(this.lblUserName);
            this.Add(this.lblPassword);
            
            this.Add(this.txtUserName);
            this.btnCopyUserName.Width = 8;
            this.btnCopyUserName.Height = 1;
            this.btnCopyUserName.X = 26;
            this.btnCopyUserName.Y = 5;
            this.btnCopyUserName.Visible = true;
            this.btnCopyUserName.Data = "btnCopyUserName";
            this.btnCopyUserName.Text = "Copy";
            this.btnCopyUserName.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnCopyUserName.IsDefault = false;
            this.Add(this.btnCopyUserName);
            this.txtPassword.Width = 20;
            this.txtPassword.Height = 1;
            this.txtPassword.X = 41;
            this.txtPassword.Y = 5;
            this.txtPassword.Visible = true;
            this.txtPassword.Secret = true;
            this.txtPassword.Data = "txtPassword";
            this.txtPassword.Text = "";
            this.txtPassword.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Add(this.txtPassword);
            this.btnViewPassword.Width = 8;
            this.btnViewPassword.Height = 1;
            this.btnViewPassword.X = 62;
            this.btnViewPassword.Y = 5;
            this.btnViewPassword.Visible = true;
            this.btnViewPassword.Data = "btnViewPassword";
            this.btnViewPassword.Text = "View";
            this.btnViewPassword.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnViewPassword.IsDefault = false;
            this.Add(this.btnViewPassword);
            this.btnCopyPassword.Width = 9;
            this.btnCopyPassword.Height = 1;
            this.btnCopyPassword.X = 71;
            this.btnCopyPassword.Y = 5;
            this.btnCopyPassword.Visible = true;
            this.btnCopyPassword.Data = "btnCopyPassword";
            this.btnCopyPassword.Text = "Co_py";
            this.btnCopyPassword.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnCopyPassword.IsDefault = false;
            this.Add(this.btnCopyPassword);
            this.lblTOTPKey.Width = 29;
            this.lblTOTPKey.Height = 1;
            this.lblTOTPKey.X = 5;
            this.lblTOTPKey.Y = 7;
            this.lblTOTPKey.Visible = true;
            this.lblTOTPKey.Data = "lblTOTPKey";
            this.lblTOTPKey.Text = "Authenticator Key (TOTP)";
            this.lblTOTPKey.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Add(this.lblTOTPKey);
            this.txtTOTPKey.Width = 29;
            this.txtTOTPKey.Height = 1;
            this.txtTOTPKey.X = 5;
            this.txtTOTPKey.Y = 8;
            this.txtTOTPKey.Visible = true;
            this.txtTOTPKey.Secret = false;
            this.txtTOTPKey.Data = "txtTOTPKey";
            this.txtTOTPKey.Text = "";
            this.txtTOTPKey.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Add(this.txtTOTPKey);
            this.lblURI.Width = 4;
            this.lblURI.Height = 1;
            this.lblURI.X = 5;
            this.lblURI.Y = 10;
            this.lblURI.Visible = true;
            this.lblURI.Data = "lblURI";
            this.lblURI.Text = "URI";
            this.lblURI.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Add(this.lblURI);
            this.lblMatch.Width = 4;
            this.lblMatch.Height = 1;
            this.lblMatch.X = 41;
            this.lblMatch.Y = 10;
            this.lblMatch.Visible = true;
            this.lblMatch.Data = "lblMatch";
            this.lblMatch.Text = "Match Detection";
            this.lblMatch.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Add(this.lblMatch);
            this.txtURI.Width = 29;
            this.txtURI.Height = 1;
            this.txtURI.X = 5;
            this.txtURI.Y = 11;
            this.txtURI.Visible = true;
            this.txtURI.Secret = false;
            this.txtURI.Data = "txtURI";
            this.txtURI.Text = "";
            this.txtURI.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Add(this.txtURI);
            this.cboMatch.Width = 20;
            this.cboMatch.Height = 2;
            this.cboMatch.X = 41;
            this.cboMatch.Y = 11;
            this.cboMatch.Visible = true;
            this.cboMatch.Data = "cboMatch";
            this.cboMatch.Text = "";
            this.cboMatch.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Add(this.cboMatch);
            this.fraNotes.Width = 75;
            this.fraNotes.Height = 8;
            this.fraNotes.X = 5;
            this.fraNotes.Y = 13;
            this.fraNotes.Visible = true;
            this.fraNotes.Data = "fraNotes";
            this.fraNotes.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
            this.fraNotes.Border.Effect3D = false;
            this.fraNotes.Border.Effect3DBrush = null;
            this.fraNotes.Border.DrawMarginFrame = true;
            this.fraNotes.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.fraNotes.Title = "Notes";
            this.Add(this.fraNotes);
            this.tvwNotes.Width = 71;
            this.tvwNotes.Height = 7;
            this.tvwNotes.X = 1;
            this.tvwNotes.Y = 0;
            this.tvwNotes.Visible = true;
            this.tvwNotes.AllowsTab = true;
            this.tvwNotes.AllowsReturn = true;
            this.tvwNotes.WordWrap = false;
            this.tvwNotes.Data = "tvwNotes";
            this.tvwNotes.Text = "\t\t\t\t\t";
            this.tvwNotes.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.fraNotes.Add(this.tvwNotes);
            this.btnSave.Width = 8;
            this.btnSave.Height = 1;
            this.btnSave.X = 30;
            this.btnSave.Y = 22;
            this.btnSave.Visible = true;
            this.btnSave.Data = "btnSave";
            this.btnSave.Text = "Save";
            this.btnSave.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnSave.IsDefault = false;
            this.Add(this.btnSave);
            this.btnCancel.Width = 10;
            this.btnCancel.Height = 1;
            this.btnCancel.X = 45;
            this.btnCancel.Y = 22;
            this.btnCancel.Visible = true;
            this.btnCancel.Data = "btnCancel";
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnCancel.IsDefault = false;
            this.Add(this.btnCancel);
        }
    }
}
