using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Parameters
{
    public class HttpRequestParameter
    {
        public string HttpMethod { get; set; }
        public string BaseUrl { get; set; } = TestConstants.Localhost;
        public string RelativeResourceUrl { get; set; }
        public string CookieUserId { get; set; }
        public string CookieEmail { get; set; }
    }
}
