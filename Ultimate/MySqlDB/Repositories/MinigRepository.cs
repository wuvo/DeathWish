using Ultimate.MysqlDB.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Ultimate.Game;

namespace Ultimate.MysqlDB.Repositories
{
    public class MinigRepository : Repository<uint, Stats>
    {
        public void MineStats(string _type, uint _count)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var t = session.CreateSQLQuery("UPDATE minig_stats SET " + _type + "=" + _type + "+" + _count);
                t.ExecuteUpdate();
            }
        }
    }
}
