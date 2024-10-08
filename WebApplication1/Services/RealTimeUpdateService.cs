﻿
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using WebApplication1.Controllers;
using WebApplication1.DB.Models;
using WebApplication1.JsonModels;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class RealTimeUpdateService : BackgroundService
    {
        private readonly ILogger<CryptocurrencyController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        public RealTimeUpdateService(
            IServiceProvider serviceProvider,
            IConfiguration config,
            ILogger<CryptocurrencyController> logger
            )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    _logger.LogInformation("Crypto WebSocket Updater is running.");

                    var currencyService = scope.ServiceProvider.GetService<ICurrencyService>();
                    var webSocketClient = new ClientWebSocket();
                    var uri = new Uri("wss://ws.coinapi.io/v1/");
                    await webSocketClient.ConnectAsync(uri, CancellationToken.None);

                    var currencies = currencyService.GetAllCurrencies().ToList();
                    var assetIds = new List<string>();
                    foreach (var item in currencies)
                    {
                        assetIds.Add(item.AssetId+"/USD");
                    }

                    var subscribeMessage = new
                    {
                        type = "subscribe",
                        apikey = _config.GetSection("CoinAPI").GetValue<string>("API_Key"),
                        heartbeat = false,
                        subscribe_data_type = new[] { "exrate" },
                        subscribe_filter_asset_id = assetIds,
                    };

                    await webSocketClient.SendAsync(
                        new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(subscribeMessage))),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );

                    var buffer = new byte[1024 * 4];
                    while (webSocketClient.State == WebSocketState.Open && !stoppingToken.IsCancellationRequested)
                    {
                        var messageBuilder = new StringBuilder();
                        WebSocketReceiveResult result;
                        do
                        {
                            result = await webSocketClient.ReceiveAsync(new ArraySegment<byte>(buffer), stoppingToken);
                            var chunk = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            messageBuilder.Append(chunk);
                        }
                        while (!result.EndOfMessage);

                        var jsonString = messageBuilder.ToString();

                        var currencyInfo = JsonConvert.DeserializeObject<JsonCurrencyInfo>(jsonString);
                        if (currencyInfo != null)
                        {
                            var currency = currencyService.GetCurrency(currencyInfo.CurrencyAssetId);
                            currencyService.UpdateCurrency(new Currency()
                            {
                                AssetId = currencyInfo.CurrencyAssetId,
                                Name = currency.Name,
                                Rate = currencyInfo.Rate,
                                DateTime = currencyInfo.DateTime
                            });
                        }

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            _logger.LogInformation("WebSocket connection closed.");
                            await webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", stoppingToken);
                        }

                        Thread.Sleep(60000);
                    }

                    _logger.LogInformation("Crypto WebSocket Updater is stopping.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"The error occurs in RealTimeUpdateService.ExecuteAsync method: {ex.Message}");
                }
            }
        }
    }
}
