using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using MeterReaderWeb.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MeterReaderClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private ReadingFactory _readingFactory;
        private MeterReadingService.MeterReadingServiceClient _client = null;

        protected MeterReadingService.MeterReadingServiceClient client
        {
            get
            {
                if (_client == null)
                {
                    var channel = GrpcChannel.ForAddress(_config["Service:ServerUrl"]);
                    _client = new MeterReadingService.MeterReadingServiceClient(channel);
                }
                return _client;
            }
        }

        public Worker(
            ReadingFactory readingFactory,
            IConfiguration config,
            ILogger<Worker> logger)
        {
            _logger = logger;
            _config = config;
            _readingFactory = readingFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var customerId = _config.GetValue<int>("Service:CustomerId");

                var packet = new ReadingPacket
                {
                    Successful = ReadingStatus.Success,
                    Notes = "This is a test"
                };
                //var reading = new ReadingMessage
                //{
                //    CustomerId = customerId,
                //    ReadingValue = 10000,
                //    ReadingTime = Timestamp.FromDateTime(DateTime.UtcNow)
                //};

                for(int x = 0; x < 5; ++x)
                {
                    packet.Readings.Add(await _readingFactory.GenerateAsync(customerId));
                }

                try
                {
                    var result = await client.AddReadingAsync(packet);
                    if (result.Success == ReadingStatus.Success)
                    {
                        _logger.LogInformation("Successfully sent");
                    }
                    else
                    {
                        _logger.LogInformation("Failed to send"); 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                await Task.Delay(_config.GetValue<int>("Service:DelayInterval"), stoppingToken);
            }
        }
    }
}  
