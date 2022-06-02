namespace DecimalDemoProject
{
    using Microsoft.Extensions.Configuration;

    public class DbSettings
    { 
        public string DefaultConnection { get; set; }
    }

    public static class SettingsLoader
    {
        public static IConfigurationSection LoadDbSettings(IConfiguration configuration) =>
            configuration.GetSection("ConnectionStrings");
    }
}
