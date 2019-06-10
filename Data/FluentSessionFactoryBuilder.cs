using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using WebApiNHibernateCrudPagination.Entities;
using WebApiNHibernateCrudPagination.Infrastructure.Extensions;

namespace WebApiNHibernateCrudPagination.Data
{
    public class FluentSessionFactoryBuilder : ISessionFactoryBuilder
    {
        private volatile object _sessionCreated = false;
        private static ISessionFactory _sessionFactory;

        public FluentSessionFactoryBuilder()
        {
        }

        public ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory != null) return _sessionFactory;
            lock (_sessionCreated)
            {
                if (_sessionFactory == null)
                {
                    var mappings = new List<Type>();
                    mappings.Add(typeof(Todo));
                    var entityListener = new EntitySaveListener();
                    var fluentConfiguration = Fluently.Configure()
                        // .Database(SQLiteConfiguration.Standard.ConnectionString(this._connectionString).ShowSql())
                        .AddSqLite()
                        // To add Many mappings from a List<Type>
                        // .Mappings(m => mappings.ForEach(e => { m.FluentMappings.Add(e); }))
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernate.Cfg.Mappings>())
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Mappings.TodoMap>())
                        .CurrentSessionContext("call")
                        .ExposeConfiguration(cfg =>
                        {
                            BuildSchema(cfg, false, true);
                            cfg.AppendListeners(ListenerType.PreInsert,
                                new IPreInsertEventListener[] {entityListener});
                            cfg.AppendListeners(ListenerType.PreUpdate,
                                new IPreUpdateEventListener[] {entityListener});
                        });

                    _sessionFactory = fluentConfiguration.BuildSessionFactory();
                }
            }

            return _sessionFactory;
        }

        /// <summary>
        ///     Build the schema of the database.
        /// </summary>
        /// <param name="config">Configuration.</param>
        private static void BuildSchema(Configuration config, bool create = false, bool update = false)
        {
            if (create)
                new SchemaExport(config).Create(false, true);
            else
                new SchemaUpdate(config).Execute(false, update);
        }
    }
}