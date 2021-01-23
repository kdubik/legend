using System.Collections.Generic;

namespace legend
{
    public struct EnemyRecord
    {
        public string id;
        public int count;

        public EnemyRecord(string id, int count)
        {
            this.id = id;
            this.count = count;
        }
    }

    public class EnemyGroup
    {
        public string id;       // ID samotnej grupy
        public string name;

        public List<EnemyRecord> enemies = new List<EnemyRecord>();
        public List<string> treasures = new List<string>();

        public EnemyGroup(string id)
        {
            this.id = id;
        }

        public void AddEnemy(string id, int count) => enemies.Add(new EnemyRecord(id,count));
        public void AddTreasure(string id) => treasures.Add(id);
    }
}