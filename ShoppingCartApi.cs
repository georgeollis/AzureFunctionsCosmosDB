using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingCartList.Models;

namespace ShoppingCartList
{

    public class ShoppingCartApi
    {
        private readonly CosmosClient _cosmosClient;
        private Container documentContainer;

        public ShoppingCartApi(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            documentContainer = _cosmosClient.GetContainer("ShoppingCartItems", "Items");
        }

        [FunctionName("GetShoppingCartItems")]
        public async Task<IActionResult> GetShoppingCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "shoppingcartitem")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting all shopping cart items");
            List<ShoppingCartItem> shoppingCartItems = new();
            var items = documentContainer.GetItemQueryIterator<ShoppingCartItem>();
            while (items.HasMoreResults)
            {
                var response = await items.ReadNextAsync();
                shoppingCartItems.AddRange(response.ToList());
            }
            return new OkObjectResult(shoppingCartItems);
        }

        [FunctionName("GetShoppingCartItemById")]
        public async Task<IActionResult> GetShoppingCartItemById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "shoppingcartitem/{id}/{category}")]
            HttpRequest req, ILogger log, string id, string category)
        {
            log.LogInformation("Getting shopping cart item by id.");
            var item = await documentContainer.ReadItemAsync<ShoppingCartItem>(id, new PartitionKey(category));

            if (item.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(item.Resource);

        }

        [FunctionName("CreateShoppingCartItem")]
        public async Task<IActionResult> CreateShoppingCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "shoppingcartitem")]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating shopping cart item");
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateShoppingCartItem>(requestData);

            var item = new ShoppingCartItem
            {
                ItemName = data.ItemName,
                Category = data.Category,
                PartitionKey = data.Category,
            };

            await documentContainer.CreateItemAsync(item, new PartitionKey(item.Category));

            return new OkObjectResult(item);
        }

        [FunctionName("PutShoppingCartItem")]
        public async Task<IActionResult> PutShoppingCartItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "shoppingcartitem/{id}/{category}")]
            HttpRequest req, ILogger log, string id, string category)
        {
            log.LogInformation("Updating shopping cart item.");

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UpdateShoppingCartItem>(requestData);

            var item = await documentContainer.ReadItemAsync<ShoppingCartItem>(id, new PartitionKey(category));

            if (item.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new NotFoundResult();

            }

            item.Resource.Collected = data.Collected;
            await documentContainer.UpsertItemAsync(item.Resource);
            return new OkObjectResult(item.Resource);


        }

        [FunctionName("DeleteShoppingCartItem")]
        public async Task<IActionResult> DeleteShoppingCartItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "shoppingcartitem/{id}/{category}")]
            HttpRequest req, ILogger log, string id, string category)
        {
            log.LogInformation("Delete shopping cart item");

            await documentContainer.DeleteItemAsync<ShoppingCartItem>(id, new PartitionKey(category));
            return new OkResult();
        }
    }
}
