using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DZZMan.ViewModels;

namespace DZZMan.Views
{
    public partial class TLEManager : Window
    {
        public TLEManager()
        {
            InitializeComponent();

            DataContext = new TLEManagerViewModel();

#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
