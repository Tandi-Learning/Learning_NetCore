// See https://aka.ms/new-console-template for more information
using console_client_signalr;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

string baseUri = "https://localhost:7241";

HttpClient client = new HttpClient();
client.BaseAddress = new Uri(baseUri);

var auctionsResult = await client.GetStringAsync("api/auction/auctions");
IEnumerable<Auction>? auctions = JsonSerializer.Deserialize<IEnumerable<Auction>>(auctionsResult);

var connection = new HubConnectionBuilder()
    .WithUrl($"{baseUri}/auctionhub", options =>
    {
        options.Transports = HttpTransportType.WebSockets;
        options.SkipNegotiation = true;
    })
    .Build();

connection.On("ReceiveNewBid", (AuctionNotify auctionNotify) =>
{
    var auction = auctions.Single(a => a.Id == auctionNotify.AuctionId);
    auction.CurrentBid = auctionNotify.NewBid;
    //Console.WriteLine("New Bid:");
    Console.WriteLine($"{auction.Id,-3} {auction.ItemName,-20} {auction.CurrentBid,10}");
});

await connection.StartAsync();

void ShowAuction(IEnumerable<Auction>? auctions)
{
    Console.WriteLine("0 - Quit");
    foreach (var auction in auctions)
    {
        Console.WriteLine(auction.Id + " - " + auction.ItemName + " " + auction.CurrentBid);
    }
}

async void SubmitBid(string? id, string? newBid)
{
    await connection.InvokeAsync("NotifyNewBid",
        new { AuctionId = int.Parse(id), NewBid = int.Parse(newBid) });
}


while (true)
{
    ShowAuction(auctions);
    Console.Write("Auction Id: ");
    string? id = Console.ReadLine();
    Console.Write("New Bid: ");
    string? newBid = Console.ReadLine();
    if (id == "0") return;
    else
        SubmitBid(id, newBid);
}

await connection.StopAsync();