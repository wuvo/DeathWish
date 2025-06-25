

using Paradise.Game;

namespace Paradise.MysqlDB
{
    public class ServerDatabase
    {
        public static ConquerDataContext Context { get; private set; }

        public static bool InitializeSql()
        {
            Context = new ConquerDataContext();
            NHibernateHelper.BuildSessionFactory();
            return true;
        }
    }
}
