using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Commerce.Application;
using Commerce.Domain;
using Commerce.Storage.Repositories;
using Moq;
using Xunit;

namespace Test.Level1.Commerce
{
    [Trait("Category", "L1")]
    public class ServiceTests
    {
        private readonly BasketService basketService;
        private readonly RegionInfo SE = new RegionInfo("SE");

        public ServiceTests()
        {
            var databaseOptions = new ConfigurableDatabaseOptions();

            var repository = new Repository(databaseOptions.Options);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            productServiceMock
                .Setup(productService => productService.GetById(42))
                .ReturnsAsync(new Product(42, "Product with difficult characaters: Ţ��", new Money(13.76, SE)));

            basketService = new BasketService(productServiceMock.Object, repository);
        }

        // Write unit tests here, using field basketService
    }
}
