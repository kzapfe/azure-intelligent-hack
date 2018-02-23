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
    Settings.Cryptography = "{AzureFunctionAppCryptographyKey}"; //previously configured in the backend
```

In your CognitiveLocator.Droid project go to Resources\values\strings.xml and set the following attributes:

Congrats if you successfully configure the app you can run it now!

<img src="http://rcervantes.me/images/cognitive-locator-app.png" width="300" />

<img src="http://rcervantes.me/images/cognitive-locator-app2.png" width="300" />
