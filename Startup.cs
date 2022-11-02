using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartList;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ShoppingCartList
{
    public class Startup : FunctionsStartup
    {
        private static string connectionString = System.Environment.GetEnvironmentVariable("connectionString");
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(s =>
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(
                        "Invalid connection string environment variable.");
                }
                
                return new CosmosClientBuilder(connectionString).Build();

            });
        }
    }
}


