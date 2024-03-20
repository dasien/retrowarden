using System;
using Terminal.Gui;

namespace Retrowarden.Views 
{
    public partial class ItemDetailView : Terminal.Gui.Dialog 
    {
        private Terminal.Gui.ScrollView scrMain;
        private Terminal.Gui.Label lblItemName;
        private Terminal.Gui.Label lblFolder;
        private Terminal.Gui.TextField txtItemName;
        private Terminal.Gui.ComboBox cboFolder;
        private Terminal.Gui.CheckBox chkFavorite;
        private Terminal.Gui.FrameView fraNotes;
        private Terminal.Gui.TextView tvwNotes;
        private Terminal.Gui.Button btnSave;
        private Terminal.Gui.Button btnCancel;
        
        private void InitializeComponent() 
        {
            this.tvwNotes = new Terminal.Gui.TextView();
            this.fraNotes = new Terminal.Gui.FrameView();
            this.chkFavorite = new Terminal.Gui.CheckBox();
            this.cboFolder = new Terminal.Gui.ComboBox();
            this.txtItemName = new Terminal.Gui.TextField();
            this.lblFolder = new Terminal.Gui.Label();
            this.lblItemName = new Terminal.Gui.Label();
            this.scrMain = new Terminal.Gui.ScrollView();
            this.btnSave = new Terminal.Gui.Button();
            this.btnCancel = new Terminal.Gui.Button();
            
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
            
            this.scrMain.Width = 100;
            this.scrMain.Height = 95;
            this.scrMain.X = 0;
            this.scrMain.Y = 1;
            this.scrMain.Visible = true;
            this.scrMain.ContentSize = new Size(100,120);
            this.scrMain.Data = "scrMain";
            this.scrMain.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.Add(this.scrMain);
            
            this.lblItemName.Width = 4;
            this.lblItemName.Height = 1;
            this.lblItemName.X = 1;
            this.lblItemName.Y = 0;
            this.lblItemName.Visible = true;
            this.lblItemName.Data = "lblItemName";
            this.lblItemName.Text = "Name";
            this.lblItemName.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.lblItemName);
            
            this.lblFolder.Width = 4;
            this.lblFolder.Height = 1;
            this.lblFolder.X = 40;
            this.lblFolder.Y = 0;
            this.lblFolder.Visible = true;
            this.lblFolder.Data = "lblFolder";
            this.lblFolder.Text = "Folder";
            this.lblFolder.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.lblFolder);
            
            this.txtItemName.Width = 30;
            this.txtItemName.Height = 1;
            this.txtItemName.X = 1;
            this.txtItemName.Y = 1;
            this.txtItemName.Visible = true;
            this.txtItemName.Secret = false;
            this.txtItemName.Data = "txtItemName";
            this.txtItemName.Text = "";
            this.txtItemName.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.txtItemName.Enter += (_) => HandleControlEnter(txtItemName);
            this.scrMain.Add(this.txtItemName);
            this.txtItemName.TabIndex = 0;

            this.cboFolder.Width = 30;
            this.cboFolder.Height = 5;
            this.cboFolder.X = 40;
            this.cboFolder.Y = 1;
            this.cboFolder.Visible = true;
            this.cboFolder.Data = "cboFolder";
            this.cboFolder.Text = "";
            this.cboFolder.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.scrMain.Add(this.cboFolder);
            this.cboFolder.TabIndex = 1;
            
            this.chkFavorite.Width = 10;
            this.chkFavorite.Height = 1;
            this.chkFavorite.X = 77;
            this.chkFavorite.Y = 1;
            this.chkFavorite.Visible = true;
            this.chkFavorite.Data = "chkFavorite";
            this.chkFavorite.Text = "Favorite";
            this.chkFavorite.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.chkFavorite.Checked = false;
            this.scrMain.Add(this.chkFavorite);
            this.chkFavorite.TabIndex = 2;
            
            this.fraNotes.Width = 97;
            this.fraNotes.Height = 9;
            this.fraNotes.X = 1;
            this.fraNotes.Y = 20;
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
            
            this.btnSave.Width = 8;
            this.btnSave.Height = 1;
            this.btnSave.X = Pos.Center() - 10;
            this.btnSave.Y = 30;
            this.btnSave.Visible = true;
            this.btnSave.Data = "btnSave";
            this.btnSave.Text = "Save";
            this.btnSave.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnSave.IsDefault = false;
            this.btnSave.Clicked += SaveButtonClicked;
            this.scrMain.Add(btnSave);
            
            this.btnCancel.Width = 8;
            this.btnCancel.Height = 1;
            this.btnCancel.X = Pos.Center() + 2;
            this.btnCancel.Y = 30;
            this.btnCancel.Visible = true;
            this.btnCancel.Data = "";
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnCancel.IsDefault = false;
            this.btnCancel.Clicked += CancelButtonClicked;
            this.scrMain.Add(btnCancel);
        }
    }
}
