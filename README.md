Most mobile apps are what we consider connected apps (they have some form of backend infrastructure). [Pierce](https://github.com/pierceboggan) and I have spent the last few months building multiple Xamarin based apps the consume Azure App Services. During this time we've found ourselves writing the same code over and over which prompted us to put it into an easily consumable package and share on Nuget. 

## Follow Us 

* Mike: [MikeCodesDotNet](https://twitter.com/mikecodesdotnet)
* Pierce: [PierceBoggan](https://twitter.com/pierceboggan)

## Build Status 
[![Build Status](https://www.bitrise.io/app/0cf5c8b31d7f3357.svg?token=-Eq6S4qlUcM-GtWwdOpY0Q&branch=master)](https://www.bitrise.io/app/0cf5c8b31d7f3357)

## What is it? 

#### Azure.Mobile 
The base Azure.Mobile package contains everything you need as a C# developer to get quickly started with Azure App Service. We've  tried our best to remove any of the complexities of the App Service SDKs and boilerplate code that we're usually forced to write. 

#### Azure.Mobile.Forms
The Forms package is an extra download that depends on Azure.Mobile. The package contains a ViewModel base class implementation which allows you to bind from XAML to Azure tables with minimal configuration on your end (just a couple of lines of code). 

## Sample Apps

We've built a couple of samples to get you started. These naturally target Xamarin.Forms as well as Traditional Xamarin. There's no reason why Azure.Mobile can't be used outside of Xamarin and in any.NET application if you desire.

* [Xamarin.Forms](https://github.com/MikeCodesDotNet/Azure.Mobile/tree/forms-sample/Samples/Xamarin.Forms)
* Xamarin.iOS
* Xamarin.Android
* Xamarin.Mac 

### License
Licensed under MIT see License file.
