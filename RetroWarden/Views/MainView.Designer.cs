using System;
using System.Xml;
using Terminal.Gui;
using Retrowarden.Utils;

namespace Retrowarden.Views 
{
    public partial class MainView : Terminal.Gui.Toplevel 
    {
        private Terminal.Gui.Window winMain;
        private Terminal.Gui.FrameView fraItems;
        private Terminal.Gui.TreeView tvwItems;
        private Terminal.Gui.FrameView fraVault;
        private Terminal.Gui.Label lblItemName;
        private Terminal.Gui.Label lblUserId;
        private Terminal.Gui.Label lblOwner;
        private Terminal.Gui.ListView lvwItems;
        private Terminal.Gui.MenuBar mnuMain;
        private Terminal.Gui.StatusBar staMain;
        private Terminal.Gui.StatusItem stiHelp;
        private Terminal.Gui.StatusItem stiView;
        private Terminal.Gui.StatusItem stiEdit; 
        private Terminal.Gui.StatusItem stiCopyUser;
        private Terminal.Gui.StatusItem stiCopyPwd;
        private Terminal.Gui.StatusItem stiFolderMove; 
        private Terminal.Gui.StatusItem stiCollectionMove;

        private void InitializeComponent() {
            
            // Allocate controls.
            this.lblItemName = new Terminal.Gui.Label();
            this.lblUserId = new Terminal.Gui.Label();
            this.lblOwner = new Terminal.Gui.Label();
            this.staMain = new Terminal.Gui.StatusBar();
            this.mnuMain = new Terminal.Gui.MenuBar();
            this.fraVault = new Terminal.Gui.FrameView();
            this.tvwItems = new Terminal.Gui.TreeView();
            this.fraItems = new Terminal.Gui.FrameView();
            this.winMain = new Terminal.Gui.Window();
            this.lvwItems = new ListView();
            
            // Top level window settings.
            this.Width = Dim.Fill(0);
            this.Height = Dim.Fill(0);
            this.X = 0;
            this.Y = 0;
            this.Visible = true;
            this.Modal = false;
            this.IsMdiContainer = false;
            this.TextAlignment = Terminal.Gui.TextAlignment.Left;
            
            // Window settings.
            this.winMain.Width = 120;
            this.winMain.Height = 29;
            this.winMain.X = 0;
            this.winMain.Y = 0;
            this.winMain.Visible = true;
            this.winMain.Modal = false;
            this.winMain.IsMdiContainer = false;
            this.winMain.Data = "winMain";
            this.winMain.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
            this.winMain.Border.Effect3D = false;
            this.winMain.Border.Effect3DBrush = null;
            this.winMain.Border.DrawMarginFrame = true;
            this.winMain.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.winMain.Title = "Retrowarden";
            this.Add(this.winMain);
            
            this.fraItems.Width = 31;
            this.fraItems.Height = 27;
            this.fraItems.X = 0;
            this.fraItems.Y = 1;
            this.fraItems.Visible = true;
            this.fraItems.Data = "fraItems";
            this.fraItems.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
            this.fraItems.Border.Effect3D = false;
            this.fraItems.Border.Effect3DBrush = null;
            this.fraItems.Border.DrawMarginFrame = true;
            this.fraItems.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.fraItems.Title = "Items";
            this.winMain.Add(this.fraItems);
            
            this.tvwItems.Width = 29;
            this.tvwItems.Height = 26;
            this.tvwItems.X = 0;
            this.tvwItems.Y = 0;
            this.tvwItems.Visible = true;
            this.tvwItems.Data = "tvwItems";
            this.tvwItems.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.tvwItems.Style.CollapseableSymbol = '-';
            this.tvwItems.Style.ColorExpandSymbol = false;
            this.tvwItems.Style.ExpandableSymbol = '+';
            this.tvwItems.Style.InvertExpandSymbolColors = false;
            this.tvwItems.Style.LeaveLastRow = false;
            this.tvwItems.Style.ShowBranchLines = true;
            this.tvwItems.SelectionChanged += HandleTreeviewSelectionChanged;
            this.fraItems.Add(this.tvwItems);
            
            this.fraVault.Width = 86;
            this.fraVault.Height = 27;
            this.fraVault.X = 31;
            this.fraVault.Y = 1;
            this.fraVault.Visible = true;
            this.fraVault.Data = "fraVault";
            this.fraVault.Border.BorderStyle = Terminal.Gui.BorderStyle.Single;
            this.fraVault.Border.Effect3D = false;
            this.fraVault.Border.Effect3DBrush = null;
            this.fraVault.Border.DrawMarginFrame = true;
            this.fraVault.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.fraVault.Title = "All Vaults";
            this.winMain.Add(this.fraVault);

            this.lblItemName.X = 0;
            this.lblItemName.Y = 0;
            this.lblItemName.Width = 30;
            this.lblItemName.Height = 1;
            this.lblItemName.Enabled = true;
            this.lblItemName.Visible = false;
            this.lblItemName.Text = "Item Name";
            this.fraVault.Add(lblItemName);
            
            this.lblUserId.X = 33;
            this.lblUserId.Y = 0;
            this.lblUserId.Width = 30;
            this.lblUserId.Height = 1;
            this.lblUserId.Enabled = true;
            this.lblUserId.Visible = false;
            this.lblUserId.Text = "User Id";
            this.fraVault.Add(lblUserId);
            
            this.lblOwner.X = 64;
            this.lblOwner.Y = 0;
            this.lblOwner.Width = 20;
            this.lblOwner.Height = 1;
            this.lblOwner.Enabled = true;
            this.lblOwner.Visible = false;
            this.lblOwner.Text = "Owner";
            this.fraVault.Add(lblOwner);
            
            this.lvwItems.Width = 84;
            this.lvwItems.Height = 25;
            this.lvwItems.X = 0;
            this.lvwItems.Y = 1;
            this.lvwItems.Visible = true;
            this.lvwItems.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.lvwItems.AllowsMultipleSelection = true;
            this.lvwItems.AllowsMarking = true;
            this.lvwItems.SelectedItemChanged += HandleListViewSelectedItemChanged;
            this.lvwItems.KeyPress += HandleListViewKeyUp;
            this.lvwItems.OpenSelectedItem += HandleListViewOpenItem;
            this.lvwItems.Enter += HandleListviewEnter;
            this.lvwItems.MouseClick += HandleListviewMouseClick;
            this.fraVault.Add(lvwItems);
            
            this.mnuMain = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_File", new MenuItem[]
                {
                    new MenuItem("_Connect...", "Connect to vault", HandleConnectionRequest, null, 
                        null),
                    new MenuItem("_Quit", "Quit Application", HandleQuitRequest, null, 
                        null)
                }),
                new MenuBarItem("_Options", new MenuItem[]
                {
                    new MenuItem("Boomer Mode!","Disable/Enable Mouse Use", HandleBoomerMode,null,null)
                    {
                        Checked = false,
                        CheckType = MenuItemCheckStyle.Checked
                    }
                }),
                new MenuBarItem ("_Help", new MenuItem [] {
                    new MenuItem ("_About...",
                        "", () =>  MessageBox.Query ("About Retrowarden", _aboutMessage.ToString(), "_Ok"), null, null, Key.CtrlMask | Key.A),
                })
            });
            
            this.mnuMain.Width = Dim.Fill(0);
            this.mnuMain.Height = 1;
            this.mnuMain.X = 0;
            this.mnuMain.Y = 0;
            this.mnuMain.Visible = true;
            this.mnuMain.Data = "mnuMain";
            this.mnuMain.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.winMain.Add(this.mnuMain);
            
            this.staMain.Width = Dim.Fill(0);
            this.staMain.Height = 1;
            this.staMain.X = 0;
            this.staMain.Y = Pos.AnchorEnd(1);
            this.staMain.Visible = true;
            this.staMain.Data = "staMain";
            this.staMain.Text = "";
            this.staMain.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.stiHelp = new Terminal.Gui.StatusItem(((Terminal.Gui.Key)(1048588u)), "~F1~ New", HandleCreateKeypress);
            this.staMain.Items = new Terminal.Gui.StatusItem[] {
                    this.stiHelp};
            
            // Create new status bar items.  These will get added as context functions when items are selected.
            stiView = new StatusItem(Key.F2, "~F2~ View", HandleViewItemKeypress);
            stiEdit = new StatusItem(Key.F3, "~F3~ Edit", HandleEditItemKeypress);
            stiCopyUser = new StatusItem(Key.F4, "~F4~ Copy UserName", HandleUserCopyKeypress);
            stiCopyPwd = new StatusItem(Key.F5, "~F5~ Copy Password", HandlePwdCopyKeypress);
            stiFolderMove = new StatusItem(Key.F6, "~F6~ Move To Folder", HandleFolderMoveKeypress);
            stiCollectionMove = new StatusItem(Key.F7, "~F7~ Move To Collection", HandleCollectionMoveKeypress);

            this.Add(this.staMain);
        }
    }
}
