using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
namespace TestProject1
{
    public class Tests
    {
        [Test]
        public void AddRestaurant_ValidInput_ShouldAddRestaurant()
        {
            // Arrange
            ReservationManager manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 10);

            // Assert
            Assert.AreEqual(1, manager.Restaurants.Count);
            Assert.AreEqual("A", manager.Restaurants[0].Name);
            Assert.AreEqual(10, manager.Restaurants[0].Tables.Length);
        }


        [Test]
        public void BookTable_ValidBooking_ShouldReturnTrue()
        {
            // Arrange
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("A", 10);

            // Act
            bool result = manager.BookTable("A", new DateTime(2023, 12, 25), 3);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void BookTable_InvalidTableNumber_ShouldThrowException()
        {
            // Arrange
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("A", 10);

            // Act and Assert
            Assert.Throws<Exception>(() => manager.BookTable("A", new DateTime(2023, 12, 25), 10));
        }

        [Test]
        public void BookTable_RestaurantNotFound_ShouldThrowException()
        {
            // Arrange
            ReservationManager manager = new ReservationManager();

            // Act and Assert
            Assert.Throws<Exception>(() => manager.BookTable("A", new DateTime(2023, 12, 25), 3));
        }

        [Test]
        public void CountAvailableTables_ValidInput_ShouldReturnCorrectCount()
        {
            // Arrange
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("A", 5);

            // Act
            int result = manager.CountAvailableTablesForRestaurantClassAndDateTimeMethod(manager.Restaurants[0], DateTime.Now);

            // Assert
            Assert.AreEqual(5, result);
        }

        [Test]
        public void SortRestaurantsByAvailabilityForUsers_ValidInput_ShouldSortCorrectly()
        {
            // Arrange
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("A", 5);
            manager.AddRestaurant("B", 10);

            // Act
            manager.SortRestaurantsByAvailabilityForUsersMethod(DateTime.Now);

            // Assert
            Assert.AreEqual("B", manager.Restaurants[0].Name);
            Assert.AreEqual("A", manager.Restaurants[1].Name);
        }
    }

    [TestFixture]
    public class RestaurantTableTests
    {
        [Test]
        public void Book_ValidInput_ShouldReturnTrue()
        {
            // Arrange
            RestaurantTable table = new RestaurantTable();

            // Act
            bool result = table.Book(DateTime.Now);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Book_AlreadyBookedDate_ShouldReturnFalse()
        {
            // Arrange
            RestaurantTable table = new RestaurantTable();
            DateTime currentDate = DateTime.Now;
            table.Book(currentDate);

            // Act
            bool result = table.Book(currentDate);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsBooked_DateBooked_ShouldReturnTrue()
        {
            // Arrange
            RestaurantTable table = new RestaurantTable();
            DateTime currentDate = DateTime.Now;
            table.Book(currentDate);

            // Act
            bool result = table.IsBooked(currentDate);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsBooked_DateNotBooked_ShouldReturnFalse()
        {
            // Arrange
            RestaurantTable table = new RestaurantTable();
            DateTime currentDate = DateTime.Now;

            // Act
            bool result = table.IsBooked(currentDate);

            // Assert
            Assert.IsFalse(result);
        }
    }
}