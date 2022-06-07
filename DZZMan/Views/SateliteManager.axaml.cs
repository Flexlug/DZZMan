using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DZZMan.ViewModels;
using ReactiveUI;

namespace DZZMan.Views
{
    public partial class TLEManager : ReactiveWindow<SateliteManagerViewModel>
    {
        public TLEManager()
        {
            InitializeComponent();

            DataContext = new SateliteManagerViewModel();

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
