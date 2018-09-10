using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace MediumServerExample.Dialog
{
    public sealed partial class TipDialog : ContentDialog
    {
        public TipDialog(string content)
        {
            this.InitializeComponent();
            TipTexBlock.Text = content;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
