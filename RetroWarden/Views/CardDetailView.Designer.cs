using System;
using Terminal.Gui;

namespace Retrowarden.Views 
{
    public partial class CardDetailView : ItemDetailView
    {
        private Terminal.Gui.View vwCard;
        private Terminal.Gui.Label lblCardholderName;
        private Terminal.Gui.Label lblCardBrand;
        private Terminal.Gui.TextField txtCardholderName;
        private Terminal.Gui.ComboBox cboCardBrand;
        private Terminal.Gui.Label lblCardNumber;
        private Terminal.Gui.Label lblCVV;
        private Terminal.Gui.TextField txtCardNumber;
        private Terminal.Gui.Button btnShowCardNumber;
        private Terminal.Gui.Button btnCopyCardNumber;
        private Terminal.Gui.TextField txtCVV;
        private Terminal.Gui.Button btnShowCVV;
        private Terminal.Gui.Button btnCopyCVV;
        private Terminal.Gui.Label lblExpMonth;
        private Terminal.Gui.Label lblExpYear;
        private Terminal.Gui.ComboBox cboExpMonth;
        private Terminal.Gui.TextField txtExpYear;

        private void InitializeComponent()
        {
            this.vwCard = new View();
            this.txtExpYear = new Terminal.Gui.TextField();
            this.cboExpMonth = new Terminal.Gui.ComboBox();
            this.lblExpYear = new Terminal.Gui.Label();
            this.lblExpMonth = new Terminal.Gui.Label();
            this.btnCopyCVV = new Terminal.Gui.Button();
            this.btnShowCVV = new Terminal.Gui.Button();
            this.txtCVV = new Terminal.Gui.TextField();
            this.btnCopyCardNumber = new Terminal.Gui.Button();
            this.btnShowCardNumber = new Terminal.Gui.Button();
            this.txtCardNumber = new Terminal.Gui.TextField();
            this.lblCVV = new Terminal.Gui.Label();
            this.lblCardNumber = new Terminal.Gui.Label();
            this.cboCardBrand = new Terminal.Gui.ComboBox();
            this.txtCardholderName = new Terminal.Gui.TextField();
            this.lblCardBrand = new Terminal.Gui.Label();
            this.lblCardholderName = new Terminal.Gui.Label();
            
            vwCard.Width = 99;
            vwCard.Height = 9;
            vwCard.X = 0;
            vwCard.Y = 3;
            vwCard.Visible = true;
            vwCard.TextAlignment = Terminal.Gui.TextAlignment.Left;
                       
            this.lblCardholderName.Width = 4;
            this.lblCardholderName.Height = 1;
            this.lblCardholderName.X = 1;
            this.lblCardholderName.Y = 0;
            this.lblCardholderName.Visible = true;
            this.lblCardholderName.Data = "lblCardholderName";
            this.lblCardholderName.Text = "Cardholder Name";
            this.lblCardholderName.TextAlignment = Terminal.Gui.TextAlignment.Left;
            vwCard.Add(this.lblCardholderName);
            
            this.lblCardBrand.Width = 4;
            this.lblCardBrand.Height = 1;
            this.lblCardBrand.X = 40;
            this.lblCardBrand.Y = 0;
            this.lblCardBrand.Visible = true;
            this.lblCardBrand.Data = "lblCardBrand";
            this.lblCardBrand.Text = "Card Brand";
            this.lblCardBrand.TextAlignment = Terminal.Gui.TextAlignment.Left;
            vwCard.Add(this.lblCardBrand);
           
            this.txtCardholderName.Width = 30;
            this.txtCardholderName.Height = 1;
            this.txtCardholderName.X = 1;
            this.txtCardholderName.Y = 1;
            this.txtCardholderName.Visible = true;
            this.txtCardholderName.Secret = false;
            this.txtCardholderName.Data = "txtCardholderName";
            this.txtCardholderName.Text = "";
            this.txtCardholderName.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.txtCardholderName.TabIndex = 3;
            this.txtCardholderName.DesiredCursorVisibility = CursorVisibility.Box;
            vwCard.Add(this.txtCardholderName);
            
            this.cboCardBrand.Width = 30;
            this.cboCardBrand.Height = 2;
            this.cboCardBrand.X = 40;
            this.cboCardBrand.Y = 1;
            this.cboCardBrand.Visible = true;
            this.cboCardBrand.Data = "cboCardBrand";
            this.cboCardBrand.Text = "";
            this.cboCardBrand.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.cboCardBrand.TabIndex = 4;
            vwCard.Add(this.cboCardBrand);
            
            this.lblCardNumber.Width = 4;
            this.lblCardNumber.Height = 1;
            this.lblCardNumber.X = 1;
            this.lblCardNumber.Y = 3;
            this.lblCardNumber.Visible = true;
            this.lblCardNumber.Data = "lblCardNumber";
            this.lblCardNumber.Text = "Card Number";
            this.lblCardNumber.TextAlignment = Terminal.Gui.TextAlignment.Left;
            vwCard.Add(this.lblCardNumber);
            
            this.lblCVV.Width = 4;
            this.lblCVV.Height = 1;
            this.lblCVV.X = 40;
            this.lblCVV.Y = 3;
            this.lblCVV.Visible = true;
            this.lblCVV.Data = "lblCVV";
            this.lblCVV.Text = "Security Code";
            this.lblCVV.TextAlignment = Terminal.Gui.TextAlignment.Left;
            vwCard.Add(this.lblCVV);
           
            this.txtCardNumber.Width = 18;
            this.txtCardNumber.Height = 1;
            this.txtCardNumber.X = 1;
            this.txtCardNumber.Y = 4;
            this.txtCardNumber.Visible = true;
            this.txtCardNumber.Secret = true;
            this.txtCardNumber.Data = "txtCardNumber";
            this.txtCardNumber.Text = "";
            this.txtCardNumber.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.txtCardNumber.TabIndex = 5;
            vwCard.Add(this.txtCardNumber);
            
            this.btnShowCardNumber.Width = 8;
            this.btnShowCardNumber.Height = 1;
            this.btnShowCardNumber.X = 20;
            this.btnShowCardNumber.Y = 4;
            this.btnShowCardNumber.Visible = true;
            this.btnShowCardNumber.Data = "btnShowCardNumber";
            this.btnShowCardNumber.Text = "Show";
            this.btnShowCardNumber.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnShowCardNumber.IsDefault = false;
            this.btnShowCardNumber.TabIndex = 6;
            this.btnShowCardNumber.Clicked += ShowCardButtonClicked;
            vwCard.Add(this.btnShowCardNumber);
            
            this.btnCopyCardNumber.Width = 8;
            this.btnCopyCardNumber.Height = 1;
            this.btnCopyCardNumber.X = 29;
            this.btnCopyCardNumber.Y = 4;
            this.btnCopyCardNumber.Visible = true;
            this.btnCopyCardNumber.Data = "btnCopyCardNumber";
            this.btnCopyCardNumber.Text = "Copy";
            this.btnCopyCardNumber.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnCopyCardNumber.IsDefault = false;
            this.btnCopyCardNumber.TabIndex = 7;
            this.btnCopyCardNumber.Clicked += CopyCardButtonClicked;
            vwCard.Add(this.btnCopyCardNumber);
            
            this.txtCVV.Width = 16;
            this.txtCVV.Height = 1;
            this.txtCVV.X = 40;
            this.txtCVV.Y = 4;
            this.txtCVV.Visible = true;
            this.txtCVV.Secret = true;
            this.txtCVV.Data = "txtCVV";
            this.txtCVV.Text = "";
            this.txtCVV.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.txtCVV.TabIndex = 8;
            vwCard.Add(this.txtCVV);
            
            this.btnShowCVV.Width = 8;
            this.btnShowCVV.Height = 1;
            this.btnShowCVV.X = 56;
            this.btnShowCVV.Y = 4;
            this.btnShowCVV.Visible = true;
            this.btnShowCVV.Data = "btnShowCVV";
            this.btnShowCVV.Text = "Show";
            this.btnShowCVV.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnShowCVV.IsDefault = false;
            this.btnShowCVV.TabIndex = 9;
            this.btnShowCVV.Clicked += ShowCVVButtonClicked;
            vwCard.Add(this.btnShowCVV);
            
            this.btnCopyCVV.Width = 8;
            this.btnCopyCVV.Height = 1;
            this.btnCopyCVV.X = 66;
            this.btnCopyCVV.Y = 4;
            this.btnCopyCVV.Visible = true;
            this.btnCopyCVV.Data = "btnCopyCVV";
            this.btnCopyCVV.Text = "Copy";
            this.btnCopyCVV.TextAlignment = Terminal.Gui.TextAlignment.Centered;
            this.btnCopyCVV.IsDefault = false;
            this.btnCopyCVV.TabIndex = 10;
            this.btnCopyCVV.Clicked += CopyCVVButtonClicked;
            vwCard.Add(this.btnCopyCVV);
            
            this.lblExpMonth.Width = 4;
            this.lblExpMonth.Height = 1;
            this.lblExpMonth.X = 1;
            this.lblExpMonth.Y = 6;
            this.lblExpMonth.Visible = true;
            this.lblExpMonth.Data = "lblExpMonth";
            this.lblExpMonth.Text = "Expiration Month";
            this.lblExpMonth.TextAlignment = Terminal.Gui.TextAlignment.Left;
            vwCard.Add(this.lblExpMonth);
            
            this.lblExpYear.Width = 4;
            this.lblExpYear.Height = 1;
            this.lblExpYear.X = 40;
            this.lblExpYear.Y = 6;
            this.lblExpYear.Visible = true;
            this.lblExpYear.Data = "lblExpYear";
            this.lblExpYear.Text = "Expiration Year";
            this.lblExpYear.TextAlignment = Terminal.Gui.TextAlignment.Left;
            vwCard.Add(this.lblExpYear);
            
            this.cboExpMonth.Width = 30;
            this.cboExpMonth.Height = 2;
            this.cboExpMonth.X = 1;
            this.cboExpMonth.Y = 7;
            this.cboExpMonth.Visible = true;
            this.cboExpMonth.Data = "cboExpMonth";
            this.cboExpMonth.Text = "";
            this.cboExpMonth.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.cboExpMonth.TabIndex = 11;
            vwCard.Add(this.cboExpMonth);
            
            this.txtExpYear.Width = 20;
            this.txtExpYear.Height = 1;
            this.txtExpYear.X = 40;
            this.txtExpYear.Y = 7;
            this.txtExpYear.Visible = true;
            this.txtExpYear.Secret = false;
            this.txtExpYear.Data = "txtExpYear";
            this.txtExpYear.Text = "";
            this.txtExpYear.TextAlignment = Terminal.Gui.TextAlignment.Left;
            this.txtExpYear.TabIndex = 12;
            vwCard.Add(this.txtExpYear);
        }
    }
}
