﻿
namespace WebApplication1.DB.Models
{
    public class Currency
    {
        public required string AssetId { get; set; }
        public string? Name { get; set; }
        public double? Rate { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
