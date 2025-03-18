using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstProjectNET.Controllers;
using FirstProjectNET.Data;
using FirstProjectNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace HotelManagementTest
{
    public class PropertiesControllerTest
    {
        private readonly PropertiesController _controller;
        private readonly HotelDbContext _hotelDbContext;
        public PropertiesControllerTest()
        {
            var option = new DbContextOptionsBuilder<HotelDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _hotelDbContext = new HotelDbContext(option);
            SeedDatabase();
            
        }

        private void SeedDatabase()
        {
            _hotelDbContext.Categories.Add(new Category { CategoryID = "Standard10", TypeName = "Standard Extend", Capacity = 5, Price = 5000000 });
            _hotelDbContext.Rooms.Add(new Room { RoomID = "R105", CategoryID = "Standard10", Status = "Vacant", Description = null });
            _hotelDbContext.RentForms.Add(new RentForm
            {
                RentFormID = "RF00001",
                BookingID = "BK00001",
                RoomID = "R201",
                StaffID = "S0001",
                CustomerID = "CUS0001",
                DateCreate = DateTime.Parse("2022-01-09"),
                DateCheckIn = DateTime.Parse("2022-01-09"),
                DateCheckOut = DateTime.Parse("2022-01-12"),
                Sale = 0.10m
            });
            _hotelDbContext.Images.Add(new Image { ImageID = "I1", RoomID = "R105", ImageUrl = "room1.jpg" });
            _hotelDbContext.Services.Add(new Services { ServiceID = "S1", ServiceName = "WiFi", Price = 0, Description = "hello" });
            _hotelDbContext.RoomServices.Add(new RoomService { RoomID = "R105", ServiceID = "S1" });
            _hotelDbContext.SaveChanges();
        }
        [Fact]
        public void Index_Return_View_With_Correct_Pagination()
        {
            var result = _controller.Index(page: 1) as ViewResult;
            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["Page"]);
            Assert.NotNull(result.ViewData["NoOfPage"]);
        }
        [Fact]
        public void RoomAvailable_Should_Return_View_With_AvailableRooms()
        {
            var dateCome = DateTime.Now;
            var dateGo = DateTime.Now.AddDays(1);

            var result = _controller.RoomAvailable(null, dateCome, dateGo, 2) as ViewResult;
            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["Rooms"]);
        }
        [Fact]
        public void RoomAvailable_InvalidDate_Should_Return_WithError()
        {
            var dateCome = DateTime.Now.AddDays(2);
            var dateGo = DateTime.Now.AddDays(1);
            var result = _controller.RoomAvailable(null, dateCome, dateGo, 2) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["Rooms"]);
        }
        [Fact]
        public void RoomDetail_WithNullRoomID()
        {
            var result = _controller.Details(null);
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void RoomDetails_WithInvalidRoomsID()
        {
            var result = _controller.Details("Hello");
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void RoomDetails_WithValidRoomID()
        {
            //SeedDatabase();

            var result = _controller.Details("Standard10") as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Equal("room1.jpg", result.ViewData["ImageUrl"]);
            Assert.Equal("Wifi", result.ViewData["RoomService"]);
        }
    }

}