using System;
using Terminal.Gui;

namespace Retrowarden.Views 
{
    public partial class VaultItemDetailView : Terminal.Gui.Dialog 
    {
        private Terminal.Gui.ScrollView scrMain;
        private Terminal.Gui.Label lblItemName;
        private Terminal.Gui.Label lblFolder;
        private Terminal.Gui.TextField txtItemName;
        private Terminal.Gui.ComboBox cboFolder;
        private Terminal.Gui.CheckBox chkFavorite;
        private Terminal.Gui.Label lblUserName;
        private Terminal.Gui.Label lblPassword;
        private Terminal.Gui.TextField txtUserName;
        private Terminal.Gui.Button btnCopyUserName;
        private Terminal.Gui.TextField txtPassword;
        private Terminal.Gui.Button btnViewPassword;
        private Terminal.Gui.Button btnCopyPassword;
        private Terminal.Gui.Button btnGeneratePassword;
        private Terminal.Gui.Label lblTOTP;
        private Terminal.Gui.TextField txtTOTP;
        private Terminal.Gui.FrameView fraURIList;
        private Terminal.Gui.ScrollView scrURIList;
        private Terminal.Gui.Button btnNewURI;
        private Terminal.Gui.FrameView fraNotes;
        private Terminal.Gui.TextView tvwNotes;
        private Terminal.Gui.StatusBar stbDetail;
        private Terminal.Gui.StatusItem f1EditMe;
        
        private void InitializeComponent() 
        {
            this.stbDetail = new Terminal.Gui.StatusBar();
            this.tvwNotes = new Terminal.Gui.TextView();
            this.fraNotes = new Terminal.Gui.FrameView();
            this.btnNewURI = new Terminal.Gui.Button();
            this.scrURIList = new Terminal.Gui.ScrollView();
            this.fraURIList = new Terminal.Gui.FrameView();
            this.txtTOTP = new Terminal.Gui.TextField();
            this.lblTOTP = new Terminal.Gui.Label();
            this.btnGeneratePassword = new Terminal.Gui.Button();
            this.btnCopyPassword = new Terminal.Gui.Button();
            this.btnViewPassword = new Terminal.Gui.Button();
            this.txtPassword = new Terminal.Gui.TextField();
            this.btnCopyUserName = new Terminal.Gui.Button();
            this.txtUserName = new Terminal.Gui.TextField();
            this.lblPassword = new Terminal.Gui.Label();
            this.lblUserName = new Terminal.Gui.Label();
            this.chkFavorite = new Terminal.Gui.CheckBox();
            this.cboFolder = new Terminal.Gui.ComboBox();
            this.txtItemName = new Terminal.Gui.TextField();
            this.lblFolder = new Terminal.Gui.Label();
            this.lblItemName = new Terminal.Gui.Label();
            this.scrMain = new Terminal.Gui.ScrollView();
            
            // Dialog details.
            this.Width = Dim.Percent(85f);
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
            
            // Main scrollview details.
            this.scrMain.Width = 100;
            this.scrMain.Height = 95;
            this.scrMain.X = 0;
            this.scrMain.Y = 1;
            this.scrMain.Visible = true;
            this.scrMain.ContentSize = new Size(100,108);
            this.scrMain.Data = "scrMain";
            this.scrMain.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Add(this.scrMain);
            
            this.lblItemName.Width = 4;
            this.lblItemName.Height = 1;
            this.lblItemName.X = 1;
            this.lblItemName.Y = 1;
            this.lblItemName.Visible = true;
            this.lblItemName.Data = "lblItemName";
            this.lblItemName.Text = "Name";
            this.lblItemName.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.lblItemName);
            
            this.lblFolder.Width = 4;
            this.lblFolder.Height = 1;
            this.lblFolder.X = 40;
            this.lblFolder.Y = 1;
            this.lblFolder.Visible = true;
            this.lblFolder.Data = "lblFolder";
            this.lblFolder.Text = "Folder";
            this.lblFolder.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.lblFolder);
            
