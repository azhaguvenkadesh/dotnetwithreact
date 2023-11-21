using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Controllers;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace dotnetapp.Tests
{
    [TestFixture]
    public class LoanControllerTests
    {
        private LoanController _LoanController;
        private LoanApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<LoanApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new LoanApplicationDbContext(options);
            _context.Database.EnsureCreated(); // Create the database

            // Seed the database with sample data
            _context.Loans.AddRange(new List<Loan>
            {
                new Loan { LoanID = 1, LoanType = "Loan 1", Description = "Loan Description1", InterestRate = 7,MaximumAmount=12000 },
                new Loan { LoanID = 2, LoanType = "Loan 2", Description = "Loan Description2", InterestRate = 8,MaximumAmount=24000 },
                new Loan { LoanID = 3, LoanType = "Loan 3", Description = "Loan Description3", InterestRate = 9,MaximumAmount=32000 }
            });
            _context.SaveChanges();

            _LoanController = new LoanController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void LoanClassExists()
        {
            // Arrange
            Type LoanType = typeof(Loan);

            // Act & Assert
            Assert.IsNotNull(LoanType, "Loan class not found.");
        }
         [Test]
        public void LoginModelClassExists()
        {
            // Arrange
            Type LoginModelType = typeof(LoginModel);

            // Act & Assert
            Assert.IsNotNull(LoginModelType, "LoginModel class not found.");
        }
         [Test]
        public void RegisterModelClassExists()
        {
            // Arrange
            Type RegisterModelType = typeof(RegisterModel);

            // Act & Assert
            Assert.IsNotNull(RegisterModelType, "RegisterModel class not found.");
        }
        [Test]
public void AccountController_Exists()
{
    // Arrange
    var assembly = Assembly.Load("dotnetwebapi"); // Replace with the actual assembly name

    // Get the namespace and controller name
    string controllerName = "Loan";
    string controllerNamespace = "dotnetapp.Controllers"; // Replace with your controller's namespace

    // Construct the full controller type name
    string controllerTypeName = controllerNamespace + "." + controllerName + "Controller";

    // Act
    Type controllerType = assembly.GetType(controllerTypeName);

    // Assert
    Assert.IsNotNull(controllerType, "Controller not found: " + controllerTypeName);
}
 [Test]
public void LoanApplicationController_Exists()
{
    // Arrange
    var assembly = Assembly.Load("dotnetwebapi"); // Replace with the actual assembly name

    // Get the namespace and controller name
    string controllerName = "LoanApplication";
    string controllerNamespace = "dotnetapp.Controllers"; // Replace with your controller's namespace

    // Construct the full controller type name
    string controllerTypeName = controllerNamespace + "." + controllerName + "Controller";

    // Act
    Type controllerType = assembly.GetType(controllerTypeName);

    // Assert
    Assert.IsNotNull(controllerType, "Controller not found: " + controllerTypeName);
}
[Test]
public void AuthController_Exists()
{
    // Arrange
    var assembly = Assembly.Load("dotnetwebapi"); // Replace with the actual assembly name

    // Get the namespace and controller name
    string controllerName = "Auth";
    string controllerNamespace = "dotnetapp.Controllers"; // Replace with your controller's namespace

    // Construct the full controller type name
    string controllerTypeName = controllerNamespace + "." + controllerName + "Controller";

    // Act
    Type controllerType = assembly.GetType(controllerTypeName);

    // Assert
    Assert.IsNotNull(controllerType, "Controller not found: " + controllerTypeName);
}

        [Test]
        public void Loan_Properties_LoanType_ReturnExpectedDataTypes()
        {
            // Arrange
            Loan Loan = new Loan();
            PropertyInfo propertyInfo = Loan.GetType().GetProperty("LoanType");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "LoanType property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "LoanType property type is not string.");
        }
[Test]
        public void Loan_Properties_Description_ReturnExpectedDataTypes()
        {
            // Arrange
            Loan Loan = new Loan();
            PropertyInfo propertyInfo = Loan.GetType().GetProperty("Description");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "Description property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "Description property type is not string.");
        }

        [Test]
        public async Task GetAllLoans_ReturnsOkResult()
        {
            // Act
            var result = await _LoanController.GetAllLoans();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllLoans_ReturnsAllLoans()
        {
            // Act
            var result = await _LoanController.GetAllLoans();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Loan>>(okResult.Value);
            var Loans = okResult.Value as IEnumerable<Loan>;

            var LoanCount = Loans.Count();
            Assert.AreEqual(3, LoanCount); // Assuming you have 3 Loans in the seeded data
        }


        [Test]
        public async Task AddLoan_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newLoan = new Loan
            {
 LoanType = "Loan New", Description = "Loan Description  New", InterestRate = 7,MaximumAmount=12000
            };

            // Act
            var result = await _LoanController.AddLoan(newLoan);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteLoan_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new LoansController(context);

                // Act
                var result = await _LoanController.DeleteLoan(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteLoan_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _LoanController.DeleteLoan(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Loan id", result.Value);
        }
    }
}
