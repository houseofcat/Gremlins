using System.Configuration;

namespace Gremlins.Demo.Models
{
    public static class GlobalVariables
    {
        public static string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["Gremlins"].ConnectionString;
    }
}
