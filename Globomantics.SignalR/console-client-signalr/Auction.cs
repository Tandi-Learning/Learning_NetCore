using System.Text.Json.Serialization;

namespace console_client_signalr;

public class Auction
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("itemName")]
    public string ItemName { get; set; } = "";
    [JsonPropertyName("currentBid")]
    public int CurrentBid { get; set; }
}


