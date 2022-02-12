using System;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace Catalog.UnitTests
{
    public class ItemsControllerTest
    {
        private readonly Mock<IItemsRepository> _repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> _loggerStub = new();
        private readonly Random _rand = new();

        #region GetItemAsync

        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item) null);
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            var result = await controller.GetItemAsync(Guid.NewGuid());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsItemFound()
        {
            var expectedItem = CreateRandomItem();
            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            var result = await controller.GetItemAsync(Guid.NewGuid());

            result.Value.Should().BeEquivalentTo(
                expectedItem,
                options => options.ComparingByMembers<Item>());
        }

        #endregion

        #region GetItemsAsync

        [Fact]
        public async Task GetItemsAsync_WithExistingItem_ReturnsItemFound()
        {
            var expectedItems = new[]
            {
                CreateRandomItem(), CreateRandomItem(), CreateRandomItem()
            };
            _repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(expectedItems);
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            var items = await controller.GetItemsAsync();

            items.Should().BeEquivalentTo(
                expectedItems,
                options => options.ComparingByMembers<Item>());
        }

        #endregion

        #region CreateItemAsync

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            var itemToCreate = new CreateItemDto
            {
                Name = Guid.NewGuid().ToString(),
                Price = _rand.Next(1000)
            };
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            var result = await controller.CreateItemAsync(itemToCreate);

            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers());
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));

        }

        #endregion

        #region UpdateItemAsync

        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
        {
            var existingItem = CreateRandomItem();
            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);
            var itemToUpdate = new UpdateItemDto
            {
                Name = Guid.NewGuid().ToString(),
                Price = existingItem.Price + 3
            };
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            var result = await controller.UpdateItemAsync(existingItem.Id, itemToUpdate);

            result.Should().BeOfType<NoContentResult>();

        }

        #endregion

        #region DeleteItemAsync

        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            var existingItem = CreateRandomItem();
            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            var result = await controller.DeleteItemAsync(existingItem.Id);

            result.Should().BeOfType<NoContentResult>();

        }

        #endregion

        #region Private fields

        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = _rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }

        #endregion
    }
}
