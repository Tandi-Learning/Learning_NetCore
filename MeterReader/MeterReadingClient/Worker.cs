using System.Security.Cryptography.X509Certificates;
using Grpc.Core;
using Grpc.Net.Client;
using MeterReader.gRPC;
using static MeterReader.gRPC.MeterReaderService;

namespace MeterReadingClient
{
  public class Worker : BackgroundService
  {
    private readonly ILogger<Worker> _logger;
    private readonly ReadingGenerator _generator;
    private readonly int _customerId;
    private readonly string _serviceUrl;
    private readonly IConfiguration _config;
    private string _token;
    private DateTime _expiration;

    public Worker(ILogger<Worker> logger, ReadingGenerator generator, IConfiguration config)
    {
      _logger = logger;
      _generator = generator;
      _customerId = config.GetValue<int>("CustomerId");
      _serviceUrl = config["ServiceUrl"];
      _config = config;
      _token = "";
      _expiration = DateTime.MinValue;
    }

    bool NeedsLogin()
    {
      return string.IsNullOrEmpty(_token) || _expiration > DateTime.UtcNow;
    }

    async Task<bool> RequestToken()
    {
      try
      {
        var req = new TokenRequest()
        {
          Username = _config["Settings:Username"],
          Password = _config["Settings:Password"]
        };

        var result = await CreateClient().GenerateTokenAsync(req);

        if (result.Success)
        {
          _token = result.Token;
          _expiration = result.Expiration.ToDateTime();
          return true;
        }

      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
      }
      return false;
    }

    MeterReaderServiceClient CreateClient()
    {
      var certificate = new X509Certificate2(
        _config["Settings:Certificate:Name"],
        _config["Settings:Certificate:Password"]
        );

      var handler = new HttpClientHandler();
      handler.ClientCertificates.Add(certificate);

      var httpClient = new HttpClient(handler);

      var options = new GrpcChannelOptions()
      {
        HttpClient = httpClient
      };

      var channel = GrpcChannel.ForAddress(_serviceUrl, options);
      return new MeterReaderServiceClient(channel);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          //if (!NeedsLogin() || await RequestToken())
          //{

            // Call the gRPC Service


            //var packet = new ReadingPacket()
            //{
            //  Successful = ReadingStatus.Success
            //};

            //var headers = new Metadata();
            //headers.Add("Authorization", $"Bearer {_token}");

            var stream = CreateClient().AddReadingStream(); // JWT used "headers"

            for (var x = 0; x < 5; ++x)
            {
              var reading = await _generator.GenerateAsync(_customerId);
              //packet.Readings.Add(reading);
              await stream.RequestStream.WriteAsync(reading);
              await Task.Delay(500);
            }

            //var status = client.AddReading(packet);
            //if (status.Status == ReadingStatus.Success)
            //{
            //  _logger.LogInformation("Successfully called GRPC");
            //}
            //else
            //{
            //  _logger.LogError("Failed to call GRPC");
            //}

            await stream.RequestStream.CompleteAsync();

            //var result = await stream.ResponseAsync;

            while (await stream.ResponseStream.MoveNext(new CancellationToken()))
            {
              _logger.LogWarning($"From Server: {stream.ResponseStream.Current.Message}");
            }

            _logger.LogInformation("Finished calling GRPC");
          //}
          //else
          //{
          //  _logger.LogInformation("Failed to get JWT Token");
          //}
        } 
        catch (RpcException rex)
        {
          _logger.LogError(rex.Message);
        }

        await Task.Delay(5000, stoppingToken);
      }
    }
  }
}