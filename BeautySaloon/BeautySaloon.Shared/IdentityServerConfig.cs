using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.Shared;
public static class IdentityServerConfig
{
    public const string AuthorityDevelopmentHttp = "https://localhost:5001";
    public const string AuthorityProductionHttp = "http://identity";

    public const string AuthorityDevelopmentHttps = "https://localhost:5001";
    public const string AuthorityProductionHttps = "http://identity";
}
