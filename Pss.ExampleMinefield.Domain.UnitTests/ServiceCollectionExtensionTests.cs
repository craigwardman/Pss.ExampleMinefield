using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Pss.ExampleMinefield.Domain.UnitTests
{
    [TestFixture]
    public class ServiceCollectionExtensionTests
    {
        [Test]
        public void AddDecisionDataStoreProvider_WhenCalled_ConfiguresValidContainer()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDomainServices();

            serviceCollection.Invoking(sc => sc.BuildServiceProvider(true)).Should().NotThrow();
        }
    }
}