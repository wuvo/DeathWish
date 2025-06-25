using System;
using NHibernate;

namespace Ultimate.MysqlDB
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static bool BuildSessionFactory()
        {
            if (_sessionFactory != null) return false;

            var configuration = new NHibernate.Cfg.Configuration();
            configuration.Configure("hibernate.cfg.xml");
            configuration.AddAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            _sessionFactory = configuration.BuildSessionFactory();

            return true;
        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}