using System;

namespace IntelligentHack.Functions
{
    public class Settings
    {
        public static string AzureWebJobsStorage = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        public static string FaceAPIKey = Environment.GetEnvironmentVariable("Face_API_Subscription_Key");
        public static string PersonGroupId = Environment.GetEnvironmentVariable("Face_API_PersonGroupId");
        public static string Zone = Environment.GetEnvironmentVariable("Face_API_Zone");
        public static string FaceListId = Environment.GetEnvironmentVariable("Face_API_FaceList");

        public static string DocumentDB = Environment.GetEnvironmentVariable("CosmosDB_URI");
        public static string DocumentDBAuthKey = Environment.GetEnvironmentVariable("CosmosDB_AuthKey");
        public static string DatabaseId = Environment.GetEnvironmentVariable("CosmosDB_DatabaseId");
        public static string PersonCollectionId = Environment.GetEnvironmentVariable("CosmosDB_PersonCollection");

        public static string NotificationAccessSignature = Environment.GetEnvironmentVariable("NotificationHub_Access_Signature");
        public static string NotificationHubName = Environment.GetEnvironmentVariable("NotificationHub_Name");

        public static string CryptographyKey = Environment.GetEnvironmentVariable("Cryptography_Key");

        public static string AppCenterID_Android = Environment.GetEnvironmentVariable("AppCenterID_Android");
        public static string AppCenterID_iOS = Environment.GetEnvironmentVariable("AppCenterID_iOS");

        public static string ImageStorageUrl = Environment.GetEnvironmentVariable("ImageStorageUrl");

        public static string SendGridAPIKey = Environment.GetEnvironmentVariable("SendGrid_API_Key");
    }
}