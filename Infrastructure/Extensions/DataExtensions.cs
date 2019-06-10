using System.Configuration;
using System.Web;
using System.Web.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;

namespace WebApiNHibernateCrudPagination.Infrastructure.Extensions
{
    public static class DataExtensions
    {
        public static FluentConfiguration AddMsSql(this FluentConfiguration fluentConfiguration)
        {
            var mysqlConnectionString = WebConfigurationManager.ConnectionStrings["MsSql"].ConnectionString;
            fluentConfiguration.Database(MySQLConfiguration.Standard.ConnectionString(mysqlConnectionString)
                .ShowSql()
                .Dialect<MsSql2012Dialect>());
            return fluentConfiguration;
        }

        public static FluentConfiguration AddMySql(this FluentConfiguration fluentConfiguration)
        {
            var mysqlConnectionString = WebConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            fluentConfiguration.Database(MySQLConfiguration.Standard.ConnectionString(mysqlConnectionString)
                .ShowSql()
                .Dialect<MySQL5Dialect>());
            return fluentConfiguration;
        }

        public static FluentConfiguration AddSqLite(this FluentConfiguration fluentConfiguration)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Sqlite"].ConnectionString.Replace("{CWD}",
                HttpContext.Current.Server.MapPath("~"));
            fluentConfiguration.Database(SQLiteConfiguration.Standard
                .ConnectionString(connectionString)
                .ShowSql().Dialect<SQLiteDialect>());

            return fluentConfiguration;
        }
    }
}