using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Sample1.ViewModels;
using System.Windows;

namespace MaSchoeller.Extensions.Desktop.Sample1
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window, IDesktopShell
    {
        public ShellWindow(NavigationFrame frame, ShellViewModel viewModel)
        {
            InitializeComponent();
            Container.Child = frame;
            DataContext = viewModel;
        }
    }
}
