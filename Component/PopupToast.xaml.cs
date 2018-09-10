using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace MediumServerExample.Component
{
    public sealed partial class PopupToast : UserControl
    {
        private string _popupContent;
        
        private Popup _popup = null;
        public PopupToast()
        {
            this.InitializeComponent();
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            
            _popup = new Popup();
            _popup.Child = this;
            
            this.Loaded += PopupNoticeLoaded;
        }

        public PopupToast(string popupContentString) : this()
        {
            _popupContent = popupContentString;
        }
        
        public void ShowPopup()
        {
            _popup.IsOpen = true;
        }
        
        public void PopupNoticeLoaded(object sender, RoutedEventArgs e)
        {
            PopupContent.Text = _popupContent;
            
            this.PopupIn.Begin();
            this.PopupIn.Completed += PopupInCompleted;
        }
        
        public async void PopupInCompleted(object sender, object e)
        {
            await Task.Delay(1500);
            this.PopupOut.Begin();
            this.PopupOut.Completed += PopupOutCompleted;
        }
        
        public void PopupOutCompleted(object sender, object e)
        {
            _popup.IsOpen = false;
        }
    }
}
