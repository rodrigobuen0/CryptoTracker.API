using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CryptoTracker.API.Models
{

    public partial class AssetsPrices
    {
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("data")]
        public List<AssetsData> Data { get; set; }
    }

    public class Status
    {
        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("error_code")]
        public long ErrorCode { get; set; }

        [JsonProperty("error_message")]
        public object ErrorMessage { get; set; }

        [JsonProperty("elapsed")]
        public long Elapsed { get; set; }

        [JsonProperty("credit_count")]
        public long CreditCount { get; set; }

        [JsonProperty("notice")]
        public object Notice { get; set; }

        [JsonProperty("total_count")]
        public long TotalCount { get; set; }
    }

    public class AssetsData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("num_market_pairs")]
        public long NumMarketPairs { get; set; }

        [JsonProperty("date_added")]
        public DateTimeOffset DateAdded { get; set; }

        [JsonProperty("infinite_supply")]
        public bool InfiniteSupply { get; set; }

        [JsonProperty("platform")]
        public Platform Platform { get; set; }

        [JsonProperty("cmc_rank")]
        public long CmcRank { get; set; }

        [JsonProperty("self_reported_circulating_supply")]
        public double? SelfReportedCirculatingSupply { get; set; }

        [JsonProperty("self_reported_market_cap")]
        public double? SelfReportedMarketCap { get; set; }

        [JsonProperty("tvl_ratio")]
        public double? TvlRatio { get; set; }

        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }

        [JsonProperty("quote")]
        public Quote Quote { get; set; }
    }

    public class Quote
    {
        [JsonProperty("USD")]
        public Usd Usd { get; set; }
    }

    public partial class Usd
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("volume_24h")]
        public double Volume24H { get; set; }

        [JsonProperty("volume_change_24h")]
        public double VolumeChange24H { get; set; }

        [JsonProperty("percent_change_1h")]
        public double PercentChange1H { get; set; }

        [JsonProperty("percent_change_24h")]
        public double PercentChange24H { get; set; }

        [JsonProperty("percent_change_7d")]
        public double PercentChange7D { get; set; }

        [JsonProperty("percent_change_30d")]
        public double PercentChange30D { get; set; }

        [JsonProperty("percent_change_60d")]
        public double PercentChange60D { get; set; }

        [JsonProperty("percent_change_90d")]
        public double PercentChange90D { get; set; }

        [JsonProperty("market_cap")]
        public double MarketCap { get; set; }

        [JsonProperty("market_cap_dominance")]
        public double MarketCapDominance { get; set; }

        [JsonProperty("fully_diluted_market_cap")]
        public double FullyDilutedMarketCap { get; set; }

        [JsonProperty("tvl")]
        public double? Tvl { get; set; }

        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }
    }
    public class Platform
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("token_address")]
        public string TokenAddress { get; set; }
    }
}
