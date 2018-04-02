---
title: "development"
bg: lightpink
color: black
fa-icon: plug
---

## Compile the Xamarin project

In case you did not download the code, clone the GitHub repo:

git clone https://github.com/rcervantes/azure-intelligent-hack.git

Open the solution file: `IntelligentHack.Xamarin.sln`

In your IntelligentHack\App.xaml.cs file set the following attributes:

```csharp
    Settings.FunctionURL = "https://{AzureFunctionApp}.azurewebsites.net";
```
```csharp
    Settings.Cryptography = "{AzureFunctionAppCryptographyKey}";
```

Congrats if you successfully configure the app you can run it now!

<img src="http://rcervantes.me/azure-intelligent-hack/images/intelligenthack-1.png" width="300" />

<img src="http://rcervantes.me/azure-intelligent-hack/images/intelligenthack-2.png" width="300" />

<img src="http://rcervantes.me/azure-intelligent-hack/images/intelligenthack-3.png" width="300" />

<img src="http://rcervantes.me/azure-intelligent-hack/images/intelligenthack-4.png" width="300" />

<img src="http://rcervantes.me/azure-intelligent-hack/images/intelligenthack-5.png" width="300" />

<img src="http://rcervantes.me/azure-intelligent-hack/images/intelligenthack-6.png" width="700" />

<img src="http://rcervantes.me/azure-intelligent-hack/images/intelligenthack-7.png" width="700" />

Once you have created a new report, you are now able to see the metadata in CosmosDB and the picture on Azure Table Storage.

In case you have questions or doubts you can send me an email to: robece@microsoft.com.
