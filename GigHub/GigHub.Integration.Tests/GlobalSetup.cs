﻿using GigHub.Core.Models;
using GigHub.Persistance;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigHub.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        [SetUp]
        public void SetUp()
        {
            MigrateDbToLatestVersion();
            Seed();
        }

        private static void MigrateDbToLatestVersion()
        {
            var configuration = new GigHub.Migrations.Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }

        public void Seed()
        {
            var context = new ApplicationDbContext();
            if (context.Users.Any())
                return;
            context.Users.Add(new ApplicationUser { Name = "user1", UserName = "user1", Email = "-", PasswordHash = "-" });
            context.Users.Add(new ApplicationUser { Name = "user2", UserName = "user2", Email = "-", PasswordHash = "-" });
            context.SaveChanges();
        }
    }
}
