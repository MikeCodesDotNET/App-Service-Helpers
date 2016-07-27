 Most mobile apps are what we consider connected apps (they have some form of backend infrastructure). Myself and [Pierce](https://github.com/pierceboggan) have spent the last few months building numerous Xamarin apps that consume Azure App Services. During this time we've found ourselves writing the same code over and over which prompted us to put it into an easily consumable package and share on Nuget. 

## Follow Us 

* Mike: [MikeCodesDotNet](https://twitter.com/mikecodesdotnet)
* Pierce: [PierceBoggan](https://twitter.com/pierceboggan)

## What is it? 

#### AppService.Helpers
App Service Helpers makes it as easy as possible to add a backend to your mobile application. In just four lines of code, you can create a cloud-connected mobile app with online/offline synchronization and automatic conflict handling, so that your app continues to function even if the user loses network connectivity. App Service Helpers also comes with a preconfigured Xamarin.Forms ViewModel, so that you can bind directly to your data without having to write any additional code.

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
#### AppService.Helpers.Forms
The Forms package is an extra download that unlocks a ViewModel base class that allows you to bind from XAML to Azure tables with minimal configuration on your end (just a couple of lines of code). 

App Service Helpers for Xamarin.Forms includes a preconfigured view model that handles all communication with the EasyMobileServiceClient to help you get started fast. Add the App Service Helpers for Xamarin.Forms NuGet and subclass the BaseAzureViewModel:

```c#
using AppServiceHelpers.Forms
 
public class TodoItemsViewModel : BaseFormsViewModel<TodoItem>
{
        public TodoItemsViewModel(IEasyMobileServiceClient client) : base (client) { }
}
```
## Documentation
You can find a compresentive set of docs over at [Readme.io](https://ashlibrary.readme.io/docs)

### License
Licensed under MIT see License file.
## Sample Apps

* [Xamarin.Forms](https://github.com/MikeCodesDotNet/Azure.Mobile/tree/forms-sample/Samples/Xamarin.Forms)

### License
Licensed under MIT see License file.
