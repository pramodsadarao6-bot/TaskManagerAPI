using Xunit;
using Moq;
using TaskManagerAPI.Controllers;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Models;
using TaskManagerAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Tests
{
    public class TasksControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfTasks()
        {
            // ARRANGE
            var mockRepo = new Mock<ITaskRepository>();

            var fakeTasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Test Task 1", IsCompleted = false, CreatedAt = DateTime.UtcNow },
                new TaskItem { Id = 2, Title = "Test Task 2", IsCompleted = true, CreatedAt = DateTime.UtcNow }
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(fakeTasks);

            var controller = new TasksController(mockRepo.Object);

            // ACT
            var result = await controller.GetAll();

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<TaskResponseDto>>(okResult.Value);

            Assert.Equal(2, returnedTasks.Count());
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // ARRANGE
            var mockRepo = new Mock<ITaskRepository>();

            mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((TaskItem?)null);

            var controller = new TasksController(mockRepo.Object);

            // ACT
            var result = await controller.GetById(99);

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedResult_WithNewTask()
        {
            // ARRANGE
            var mockRepo = new Mock<ITaskRepository>();

            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<TaskItem>()))
                .ReturnsAsync((TaskItem t) =>
                {
                    t.Id = 1;
                    return t;
                });

            var controller = new TasksController(mockRepo.Object);

            var createDto = new TaskCreateDto
            {
                Title = "New Task",
                IsCompleted = false
            };

            // ACT
            var result = await controller.Create(createDto);

            // ASSERT
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedTask = Assert.IsType<TaskResponseDto>(createdResult.Value);

            Assert.Equal("New Task", returnedTask.Title);
        }
    }
}