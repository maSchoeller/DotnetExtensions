# Generic Host wpf extension

## Ziele

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
Erstellen eines neuen Konsolen/WPF Projekts
```powershell
dotnet new console
# or
dotnet new wpf
```
**Falls ein Konsolen projekt erstellt wurde muss im `.csproj file` eine anpassung gemacht werden:**

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

**Falls eine Wpf Applikation erstellt wurde kann die `App.xaml` gelöscht werden.**

Danach werden zwei Packet referenzen benötigt:
[Zum Nugetfeed der erweiterung](https://www.nuget.org/packages/MaSchoeller.Extensions.Desktop/)


- Installation über `nuget.exe`
```powershell
Install-Package MaSchoeller.Extensions.Desktop -Version 0.1.3
Install-Package Microsoft.Extensions.Hosting -Version 3.1.0
```
- Installation über `dotnet cli`
```cmd
dotnet add package MaSchoeller.Extensions.Desktop --version 0.1.3
dotnet add package Microsoft.Extensions.Hosting --version 3.1.0
```
- Installation über das anpassen über das `.csproj file`.
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="3.1.0" />
    <PackageReference Include="MaSchoeller.Extensions.Desktop" Version="0.1.3" />
</ItemGroup>
```
### Basic
Anpassung der `Program.cs`

```csharp
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Sample1
{
    class Program
    {
       
        static async Task Main(string[] args)
        {  
            await Host.CreateDefaultBuilder(args)
                    .ConfigureDesktopDefaults<MainWindow>()
                    .Build()
                    .RunAsync();
        }
    }
}
```

Anpassungen an der `MainWindow.xaml.cs` 
```csharp
namespace Sample1
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDesktopShell
    {
        public ShellWindow(CustomeMainViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}

```

### Startup

### Navigation

### Splashscreen

