using System;
using System.Collections.Generic;

namespace legend
{
    public enum BattleStatus { BATTLING, WIN, LOOSE, ENEMIES_FLIED, PLAYER_FLIED, CANCELLED };

    class participant
    {
        public string id;
        public bool isEnemy;

        public Enemy enemy;

        public int health;

        public int initRoll;

        public participant(string id, bool isEnemy)
        {
            this.id = id;
            this.isEnemy = isEnemy;
        }
    }

    public class Combat
    {
        Library lib;
        Room lRoom;
        Party party;
        EnemyGroup enemies;
        Dices dices = new Dices();

        List<participant> battlefield = new List<participant>();

        public Combat(Library lib, Party party, Room lRoom)
        {
            this.lib = lib;     // Data about enemies
            this.party = party; // Actual party members
            this.lRoom = lRoom; // Actual room

            enemies = lib.GetEnemyGroup(lRoom.enemyGroup);  // Enemies in the field
        }

        void SortBattlefield()
        {
            battlefield.Sort((a, b) => a.initRoll.CompareTo(b.initRoll));
            
            // Show result
            foreach (participant p in battlefield)
            {
                Console.WriteLine("{0} - {1}",p.id, p.initRoll.ToString());
            }
        }

        void PrepareBattlefield()
        {
            // 1. Add participants to the battlefield

            // a) Title hero
            participant hero = new participant("0",false);
            hero.health = party.members[0].health;
            hero.initRoll = dices.ThrowDiceX(3,6) + party.members[0].GetAttribute(Attribute.DEXTERITY);
            battlefield.Add(hero);
            
            // b) Enemies
            foreach (EnemyRecord er in enemies.enemies)
            {
                for (int a=0;a<er.count;a++)    // nepriatelia mozu byt aj viacnasobny...
                {
                    Enemy en = lib.GetEnemy(er.id);
                    hero = new participant(er.id,true);
                    hero.enemy = en;
                    hero.health = en.health;
                    hero.initRoll = dices.ThrowDiceX(3,6) + en.attr[(int)Attribute.DEXTERITY];
                    battlefield.Add(hero);
                }
            }
            SortBattlefield();  // Upraceme bojisko podla hodu na iniciativu
        }

        Attribute GetTestAttribute(string wheaponType)
        {
            Attribute res = Attribute.ACCURACY;

            switch(wheaponType)
            {
                case "axe": res=Attribute.FIGHTING; break;
                case "bludgeon": res=Attribute.FIGHTING; break;
                case "heavy_blade": res=Attribute.FIGHTING; break;
                case "lance": res=Attribute.FIGHTING; break;
                case "polearm": res=Attribute.FIGHTING; break;
                default: res = Attribute.ACCURACY; break;
            }

            return res;
        }

        public BattleStatus DoBattle()
        {
            BattleStatus status = BattleStatus.BATTLING;

            Console.Clear();
            Console.WriteLine("Combat!");
            PrepareBattlefield();

            // Hlavny battle cyklus
            do
            {
                foreach (participant p in battlefield)
                {
                    if (p.isEnemy)
                    {
                        //Utoci nepriatel
                        DicesRoll dc = new DicesRoll();
                        int uc = p.enemy.wheapon.attackRoll + dc.total;
                        int oc = party.members[0].defense;
                        int dmg = -1;
                        if (uc>oc)
                        {
                            dmg = dices.ThrowDiceString(p.enemy.wheapon.damage);
                        }
                        Console.WriteLine("{0} pouziva {1}!", p.enemy.name, p.enemy.wheapon.name);
                        Console.WriteLine("UC:{0} vs OC:{1}");
                        if (dmg>-1) Console.WriteLine("Uspech! {0} sposobuje {1} bod(y) poskodenia!");
                        Console.WriteLine("");
                    }
                    else
                    {
                        //Utoci hrac
                        
                        // Actual wheapon
                        string act_wheapon = party.members[0].GetActualWheaponId();

                        Console.WriteLine("{0} pouziva {1}!", party.members[0].name, p.enemy.wheapon.name);
                        DicesRoll dc = new DicesRoll();
                        //int hod_na_uc = dices
                        int uc = p.enemy.wheapon.attackRoll + dc.total;
                        int oc = party.members[0].defense;
                        int dmg = -1;
                    }
                }
            } while (status!=BattleStatus.BATTLING);

            return status;
        }
    }
}