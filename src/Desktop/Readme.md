# Generic Host wpf extension

## Goals

The generic host allows full access to dependency injection, configuration and logging.
By implencing for WPF applications, the viewmodels can consume service via the constructor.

Functions enabled by the generic host:
- access to `IConfiguration`:
    - Enviroment variables
    - Configuration files(.json, .xml, .ini)
    - Command line arguments
- `ILogger`
    - console logger
    - evntsource logger
    - debug logger
    - ...
- access to dependcy injection

Functions made possible by the library:
- adding a user-defined splash screen, the service can edit it via an interface during startup (`ISplashscreenWindow`).
- A navigation service (`INavigationservice`)
    - Takes care of binding viewmodels to the datacontext of the matching view.
    - The viewmodels are created via dependeny incjection, so you can use any registered service.
    - Allows you to navigate through routes (`strings`) to which the appropriate view and ViewModel is bound.
    - The view models are notified when navigating and leaving the page (`IRoutable`).
- Mvvm base extensions
    - the extensions are rudimentary and cannot be compared to a prism or mvvm light. they allow a quick start without additional dependencies.
    - Standard implementation of `INotifyPropertyChanged` => `NotifyPropertyChangedBase`.
    - `ICommand` implementation.
    - Basic implementation for `IRoutable` for navigation => `RoutableBase`.  
- `IDesktopContext` provides context properties that can be useful for WPF programming.  

## Get started
Creating a new console/WPF project
```powershell
dotnet new console
# or
dotnet new wpf
```
**If a console project has been created, a customization must be made in the `.csproj file`:**

```xml
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <UseWpf>true</UseWpf>
    <OutputType>WinExe</OutputType>
  </PropertyGroup>

  <ItemGroup>
      <PackageReference Include="MaSchoeller.Extensions.Desktop" Version="0.1.3" />
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="3.1.0" />
  </ItemGroup>
</Project>
```

**If a Wpf application has been created, the `App.xaml` can be deleted.**

After that you need two packet references:
[To the nugetfeed of the extension](https://www.nuget.org/packages/MaSchoeller.Extensions.Desktop/)


- Installation via `nuget.exe`
```powershell
Install-Package MaSchoeller.Extensions.Desktop -Version 0.1.3
Install-Package Microsoft.Extensions.Hosting -Version 3.1.0
```
- Installation via `dotnet cli`.
```cmd
dotnet add package MaSchoeller.Extensions.Desktop --version 0.1.3
dotnet add package Microsoft.Extensions.Hosting --version 3.1.0
```
- Installation via customizing the `.csproj file`.
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="3.1.0" />
    <PackageReference Include="MaSchoeller.Extensions.Desktop" Version="0.1.3" />
</ItemGroup>
```
### Basic
`Program.cs`

```csharp
using MaSchoeller.Extensions.Desktop;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Sample1
{
    class Program
    {
       
        static async Task Main(string[] args)
        {  
            await Host.CreateDefaultBuilder(args)
                    .ConfigureDesktopDefaults<MainWindow>()
                    .build()
                    .RunAsync();
        }
    }
}
```

`MainWindow.xaml.cs` 
```csharp
namespace Sample1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDesktopShell
    {
        public MainWindow(CustomeMainViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}

```
### Splashscreen
`Program.cs`

```csharp
using MaSchoeller.Extensions.Desktop;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Sample1
{
    class Program
    {
       
        static async Task Main(string[] args)
        {  
            await Host.CreateDefaultBuilder(args)
                    .ConfigureSplashscreen<Splashscreen>()
                    .ConfigureDesktopDefaults<MainWindow>(b =>
                    {
                        b.ConfigureServices(services => services.AddHostedService<CustomeService>());
                    })
                    .build()
                    .RunAsync();
        }
    }
}
```

`Splashscreen.xaml.cs`
```csharp
using MaSchoeller.Extensions.Desktop.Helpers;

namespace Sample2
{
    /// <summary>
    /// Interaction logic for Splashscreen.xaml
    /// </summary>
    public partial class Splashscreen : SplashScreenBase //Or ISplashscreenWindow
    {
        public Splashscreen()
        {
            InitializeComponent();
        }
    }
}
```

`Splashscreen.xaml`
``` xml
<sp:SplashScreenBase x:Class="Sample2.Splashscreen"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:sp="clr-namespace:MaSchoeller.Extensions.Desktop.Helpers;assembly=MaSchoeller.Extensions.Desktop"
        mc:Ignorable="d"
        Title="SplashscreenWindow" 
        Height="450" Width="800">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <TextBlock Text="{Binding ReportMessage}" />
        <ProgressBar Grid.Row="1" Value="{Binding Progress}" />
    </Grid>
</sp:SplashScreenBase>

```

The splashscreen can be edited via the interface (`ISplashscreenWindow`) at startup time.

```csharp
using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Sample2
{
    public class CustomeService : IHostedService
    {
        private readonly ISplashscreenWindow _splashscreen;

        public CustomeService(
            ISplashscreenWindow splashscreen, 
            IHostApplicationLifetime lifetime)
        {
            _splashscreen = splashscreen;
            lifetime.ApplicationStarted.Register(() =>
            {
                _splashscreen.IsBusy = false;
            });
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _splashscreen.IsBusy = true;
            _splashscreen.Progress = 30;
            _splashscreen.ReportMessage = "Some Message";
            await Task.Delay(3000);
            _splashscreen.Progress = 40;
            _splashscreen.ReportMessage = "Other Message";
            await Task.Delay(1000);
        }

        public Task StopAsync(CancellationToken cancellationToken) 
            => Task.CompletedTask;
    }
}


```

### Startup
### Navigation
