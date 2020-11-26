using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccessLayer.DataContext
{
    public class AppConfiguration
    {
        public AppConfiguration()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            connectionstring = configuration.GetValue<string>("ConnectionStrings:TrackerDb");
        }

        public string connectionstring { get; set; }
    }
}
