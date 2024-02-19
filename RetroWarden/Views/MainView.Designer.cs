using System;
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
        private Terminal.Gui.TableView tabItems;
        private Terminal.Gui.MenuBar mnuMain;
        private Terminal.Gui.StatusBar staMain;
        private Terminal.Gui.StatusItem stiHelp;
        
        private void InitializeComponent() {
            
            // Allocate controls.
            this.staMain = new Terminal.Gui.StatusBar();
            this.mnuMain = new Terminal.Gui.MenuBar();
            this.tabItems = new Terminal.Gui.TableView();
            this.fraVault = new Terminal.Gui.FrameView();
            this.tvwItems = new Terminal.Gui.TreeView();
            this.fraItems = new Terminal.Gui.FrameView();
            this.winMain = new Terminal.Gui.Window();
            
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
            
            this.tabItems.Width = 84;
            this.tabItems.Height = 26;
            this.tabItems.X = 0;
            this.tabItems.Y = 0;
            this.tabItems.Visible = true;
            this.tabItems.Data = "tabItems";
            this.tabItems.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.tabItems.FullRowSelect = true;
            this.tabItems.MultiSelect = true;
            this.tabItems.SelectedCellChanged += HandleItemTableSelectedCellChanged;
            this.tabItems.Style.AlwaysShowHeaders = true;
            this.tabItems.Style.ExpandLastColumn = true;
            this.tabItems.Style.InvertSelectedCellFirstCharacter = false;
            this.tabItems.Style.ShowHorizontalHeaderOverline = false;
            this.tabItems.Style.ShowHorizontalHeaderUnderline = true;
            this.tabItems.Style.ShowVerticalCellLines = false;
            this.tabItems.Style.ShowVerticalHeaderLines = false;
            this.fraVault.Add(this.tabItems);
            
            this.mnuMain = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_File", new MenuItem[]
                {
                    new MenuItem("_Connect...", "Connect to vault", HandleConnectionRequest, null, 
                        null),
                    new MenuItem("_Quit", "Quit Application", HandleQuitRequest, null, 
                        null)
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
            this.staMain.Text = "s";
            this.staMain.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.stiHelp = new Terminal.Gui.StatusItem(((Terminal.Gui.Key)(1048588u)), "F1 - Help", HandleHelpKeypress);
            this.staMain.Items = new Terminal.Gui.StatusItem[] {
                    this.stiHelp};
            this.Add(this.staMain);
        }
    }
}
