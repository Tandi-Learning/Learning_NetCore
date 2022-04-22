using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MeterReaderWeb.Services;
using Microsoft.Extensions.Logging;

namespace MeterReaderClient
{
    public class ReadingFactory
    {
        //private readonly ILogger _logger;

        //public ReadingFactory(
        //    ILogger logger)
        //{
        //    _logger = logger;
        //}

        public Task<ReadingMessage> GenerateAsync(int customerId)
        {
            var reading = new ReadingMessage
            {
                CustomerId = customerId,
                ReadingTime = Timestamp.FromDateTime(DateTime.UtcNow),
                ReadingValue = new Random().Next(10000)
            };
            return Task.FromResult(reading);
        }
    }
}
