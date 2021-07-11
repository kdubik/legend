using System.Collections.Generic;

namespace LegendLibrary
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
        public string name_attack;  // How we name this group, when tiey are attacking?
        public string name_group;   // How to call this group of enemies, that are wandering around?
        public int friendliness = -1;    //Standard enemy (1 friend, 0 none, -1 enemy, -2 angry enemy)

        public List<EnemyRecord> enemies = new List<EnemyRecord>();
        public List<string> treasures = new List<string>();
        public string leader = "";   // Id of character, who is in charge

        public EnemyGroup(string id)
        {
            this.id = id;
        }

        public void AddEnemy(string id, int count) => enemies.Add(new EnemyRecord(id,count));
        public void AddTreasure(string id) => treasures.Add(id);
    }
}