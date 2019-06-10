using NHibernate;

namespace WebApiNHibernateCrudPagination.Data
{
    public interface ISessionFactoryBuilder
    {
        ISessionFactory GetSessionFactory();
    }
}