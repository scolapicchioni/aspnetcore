using MyTested.AspNetCore.Mvc;
using PhotoSharingApplication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhotoSharingApplication.Tests
{
    public class RoutingTests
    {
        [Fact]
        public void RootMapsToHomeController() {
            MyMvc
                .Routing()
                .ShouldMap("/")
                .To<HomeController>();                
        }
        [Fact]
        public void HomeMapsToHomeController()
        {
            MyMvc
                .Routing()
                .ShouldMap("/Home")
                .To<HomeController>();
        }
        [Fact]
        public void HomeIndexMapsToHomeController()
        {
            MyMvc
                .Routing()
                .ShouldMap("/Home/Index")
                .To<HomeController>();
        }
    }
}
