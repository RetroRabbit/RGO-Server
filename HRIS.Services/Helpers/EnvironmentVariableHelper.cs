using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Services.Helpers
{
    public static class EnvironmentVariableHelper
    {

        public static string CONNECTION_STRINGS_DEFAULT => Environment.GetEnvironmentVariable("ConnectionStrings__Default");
        public static string AUTH_MANAGEMENT_ISSUER => Environment.GetEnvironmentVariable("AuthManagement__Issuer");
        public static string AUTH_MANAGEMENT_CLIENT_SECRET => Environment.GetEnvironmentVariable("AuthManagement__ClientSecret");
        public static string AUTH_MANAGEMENT_CLIENT_ID => Environment.GetEnvironmentVariable("AuthManagement__ClientId");
        public static string AUTH_MANAGEMENT_AUDIENCE => Environment.GetEnvironmentVariable("AuthManagement__Audience");
        public static string SMTP_HOST => Environment.GetEnvironmentVariable("SMTP__Host");
        public static string SMTP_NAME => Environment.GetEnvironmentVariable("SMTP__Name");
        public static string SMTP_MAIL => Environment.GetEnvironmentVariable("SMTP__Mail");
        public static string SMTP_PASSWORD => Environment.GetEnvironmentVariable("SMTP__Password");
        public static string SMTP_PORT => Environment.GetEnvironmentVariable("SMTP__Port");
    }
}