            this.txtItemName.Width = 30;
            this.txtItemName.Height = 1;
            this.txtItemName.X = 1;
            this.txtItemName.Y = 2;
            this.txtItemName.Visible = true;
            this.txtItemName.Secret = false;
            this.txtItemName.Data = "txtItemName";
            this.txtItemName.Text = "";
            this.txtItemName.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.txtItemName);
            
            this.cboFolder.Width = 30;
            this.cboFolder.Height = 2;
            this.cboFolder.X = 40;
            this.cboFolder.Y = 2;
            this.cboFolder.Visible = true;
            this.cboFolder.Data = "cboFolder";
            this.cboFolder.Text = "";
            this.cboFolder.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.cboFolder);
            
            this.chkFavorite.Width = 10;
            this.chkFavorite.Height = 1;
            this.chkFavorite.X = 77;
            this.chkFavorite.Y = 2;
            this.chkFavorite.Visible = true;
            this.chkFavorite.Data = "chkFavorite";
            this.chkFavorite.Text = "Favorite";
            this.chkFavorite.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.chkFavorite.Checked = false;
            this.scrMain.Add(this.chkFavorite);
            
            this.lblUserName.Width = 4;
            this.lblUserName.Height = 1;
            this.lblUserName.X = 1;
            this.lblUserName.Y = 4;
            this.lblUserName.Visible = true;
            this.lblUserName.Data = "lblUserName";
            this.lblUserName.Text = "Username";
            this.lblUserName.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.lblUserName);
            
            this.lblPassword.Width = 4;
            this.lblPassword.Height = 1;
            this.lblPassword.X = 40;
            this.lblPassword.Y = 4;
            this.lblPassword.Visible = true;
            this.lblPassword.Data = "lblPassword";
            this.lblPassword.Text = "Password";
            this.lblPassword.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.lblPassword);
            
            this.txtUserName.Width = 21;
            this.txtUserName.Height = 1;
            this.txtUserName.X = 1;
            this.txtUserName.Y = 5;
            this.txtUserName.Visible = true;
            this.txtUserName.Secret = false;
            this.txtUserName.Data = "txtUserName";
            this.txtUserName.Text = "";
            this.txtUserName.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.txtUserName);
            
            this.btnCopyUserName.Width = 8;
            this.btnCopyUserName.Height = 1;
            this.btnCopyUserName.X = 23;
            this.btnCopyUserName.Y = 5;
            this.btnCopyUserName.Visible = true;
            this.btnCopyUserName.Data = "btnCopyUserName";
            this.btnCopyUserName.Text = "Copy";
            this.btnCopyUserName.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnCopyUserName.IsDefault = false;
            this.btnCopyUserName.Clicked += CopyUserNameButtonClicked;
            this.scrMain.Add(this.btnCopyUserName);
            
            this.txtPassword.Width = 21;
            this.txtPassword.Height = 1;
            this.txtPassword.X = 40;
            this.txtPassword.Y = 5;
            this.txtPassword.Visible = true;
            this.txtPassword.Secret = true;
            this.txtPassword.Data = "txtPassword";
            this.txtPassword.Text = "";
            this.txtPassword.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.txtPassword);
            
            this.btnViewPassword.Width = 8;
            this.btnViewPassword.Height = 1;
            this.btnViewPassword.X = 62;
            this.btnViewPassword.Y = 5;
            this.btnViewPassword.Visible = true;
            this.btnViewPassword.Data = "btnViewPassword";
            this.btnViewPassword.Text = "View";
            this.btnViewPassword.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnViewPassword.IsDefault = false;
            this.btnViewPassword.Clicked += ViewPasswordButtonClicked;
            this.scrMain.Add(this.btnViewPassword);
            
            this.btnCopyPassword.Width = 8;
            this.btnCopyPassword.Height = 1;
            this.btnCopyPassword.X = 71;
            this.btnCopyPassword.Y = 5;
            this.btnCopyPassword.Visible = true;
            this.btnCopyPassword.Data = "btnCopyPassword";
            this.btnCopyPassword.Text = "Copy";
            this.btnCopyPassword.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnCopyPassword.IsDefault = false;
            this.btnCopyPassword.Clicked += CopyPasswordButtonClicked;
            this.scrMain.Add(this.btnCopyPassword);
            
            this.btnGeneratePassword.Width = 12;
            this.btnGeneratePassword.Height = 1;
            this.btnGeneratePassword.X = 80;
            this.btnGeneratePassword.Y = 5;
            this.btnGeneratePassword.Visible = true;
            this.btnGeneratePassword.Data = "btnGeneratePassword";
            this.btnGeneratePassword.Text = "Generate";
            this.btnGeneratePassword.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnGeneratePassword.IsDefault = false;
            this.btnGeneratePassword.Clicked += GeneratePasswordButtonClicked;
            this.scrMain.Add(this.btnGeneratePassword);
            
            this.lblTOTP.Width = 4;
            this.lblTOTP.Height = 1;
            this.lblTOTP.X = 1;
            this.lblTOTP.Y = 7;
            this.lblTOTP.Visible = true;
            this.lblTOTP.Data = "lblTOTP";
            this.lblTOTP.Text = "Authenticator Key (TOTP)";
            this.lblTOTP.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.lblTOTP);
            
            this.txtTOTP.Width = 30;
            this.txtTOTP.Height = 1;
            this.txtTOTP.X = 1;
            this.txtTOTP.Y = 8;
            this.txtTOTP.Visible = true;
            this.txtTOTP.Secret = false;
            this.txtTOTP.Data = "txtTOTP";
            this.txtTOTP.Text = "";
            this.txtTOTP.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.txtTOTP);
            
            this.fraURIList.Width = 97;
            this.fraURIList.Height = 7;
            this.fraURIList.X = 1;
            this.fraURIList.Y = 10;
            this.fraURIList.Visible = true;
            this.fraURIList.Data = "fraURIList";
            this.fraURIList.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
            this.fraURIList.Border.Effect3D = false;
            this.fraURIList.Border.Effect3DBrush = null;
            this.fraURIList.Border.DrawMarginFrame = true;
            this.fraURIList.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.fraURIList.Title = "URI List";
            this.scrMain.Add(this.fraURIList);
            
            this.scrURIList.Width = 95;
            this.scrURIList.Height = 5;
            this.scrURIList.X = 0;
            this.scrURIList.Y = 1;
            this.scrURIList.Visible = true;
            this.scrURIList.ContentSize = new Size(95,10);
            this.scrURIList.Data = "scrURIList";
            this.scrURIList.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.fraURIList.Add(this.scrURIList);
            
            this.btnNewURI.Width = 8;
            this.btnNewURI.Height = 1;
            this.btnNewURI.X = 1;
            this.btnNewURI.Y = 17;
            this.btnNewURI.Visible = true;
            this.btnNewURI.Data = "btnNewURI";
            this.btnNewURI.Text = "New URI";
            this.btnNewURI.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnNewURI.IsDefault = false;
            this.btnNewURI.Clicked += NewUriButtonClicked;
            this.scrMain.Add(this.btnNewURI);
            
            this.fraNotes.Width = 97;
            this.fraNotes.Height = 9;
            this.fraNotes.X = 1;
            this.fraNotes.Y = 19;
            this.fraNotes.Visible = true;
            this.fraNotes.Data = "fraNotes";
            this.fraNotes.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
            this.fraNotes.Border.Effect3D = false;
            this.fraNotes.Border.Effect3DBrush = null;
            this.fraNotes.Border.DrawMarginFrame = true;
            this.fraNotes.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.fraNotes.Title = "Notes";
            this.scrMain.Add(this.fraNotes);
            
            this.tvwNotes.Width = 95;
            this.tvwNotes.Height = 7;
            this.tvwNotes.X = 1;
            this.tvwNotes.Y = 0;
            this.tvwNotes.Visible = true;
            this.tvwNotes.AllowsTab = true;
            this.tvwNotes.AllowsReturn = true;
            this.tvwNotes.WordWrap = false;
            this.tvwNotes.Data = "tvwNotes";
            this.tvwNotes.Text = "\t";
            this.tvwNotes.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.fraNotes.Add(this.tvwNotes);
            
            this.stbDetail.Width = Dim.Fill(0);
            this.stbDetail.Height = 1;
            this.stbDetail.X = 0;
            this.stbDetail.Y = Pos.AnchorEnd(1);
            this.stbDetail.Visible = true;
            this.stbDetail.Data = "stbDetail";
            this.stbDetail.Text = "";
            this.stbDetail.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.f1EditMe = new Terminal.Gui.StatusItem(((Terminal.Gui.Key)(1048588u)), "F1 - Edit Me", null);
            this.stbDetail.Items = new Terminal.Gui.StatusItem[] {
                    this.f1EditMe};
            this.Add(this.stbDetail);
        }
    }
}
