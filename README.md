![App Service Helpers Banner](assets/readmeBanner.png)

# App Service Helpers 
App Service Helpers (ASH) makes it as easy as possible to add data storage with sync support to your Xamarin based mobile apps with [Microsoft's Azure App Service Platform](https://azure.microsoft.com/en-us/services/app-service/mobile/?WT.mc_id=ashpackage-github-mijam). ASH was built with the mobile developer in mind and requires no previous experience with developing backend infrastructure. Its entire
purpose is to allow you to focus on the mobile app! 

## Why
ASH removes a lot of complexitities of developing cloud connected apps by allowing you to add online/offline synchronization functionality to apps in just four lines of code. Because ASH is an abstraction API, its even possible to Authenticate users with Facebook, Twitter, Google, Microsoft accounts, Azure AD, and even Azure B2C. ASH even takes care of securely storing access tokens and refreshing them regularly,with no extra effort required from you.

App Service Helpers was developed as a supplemental library to [Microsoft's Azure Client SDK](https://www.nuget.org/packages/Microsoft.Azure.Mobile.Client/). Rather than replacing this library, ASH extends it by lowering the barrier to entry for developers who wish to build cloud-connected mobile apps in C#. If you ever find yourself outgrowing App Service Helpers, you can drop down to a lower level with the Microsoft Azure Client SDK for fine-tuned control or even remove ASH with minimal refactoring.

## Supported Platforms
- [.NET Standard 1.4](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support?WT.mc_id=ashpackage-github-mijam)
- [Xamarin Android](https://docs.microsoft.com/en-us/xamarin/android/?WT.mc_id=ashpackage-github-mijam) for API 19 through 24 (KitKat through Nougat)
- [Xamarin iOS](https://docs.microsoft.com/en-us/xamarin/ios/index?WT.mc_id=ashpackage-github-mijam) for iOS versions 8.0 through 10.0
- [Xamarin.Forms](https://docs.microsoft.com/en-us/xamarin/#pivot=xamarin-forms?WT.mc_id=ashpackage-github-mijam) (Android, iOS and UWP)
- [Universal Windows Platform](https://docs.microsoft.com/en-us/windows/uwp/?WT.mc_id=ashpackage-github-mijam)

## Quick Start 

The most basic usage of the library can be achieved with just 4 lines of code. Below shows how to do this in the context of a To Do app. 

1. Add Nuget Package

```bash
Install-Package AppService.Helpers
```
2. Intialise
```csharp
var client = new EasyMobileServiceClient();
client.Initialize("YOUR_AZURE_URL_GOES_HERE");

//Register our objects
client.RegisterTable<ToDo>();
client.FinalizeSchema();
```
3. Using
You can now create a ConnectedObservableCollection of your type for use in databindings.

```csharp
var Todos = new ConnectedObservableCollection<ToDo>(client.Table<ToDo>());
```

## Learn More with Docs! 
You can find a compresentive set of docs over at [Readme.io](https://ashlibrary.readme.io/docs)

### More Resources
- [Xamarin Docs](https://docs.microsoft.com/en-us/xamarin/?WT.mc_id=ashpackage-github-mijam)
- [Azure App Service Platform](https://azure.microsoft.com/en-us/services/app-service/mobile/?WT.mc_id=ashpackage-github-mijam)

## Problems or Suggestions
[Open an issue here](../..//issues)

## Authors
Developed by former Xamarin employees, now working for Microsoft.

|        ![Photo](assets/mike.png)       |   ![Photo](assets/pierce.png)   |
|:----------------------------------------------:|:--------------------------------------------:|
|                 **Mike James**                 |            **Pierce Boggan**            |
|  [GitHub](https://github.com/MikeCodesDotNet)  | [GitHub](https://github.com/pierceboggan) |
| [Twitter](https://twitter.com/MikeCodesDotNet) | [Twitter](https://twitter.com/pierceboggan)  |
        

## License
Licensed under MIT see License file.
