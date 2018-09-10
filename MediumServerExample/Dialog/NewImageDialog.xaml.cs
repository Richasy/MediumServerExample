using MediumServerExample.Tools;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace MediumServerExample.Dialog
{
    public sealed partial class NewImageDialog : ContentDialog
    {
        private StorageFile ImageFile;
        public NewImageDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            if (ImageFile == null)
            {
                new Component.PopupToast("Please select a image first.").ShowPopup();
                return;
            }
            IsPrimaryButtonEnabled = false;
            PrimaryButtonText = "Uploading...";

            string name = NameBox.Text ?? "image";

            var result = await MainPage.Current.Helper.UploadImage(ImageFile, name);

            if (result != null)
            {
                MainPage.Current.ShowImageInfo(result);
                Hide();
            }
            IsPrimaryButtonEnabled = true;
            PrimaryButtonText = "Upload";
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void GetImageButton_Click(object sender, RoutedEventArgs e)
        {
            var file = await IOTools.OpenLocalFile(".jpg", ".png", ".gif", ".tiff");
            if (file != null)
            {
                ImageFile = file;
            }
            PathTextBox.Text = ImageFile.Path;
            NameBox.Text = Path.GetFileNameWithoutExtension(ImageFile.Path);
        }
    }
}
