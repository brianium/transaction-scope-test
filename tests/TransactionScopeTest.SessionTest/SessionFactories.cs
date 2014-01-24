using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace TransactionScopeTest.SessionTest
{
    public class SessionFactories
    {
        private static readonly object Locker = new object();

        public static ISessionFactory ProductConnectionFactory { get; set; }
        public static ISessionFactory OrderConnectionFactory { get; set; }

        static SessionFactories()
        {
            lock (Locker)
            {
                ProductConnectionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("ProductConnection")))
                    .Mappings(cfg => cfg.FluentMappings.AddFromAssemblyOf<SessionFactories>())
                    .BuildConfiguration()
                    .CurrentSessionContext<ThreadStaticSessionContext>().BuildSessionFactory();

                OrderConnectionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("OrderConnection")))
                    .Mappings(cfg => cfg.FluentMappings.AddFromAssemblyOf<SessionFactories>())
                    .BuildConfiguration()
                    .CurrentSessionContext<ThreadStaticSessionContext>().BuildSessionFactory();
            }
        }
    }
}
