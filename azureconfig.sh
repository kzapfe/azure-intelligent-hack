#!/bin/bash

clear
echo "*****************************************************"
echo "******Azure Intelligent Hack Configuration Tool******"
echo "*****************************************************"

ConfigureAzureSubscription ()
{
    # authenticate to azure suscription
    az login
  
    echo "Verify your default subscription where you created the resources:"
    az account list 

    read -p "Paste the id of the subscription where you created the resources: " subscriptionId
    az account set --subscription $subscriptionId

    echo "The subscription you have selected is: "
    az account show

    read -p "In this subscription your previously created the resources, is that correct? (yes/no)" answer

    # all to lower case
    answer=$(echo $answer | awk '{print tolower($0)}')

    # check and act on given answer
    case $answer in
        "yes")  ProceedConfiguration ;;
        *)      echo "Please answer yes or no" ; ConfigureAzureSubscription ;;
    esac
}

ProceedConfiguration ()
{
    read -p "Introduce the name of your resource group: " resourceGroupName
    az group show --name $resourceGroupName

    ConfigureDatabase

    ConfigureFaceAPI

    ConfigureFunctionApp

    echo "Great, your backend has been configured successfully!"
    #more code here
    exit 0
}

ConfigureDatabase ()
{
    echo "* Configuring: Database>>"

    collectionName="Person"
    echo "Collection person name: " collectionName
    
    collectionBotName="Bot"
    echo "Collection bot name: " collectionBotName
    
    databaseAccountName=$resourceGroupName"-cos"
    echo "Account name: " $databaseAccountName
    
    databaseName="AzureIntelligentHack"
    echo "Database name: " $databaseName

    # create database 
    az cosmosdb database create --name $databaseAccountName --db-name $databaseName --resource-group $resourceGroupName
    echo "Database created!"

    # create person collection in documentDB database
    az cosmosdb collection create --collection-name $collectionName --name $databaseAccountName --db-name $databaseName --resource-group $resourceGroupName
    
    # create bot collection in documentDB database
    az cosmosdb collection create --collection-name $collectionBotName --name $databaseAccountName --db-name $databaseName --resource-group $resourceGroupName
  
    echo "Database configured successfully!"
}

ConfigureFaceAPI ()
{
    echo "* Configuring: Face API>>"
    
    echo "1) West US - westus.api.cognitive.microsoft.com"
    echo "2) West US 2 - westus2.api.cognitive.microsoft.com"
    echo "3) East US - eastus.api.cognitive.microsoft.com"
    echo "4) East US 2 - eastus2.api.cognitive.microsoft.com"
    echo "5) West Central US - westcentralus.api.cognitive.microsoft.com"
    echo "6) South Central US - southcentralus.api.cognitive.microsoft.com"
    echo "7) West Europe - westeurope.api.cognitive.microsoft.com"
    echo "8) North Europe - northeurope.api.cognitive.microsoft.com"
    echo "9) Southeast Asia - southeastasia.api.cognitive.microsoft.com"
    echo "10) East Asia - eastasia.api.cognitive.microsoft.com"
    echo "11) Australia East - australiaeast.api.cognitive.microsoft.com"
    echo "12) Brazil South - brazilsouth.api.cognitive.microsoft.com"

    read -p "Select your Face API region: " answer

    # all to lower case
    answer=$(echo $answer | awk '{print tolower($0)}')

    # check and act on given answer
    case $answer in
        "1")  ProceedConfigureFaceAPI "westus" ;;
        "2")  ProceedConfigureFaceAPI "westus2" ;;
        "3")  ProceedConfigureFaceAPI "eastus" ;;
        "4")  ProceedConfigureFaceAPI "eastus2" ;;
        "5")  ProceedConfigureFaceAPI "westcentralus" ;;
        "6")  ProceedConfigureFaceAPI "southcentralus" ;;
        "7")  ProceedConfigureFaceAPI "westeurope" ;;
        "8")  ProceedConfigureFaceAPI "northeurope" ;;
        "9")  ProceedConfigureFaceAPI "southeastasia" ;;
        "10") ProceedConfigureFaceAPI "eastasia" ;;
        "11") ProceedConfigureFaceAPI "australiaeast" ;;
        "12") ProceedConfigureFaceAPI "brazilsouth" ;;
        *)    echo "Please select a valid region" ; ConfigureFaceAPI ;;
    esac
}

ProceedConfigureFaceAPI ()
{
    faceApiName=$resourceGroupName"-fac"
    echo "Face API name: " $faceApiName

    # get cognitive services face api key 1
    faceApiKeys="$(az cognitiveservices account keys list --resource-group $resourceGroupName --name $faceApiName)"
    # echo "Face API keys: " $faceApiKeys

    tfaceApiKey1="$(jq '.key1' <<< "$faceApiKeys")"
    faceApiKey1=$(echo "$tfaceApiKey1" | sed -e 's/^"//' -e 's/"$//')
    echo "Face API key1: " $faceApiKey1

    # create person group
    curl -v -X PUT "https://$1.api.cognitive.microsoft.com/face/v1.0/persongroups/people_group" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: $faceApiKey1" --data-ascii "{'name':'People Group','userData':'People group'}" 

    # create face list
    curl -v -X PUT "https://$1.api.cognitive.microsoft.com/face/v1.0/facelists/face_list" -H "Content-Type: application/json" -H "Ocp-Apim-Subscription-Key: $faceApiKey1" --data-ascii "{'name':'Face List','userData':'Face list'}" 
    
    # face api region
    faceApiRegion=$1

    echo "Face API configured successfully!"
}

ConfigureFunctionApp ()
{
    echo "* Configuring: Function App>>"

    functionAppName=$resourceGroupName"-fun"
    echo "Function App name: " $functionAppName

    # configure face api key
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings Face_API_Subscription_Key=$faceApiKey1

    # configure person group
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings Face_API_PersonGroupId=people_group

    # configure face list 
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings Face_API_FaceList=face_list

    # configure face api region
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings Face_API_Zone=$faceApiRegion

    # configure database uri
    databaseUri="https://$databaseAccountName.documents.azure.com:443/"
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings CosmosDB_URI=$databaseUri
    
    # configure database key
    databaseKeys="$(az cosmosdb list-keys --name $databaseAccountName --resource-group $resourceGroupName)"
    tdatabaseKey1="$(jq '.primaryMasterKey' <<< "$databaseKeys")"
    databaseKey1=$(echo "$tdatabaseKey1" | sed -e 's/^"//' -e 's/"$//')
    echo "Database key1: " $databaseKey1
    
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings CosmosDB_AuthKey=$databaseKey1

    # configure database id
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings CosmosDB_DatabaseId=AzureIntelligentHack

    # configure database person collection
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings CosmosDB_PersonCollection=Person

    # configure crytography key
    read -p "Paste your cryptography key: " cryptographyKey
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings Cryptography_Key=$cryptographyKey

    # configure app center android
    read -p "Paste your App Center Id for Android: " appCenterAndroid
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings AppCenterID_Android=$appCenterAndroid

    # configure app center ios
    read -p "Paste your App Center Id for iOS: " appCenteriOS
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings AppCenterID_iOS=$appCenteriOS
    
    # configure sender mail api key
    read -p "Paste your SendGrid API Key: " sendGridAPIKey
    az functionapp config appsettings set --resource-group $resourceGroupName --name $functionAppName --settings SendGrid_API_Key=$sendGridAPIKey
 
    
    echo "Function App configured successfully!"
}

ConfigureAzureSubscription
