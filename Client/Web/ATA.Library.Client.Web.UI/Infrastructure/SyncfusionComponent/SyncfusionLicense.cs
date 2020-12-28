namespace ATA.Library.Client.Web.UI.Infrastructure.SyncfusionComponent
{
    public static class SyncfusionLicense
    {
        public static void Register()
        {
            string licenseKey = "Mzc0MjE5QDMxMzgyZTM0MmUzMFJwTWZWclFsV1FvaU4reUJyZ2xhcHFvc0o1eEQxTTlJYkE2STExbWVOT0k9";

            //Register Syncfusion license 
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);
        }
    }
}