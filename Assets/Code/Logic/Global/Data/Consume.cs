using Newtonsoft.Json;

namespace Static
{
  public class Consume
  {
    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
    public int Amount { get; set; }

    public Consume() { }

    public Consume(string type, int ammount)
    {
      Type = type;
      Amount = ammount;
    }

    
  }
}