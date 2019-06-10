using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace WebApiNHibernateCrudPagination.Data
{
    public class XmlSessionFactoryBuilder : ISessionFactoryBuilder
    {
        private volatile object _sessionCreated = false;
        private ISessionFactory _sessionFactory;


        public ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory != null) return _sessionFactory;
            lock (_sessionCreated)
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    // configuration.SetProperty("hbm2ddl.auto", "create");
                    var configurationPath = HttpContext.Current.Server.MapPath(@"~\Entities\hibernate.cfg.xml");
                    configuration.Configure(configurationPath);


                    configuration.DataBaseIntegration(x =>
                    {
                        x.LogFormattedSql = true;
                        x.LogSqlInConsole = true;
                    });

                    var todoMappingFile =
                        HttpContext.Current.Server.MapPath(@"~\Entities\Mappings\Todo.hbm.xml");
                    configuration.AddFile(todoMappingFile);

                    _sessionFactory = configuration.BuildSessionFactory();
                }
            }

            return _sessionFactory;
        }
    }
}