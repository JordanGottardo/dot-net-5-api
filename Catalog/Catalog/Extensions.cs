using Catalog.Dtos;
using Catalog.Entities;

namespace Catalog
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new()
            {
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate,
                Id = item.Id
            };
        }
    }
}