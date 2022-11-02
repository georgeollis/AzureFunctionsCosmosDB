using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;


namespace ShoppingCartList.Models
{
    internal class ShoppingCartItem : TableEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("created")]
        public DateTime Created { get; set; } = DateTime.Now;
         [JsonProperty("itemName")]
        public string ItemName { get; set; }
         [JsonProperty("collected")]
        public bool Collected { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
    }

    internal class CreateShoppingCartItem
    {
        [JsonProperty("itemName")]
        public string ItemName { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
    }

    internal class UpdateShoppingCartItem
    {
        [JsonProperty("collected")]
        public bool Collected { get; set; }
    }
}