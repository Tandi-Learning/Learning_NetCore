﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MeterReader.gRPC;

namespace MeterReadingClient
{
  public class ReadingGenerator
  {
    public Task<ReadingMessage> GenerateAsync(int customerId)
    {
      var reading = new ReadingMessage()
      {
        CustomerId = customerId,
        ReadingTime = Timestamp.FromDateTime(DateTime.UtcNow),
        ReadingValue = new Random().Next(10000)
      };

      return Task.FromResult(reading);
    }
  }
}
