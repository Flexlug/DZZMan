using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DZZMan.ViewModels;
using ReactiveUI;

namespace DZZMan.Views
{
    public partial class TLEManager : ReactiveWindow<TLEManagerViewModel>
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
