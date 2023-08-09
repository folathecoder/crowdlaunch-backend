// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using MARKETPLACEAPI.Controllers;
// using MARKETPLACEAPI.Models;
// using MARKETPLACEAPI.Services;

// using Xunit;


// namespace MARKETPLACEAPI.Tests;

// public class AuthControllerTests 
// {
//     [Fact]
//     public void Get_ReturnsListOfProjects()
//     {
//         // Arrange
//         var controller = new AuthController(
//           new UserService(
//             new MongoDbContext("mongodb://localhost:27017", "test")
//           ),
//           new AuthService()
//         );

//         // Act
//         var result = controller.Get();

//         // Assert
//         var model = Assert.IsAssignableFrom<IEnumerable<Project>>(
//             result);
//         Assert.Equal(2, model.Count());
//     }
// }