using System.Configuration;

namespace HouseofCat.Gremlins.Demo.Models
{
    public static class GlobalVariables
    {
        public static string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["Gremlins"].ConnectionString;
    }
}
