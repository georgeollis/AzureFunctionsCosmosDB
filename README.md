# Azure Functions shopping list API using Cosmos DB.

An example of using Cosmos DB and Azure Functions to create a serverless shopping list API. 

Functions are using the HTTP trigger. The functions are the following.
- GetShoppingCartItems
- GetShoppingCartItemById
- CreateShoppingCartItem
- PutShoppingCartItem
- DeleteShoppingCartItem

The functions are written in C# for this example.

<img src="image/overview.png"
     alt="overview"
     style="float: middle; margin-right: 10px;"
     width="600" 
/>

The application needs an environment variable generated called connectionString.
- This environment variable should be the access key connection strings provided by your CosmosDB resource. 
- This should be stored in Azure Key Vault and referenced within the application settings.

Blogs related to Azure Functions and Cosmos DB.
  - https://www.georgeollis.com/using-secrets-from-azure-key-vault-for-your-azure-functions/ 