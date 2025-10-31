using System.Text.Json.Serialization;

namespace SrvBack.Models
{
    public class Pais
    {
        public Id Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }

    public class Id
    {
        [JsonPropertyName("M49")]
        public int M49 { get; set; }

        [JsonPropertyName("ISO-ALPHA-2")]
        public string ISOAlpha2 { get; set; }

        [JsonPropertyName("ISO-ALPHA-3")]
        public string ISOAlpha3 { get; set; }
    }

}
