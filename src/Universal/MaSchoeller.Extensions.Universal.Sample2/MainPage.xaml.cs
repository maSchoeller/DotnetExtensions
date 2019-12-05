using MaSchoeller.Extensions.Universal.Abstracts;
using MaSchoeller.Extensions.Universal.Sample2.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MaSchoeller.Extensions.Universal.Sample2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page , INotifyPropertyChanged
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public MainViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MainPageContext context)
            {
                NavigationContainer.Content = context.Frame;
                ViewModel = context.ViewModel as MainViewModel;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavigationContainer)));
            }
        }
    }
}
