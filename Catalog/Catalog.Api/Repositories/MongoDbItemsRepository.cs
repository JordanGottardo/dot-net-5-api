using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        #region Private Fields

        private const string DatabaseName = "catalog";
        private const string CollectionName = "items";
        private readonly IMongoCollection<Item> _itemsCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        #endregion

        #region Initialization

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(DatabaseName);
            _itemsCollection = database.GetCollection<Item>(CollectionName);
        }

        #endregion

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await _itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            return await _itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateItemAsync(Item item)
        {
            await _itemsCollection.InsertOneAsync(item);
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await _itemsCollection.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, id);
            await _itemsCollection.DeleteManyAsync(filter);
        }
    }
}