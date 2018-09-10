using MediumServerExample.Tools;
using System;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace MediumServerExample.Dialog
{
    public sealed partial class AuthorizeDialog : ContentDialog
    {
        private bool IsInt = false;
        public AuthorizeDialog(string tip,bool isInt)
        {
            this.InitializeComponent();
            TipTextBlock.Text = tip;
            IsInt = isInt;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            string code = CodeTextBox.Text;
            if (String.IsNullOrEmpty(code.Trim()))
            {
                new Component.PopupToast("Code can't be empty").ShowPopup();
                return;
            }
            IsPrimaryButtonEnabled = false;
            PrimaryButtonText = "Validating...";
            await MainPage.Current.Helper.FillClient(code,IsInt);
            bool isAuthorized = Convert.ToBoolean(AppTools.GetLocalSetting(Settings.IsAuthorized, "False"));
            if (isAuthorized)
            {
                MainPage.Current.ShowUserInfo();
                Hide();
            }
            else
            {
                IsPrimaryButtonEnabled = true;
                PrimaryButtonText = "Retry";
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
