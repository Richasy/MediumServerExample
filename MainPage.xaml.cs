using MediumServerExample.Tools;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MediumServerExample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MediumHelper Helper = new MediumHelper();
        public static MainPage Current;
        private bool IsUserInit = false;
        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
        }

        private async void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            bool isAuthorized = Convert.ToBoolean(AppTools.GetLocalSetting(Settings.IsAuthorized, "False"));
            if (isAuthorized)
            {
                AuthorizeButton.IsEnabled = false;
                await Helper.FillClient();
                ShowUserInfo();
                AuthorizeButton.IsEnabled = true;
            }
            else
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }

        public async void ShowUserInfo()
        {
            var token = await IOTools.GetMediumToken();
            UserNameRun.Text = Helper.user?.Name;
            AccessTokenRun.Text = token?.AccessToken ?? "null";
            IsUserInit = Helper.user == null ? false : true;
        }

        private async void OAuth_Click(object sender, RoutedEventArgs e)
        {
            string url = Helper.GetAuthUrl();
            await Launcher.LaunchUriAsync(new Uri(url));
            var dialog = new Dialog.AuthorizeDialog("Copy the code you see in the default browser window.",false);
            await dialog.ShowAsync();
        }

        private async void Int_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialog.AuthorizeDialog("Input the Integration Token in the TextBox",true);
            await dialog.ShowAsync();
        }

        private async void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsUserInit)
            {
                ImageInfoContainer.Visibility = Visibility.Collapsed;
                var dialog = new Dialog.NewImageDialog();
                await dialog.ShowAsync();
            }
        }

        private async void GetContributorsButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsUserInit)
            {
                string result = "";
                var con = await Helper.GetAllContributors();
                if (con.Count > 0)
                {
                    foreach (var item in con)
                    {
                        result += $"Publication Id: {item.PublicationId}\n" +
                            $"User Id: {item.UserId}\n" +
                            $"Role: {item.Role}\n" +
                            $"---\n";
                    }
                    var dialog = new Dialog.TipDialog(result);
                    await dialog.ShowAsync();
                }
                new Component.PopupToast("No data").ShowPopup();
            }
        }

        private async void CreatePostButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsUserInit)
            {
                string content = InputTextBox.Text.Trim();
                if (String.IsNullOrEmpty(content))
                {
                    new Component.PopupToast("The article must have content").ShowPopup();
                    return;
                }
                var dialog = new Dialog.NewPostDialog(content);
                await dialog.ShowAsync();
            }
        }

        public void ShowImageInfo(MediumServer.Models.Image image)
        {
            ImageInfoContainer.Visibility = Visibility.Visible;
            ImageMD5Run.Text = image.Md5;
            ImageUrlRun.Text = image.Url;
        }
    }
}
