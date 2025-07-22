using System;
using System.IO;
using System.Threading.Tasks;
using Alpaca.Markets;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AlpacaExample
{
    internal static class Program
    {


        public static async Task Main(string[] args)
        {

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // usually project root
                .AddJsonFile("Config/appsettings.json", optional: false)
                .Build();

            var tradingConfig = config.GetSection("Alpaca").Get<TradingConfig>();

            if (string.IsNullOrWhiteSpace(tradingConfig?.ApiKey) || string.IsNullOrWhiteSpace(tradingConfig?.SecretKey))
            {
                Console.WriteLine("Missing API credentials in configuration.");
                return;
            }

            Console.WriteLine($"API Key Loaded: {tradingConfig.ApiKey}");

            // First, open the API connection
            var client = Environments.Paper.GetAlpacaTradingClient(new SecretKey(
                tradingConfig.ApiKey, tradingConfig.SecretKey));

            // Get our account information.
            var account = await client.GetAccountAsync();

            // Check if our account is restricted from trading.
            if (account.IsTradingBlocked)
            {
                Console.WriteLine("Account is currently restricted from trading.");
            }

            Console.WriteLine(account.BuyingPower + " is available as buying power.");

            Console.Read();
        }
    }
}