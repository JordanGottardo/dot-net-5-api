using Catalog.Api.Dtos;
using Catalog.Api.Entities;

namespace Catalog.Api
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