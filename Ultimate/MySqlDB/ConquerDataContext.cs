using Ultimate.MysqlDB.Domain;
using Ultimate.MysqlDB.Repositories;

namespace Ultimate.MysqlDB
{
    public class ConquerDataContext
    {
        private StatsRepository _StatsRepository;
        private MinigRepository _MineRepository;
        public StatsRepository Stats
        {
            get { return _StatsRepository ?? (_StatsRepository = new StatsRepository()); }
        }
        public MinigRepository MineStats
        {
            get { return _MineRepository ?? (_MineRepository = new MinigRepository()); }
        }

    }
}
