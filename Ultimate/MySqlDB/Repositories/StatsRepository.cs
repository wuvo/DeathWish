using Ultimate.MysqlDB.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Ultimate.Game;

namespace Ultimate.MysqlDB.Repositories
{
    public class StatsRepository : Repository<uint, Stats>
    {
        public void myStats(string _type, uint _count)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var t = session.CreateSQLQuery("UPDATE drop_stats SET " + _type + "=" + _type + "+" + _count);
                t.ExecuteUpdate();
            }
        }
    }
}
