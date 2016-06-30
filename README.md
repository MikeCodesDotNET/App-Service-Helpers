Most mobile apps are what we consider connected apps (they have some form of backend infrastructure). [Pierce](https://github.com/pierceboggan) and I have spent the last few months building multiple Xamarin based apps the consume Azure App Services. During this time we've found ourselves writing the same code over and over which prompted us to put it into an easily consumable package and share on Nuget. 

## Follow Us 

* Mike: [MikeCodesDotNet](https://twitter.com/mikecodesdotnet)
* Pierce: [PierceBoggan](https://twitter.com/pierceboggan)

## What is it? 

#### Azure.Mobile 
The base Azure.Mobile package contains everything you need as a C# developer to get quickly started with Azure App Service. We've  tried our best to remove any of the complexities of the App Service SDKs and boilerplate code that we're usually forced to write. 

#### Azure.Mobile.Forms
The Forms package is an extra download that depends on Azure.Mobile. The package contains a ViewModel base class implementation which allows you to bind from XAML to Azure tables with minimal configuration on your end (just a couple of lines of code). 

## Getting Started
### Add App Service Helpers
Configuring data access in your mobile apps with App Service Helpers is easy. Simply add the App Service Helpers NuGet, initialize the library, pass in your Azure mobile app’s URL, register a data model as a table, and finalize the schema, as seen below:
###
```c#
using AppServiceHelpers;
 
// 1. Create a new EasyMobileServiceClient.
var client = EasyMobileServiceClient.Create();
 
// 2. Initialize the library with the URL of the Azure Mobile App you created in Step #1.
// Example: http://appservicehelpers.azurewebsites.net
client.Initialize("{Your_Mobile_App_Backend_Url_Here");
 
// 3. Register a model with the EasyMobileServiceClient to create a table.
client.RegisterTable<TodoItem>();
 
// 4. Finalize the schema for our database. All table registrations must be done before this step.
client.FinalizeSchema();
```
### Subclass App Service Helpers’ View Model
App Service Helpers for Xamarin.Forms includes a preconfigured view model that handles all communication with the EasyMobileServiceClient to help you get started fast. Add the App Service Helpers for Xamarin.Forms NuGet and subclass the BaseAzureViewModel:
```c#
using AppServiceHelpers.Forms
 
public class TodoItemsViewModel : BaseFormsViewModel<TodoItem>
{
        public TodoItemsViewModel(IEasyMobileServiceClient client) : base (client) { }
}
```
## Sample Apps

* [Xamarin.Forms](https://github.com/MikeCodesDotNet/Azure.Mobile/tree/forms-sample/Samples/Xamarin.Forms)

### License
Licensed under MIT see License file.
