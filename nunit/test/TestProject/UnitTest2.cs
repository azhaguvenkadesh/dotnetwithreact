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
    public class LoanApplicationControllerTests
    {
        private LoanApplicationController _LoanApplicationController;
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
            _context.LoanApplications.AddRange(new List<LoanApplication>
            {
new LoanApplication { LoanApplicationID = 1,userId="101",userName="user1", RequestedAmount=200000, SubmissionDate = DateTime.Parse("2023-04-30"),EmploymentStatus="self",Income=12000M,CreditScore=100,LoanStatus=0,LoanType="Loan1"},
new LoanApplication { LoanApplicationID = 2,userId="102",userName="user2", RequestedAmount=400000, SubmissionDate = DateTime.Parse("2023-01-15"),EmploymentStatus="working",Income=44000M,CreditScore=10,LoanStatus=0,LoanType="Loan2"},
new LoanApplication { LoanApplicationID = 3,userId="101",userName="user3", RequestedAmount=600000, SubmissionDate = DateTime.Parse("2023-02-03"),EmploymentStatus="Business",Income=45000M,CreditScore=200,LoanStatus=0,LoanType="Loan3"}
            });
            _context.SaveChanges();

            _LoanApplicationController = new LoanApplicationController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void LoanApplicationClassExists()
        {
            // Arrange
            Type LoanApplicationType = typeof(LoanApplication);

            // Act & Assert
            Assert.IsNotNull(LoanApplicationType, "LoanApplication class not found.");
        }
        [Test]
        public void LoanApplication_Properties_userId_ReturnExpectedDataTypes()
        {
            // Arrange
            LoanApplication LoanApplication = new LoanApplication();
            PropertyInfo propertyInfo = LoanApplication.GetType().GetProperty("userId");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "userId property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "userId property type is not string.");
        }
[Test]
        public void LoanApplication_Properties_SubmissionDate_ReturnExpectedDataTypes()
        {
            // Arrange
            LoanApplication LoanApplication = new LoanApplication();
            PropertyInfo propertyInfo = LoanApplication.GetType().GetProperty("SubmissionDate");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "SubmissionDate property not found.");
            Assert.AreEqual(typeof(DateTime), propertyInfo.PropertyType, "SubmissionDate property type is not string.");
        }

        [Test]
        public async Task GetAllLoanApplications_ReturnsOkResult()
        {
            // Act
            var result = await _LoanApplicationController.GetAllLoanApplications();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllLoanApplications_ReturnsAllLoanApplications()
        {
            // Act
            var result = await _LoanApplicationController.GetAllLoanApplications();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<LoanApplication>>(okResult.Value);
            var LoanApplications = okResult.Value as IEnumerable<LoanApplication>;

            var LoanApplicationCount = LoanApplications.Count();
            Assert.AreEqual(3, LoanApplicationCount); // Assuming you have 3 LoanApplications in the seeded data
        }


        [Test]
        public async Task AddLoanApplication_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newLoanApplication = new LoanApplication
            {
userId="101",userName="user1", RequestedAmount=200000, SubmissionDate = DateTime.Parse("2023-04-30"),EmploymentStatus="self",Income=12000M,CreditScore=100,LoanStatus=0,LoanType="Loan1" };

            // Act
            var result = await _LoanApplicationController.AddLoanApplication(newLoanApplication);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteLoanApplication_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new LoanApplicationsController(context);

                // Act
                var result = await _LoanApplicationController.DeleteLoanApplication(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteLoanApplication_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _LoanApplicationController.DeleteLoanApplication(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid LoanApplication id", result.Value);
        }
    }
}
