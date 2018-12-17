﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalendarSkill.Models;
using CalendarSkill.ServiceClients;
using CalendarSkillTest.API.Fakes;
using Microsoft.Bot.Solutions.Skills;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CalendarSkillTest.API
{
    [TestClass]
    public class ServiceManagerTests
    {
        public static IServiceManager serviceManager;
        private static Mock<ISkillConfiguration> mockConfig;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            mockConfig = new Mock<ISkillConfiguration>();
            mockConfig.SetupGet(config => config.Properties).Returns(() =>
            {
                Dictionary<string, object> props = new Dictionary<string, object>();
                props.Add("googleAppName", "testAppName");
                props.Add("googleClientId", "testClientId");
                props.Add("googleClientSecret", "testClientSecret");
                props.Add("googleScopes", "testScopes");

                return props;
            });
            serviceManager = new ServiceManager(mockConfig.Object);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestInit()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public async Task GetMSUserServiceTest()
        {
            IUserService userService = serviceManager.InitUserService("test token", EventSource.Microsoft);
            Assert.IsTrue(userService is UserService);
        }

        [TestMethod]
        public async Task GetMSCalendarServiceTest()
        {
            ICalendarService calendarService = serviceManager.InitCalendarService("test token", EventSource.Microsoft);
            Assert.IsTrue(calendarService is CalendarService);
        }

        [TestMethod]
        public async Task GetGoogleUserServiceTest()
        {
            IUserService userService = serviceManager.InitUserService("test token", EventSource.Google);
            Assert.IsTrue(userService is UserService);
        }

        [TestMethod]
        public async Task GetGoogleCalendarServiceTest()
        {
            ICalendarService calendarService = serviceManager.InitCalendarService("test token", EventSource.Google);
            Assert.IsTrue(calendarService is CalendarService);
        }



        [TestMethod]
        public async Task GetOtherUserServiceTest_Throws()
        {
            try
            {
                IUserService userService = serviceManager.InitUserService("test token", EventSource.Other);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "Event Type not Defined");
                return;
            }

            Assert.Fail("Should throw exception");
        }

        [TestMethod]
        public async Task GetOtherCalendarServiceTest_Throws()
        {
            try
            {
                ICalendarService calendarService = serviceManager.InitCalendarService("test token", EventSource.Other);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "Event Type not Defined");
                return;
            }

            Assert.Fail("Should throw exception");
        }
    }
}