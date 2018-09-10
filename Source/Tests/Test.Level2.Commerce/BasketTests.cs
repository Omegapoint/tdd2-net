using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Commerce.Host.DataContracts;
using Microsoft.Rest.Azure;
using Xunit;

namespace Test.Level2.Commerce
{
    [Trait("Category", "L2")]
    public class BasketTests
    {
        private readonly HttpClient client = new ConfigurableHttpClient();

        // Write your tests here.  You can look at class ProductTests for
        // how you can use the client field above.
    }
}
