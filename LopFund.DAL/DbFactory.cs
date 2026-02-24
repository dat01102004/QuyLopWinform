using System.Configuration;

namespace LopFund.DAL
{
    public static class DbFactory
    {
        public static string ConnStr => ConfigurationManager.ConnectionStrings["LopFundConn"].ConnectionString;
    }
}
