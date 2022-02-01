using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Entities;

namespace Catalog.Repositories
{

    public class InMemItemsRepository : IItemsRepository
    {
        #region Private fields

        private readonly List<Item> _items = new()
        {
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Potion",
                Price = 9,
                CreatedDate = DateTimeOffset.UtcNow
            },
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Iron Sword",
                Price = 20,
                CreatedDate = DateTimeOffset.UtcNow
            },
            new Item
            {
                Id = Guid.NewGuid(),
                Name = "Bronze Shield",
                Price = 18,
                CreatedDate = DateTimeOffset.UtcNow
            },
        };

        #endregion

        public IEnumerable<Item> GetItems()
        {
            return _items;
        }

        public Item GetItem(Guid id)
        {
            return _items.FirstOrDefault(item => item.Id == id);
        }

        public void CreateItem(Item item)
        {
            _items.Add(item);
        }

        public void UpdateItem(Item item)
        {
            var index = GetIndexOf(item.Id);
            _items[index] = item;
        }

        public void DeleteItem(Guid id)
        {
            var index = GetIndexOf(id);
            _items.RemoveAt(index);
        }

        #region Private fields

        private int GetIndexOf(Guid id)
        {
            return _items.FindIndex(existingItem => existingItem.Id == id);
        }

        #endregion
    }
}
