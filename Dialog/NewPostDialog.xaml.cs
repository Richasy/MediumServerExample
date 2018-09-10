using MediumServer.Extends;
using MediumServer.Models;
using MediumServerExample.Tools;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace MediumServerExample.Dialog
{
    public sealed partial class NewPostDialog : ContentDialog
    {
        private string MarkdownContent;
        private PublishStatus Status;
        private ContentFormat Format;
        private string PublicationId = "";

        public NewPostDialog(string content)
        {
            this.InitializeComponent();
            MarkdownContent = content;
            PublicationInit();
        }

        private async void PublicationInit()
        {
            // Using IntegrationToken will not be able to get the publication.
            bool isInt = Convert.ToBoolean(AppTools.GetLocalSetting(Settings.IsIntegrationToken, "False"));
            if (isInt)
            {
                PublicaitonContainer.Visibility = Visibility.Collapsed;
                return;
            }
            var publications = await MainPage.Current.Helper.GetAllPublictions();
            if(publications!=null && publications.Count > 0)
            {
                PublishComboBox.ItemsSource = publications;
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            IsPrimaryButtonEnabled = false;
            PrimaryButtonText = "Creating...";
            string title = TitleBox.Text.Trim();
            string tag = TagBox.Text;
            var reg = new Regex(@"#(.*?)#");
            var tags = new string[] { tag };
            if (!String.IsNullOrEmpty(tag))
            {
                var temp = reg.Split(tag);
                var yo = new List<string>();
                int count = 0;
                for (int i = 0; i < temp.Length; i++)
                {
                    if (count == 5) { break; }
                    temp[i] = temp[i].Replace("#", "").Trim();
                    if (temp[i].Length == 0) { continue; }
                    yo.Add(temp[i]);
                    count++;
                }
                tags = yo.ToArray();
            }
            if (!String.IsNullOrEmpty(title))
            {
                bool result = await MainPage.Current.Helper.CreatePost(MarkdownContent, TitleBox.Text, Status,Format, PublicationId, tags);
                if (result)
                {
                    new Component.PopupToast("Create Successed!").ShowPopup();
                    Hide();
                }
            }
            else
            {
                new Component.PopupToast("Title can't be empty!").ShowPopup();
            }
            IsPrimaryButtonEnabled = true;
            PrimaryButtonText = "Create";
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void PublicationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (Publication)PublishComboBox.SelectedItem;
            PublicationId = item.Id;
        }

        private void PublishComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = ((ComboBoxItem)PublishComboBox.SelectedItem).Content.ToString();
            switch (name)
            {
                case "Public":
                    Status = PublishStatus.Public;
                    break;
                case "Draft":
                    Status = PublishStatus.Draft;
                    break;
                case "Unlisted":
                    Status = PublishStatus.Unlisted;
                    break;
            }
        }

        private void ContentFormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = ((ComboBoxItem)ContentFormatComboBox.SelectedItem).Content.ToString();
            switch (name)
            {
                case "Markdown":
                    Format = ContentFormat.Markdown;
                    break;
                case "HTML":
                    Format = ContentFormat.HTML;
                    break;
            }
        }
    }
}
