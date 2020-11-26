using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.DataContext;

namespace DataAccessLayer.DataContext
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<TrackerDbContext>
    {
        public TrackerDbContext CreateDbContext(string[] args)
        {
            AppConfiguration appConfig = new AppConfiguration();
            var opsBuilder = new DbContextOptionsBuilder<TrackerDbContext>();
            opsBuilder.UseSqlServer(appConfig.connectionstring);
            return new TrackerDbContext(opsBuilder.Options);
        }
    }
}
