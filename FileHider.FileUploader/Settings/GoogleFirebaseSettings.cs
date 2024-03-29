namespace FileHider.Web.MVC.Settings
{
    public class GoogleFirebaseSettings
    {
        public const string Section = "Firebase";
        public required string ServiceAccountFilePath { get; set; }
        public required string BucketName { get; set; }

    }
}
