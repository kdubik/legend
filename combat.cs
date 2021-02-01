using System;
using System.Collections.Generic;

namespace legend
{
    public enum BattleStatus { BATTLING, WIN, LOOSE, ENEMIES_FLIED, PLAYER_FLIED, CANCELLED, NOBATTLE };

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

        /// <summary>
        /// Combat class constructor
        /// </summary>
        /// <param name="lib">Pointer to the library</param>
        /// <param name="party">Pointer to the players party</param>
        /// <param name="lRoom">Actual room object</param>
        public Combat(Library lib, Party party, Room lRoom)
        {
            this.lib = lib;     // Data about enemies
            this.party = party; // Actual party members
            this.lRoom = lRoom; // Actual room

            enemies = lib.GetEnemyGroup(lRoom.enemyGroup);  // Enemies in the field
        }

        /// <summary>
        /// We need order troopers on battliefied regarding their "init roll"
        /// </summary>
        void SortBattlefield()
        {
            battlefield.Sort((a, b) => a.initRoll.CompareTo(b.initRoll));
            
            /*
            // Show result
            foreach (participant p in battlefield)
            {
                Console.WriteLine("{0} - {1}",p.id, p.initRoll.ToString());
            }*/
        }

        void PrepareBattlefield()
        {
            // 1. Add participants to the battlefield

            // a) Title hero
            participant hero = new participant("0",false);
            hero.health = party.members[0].health;
            // 100 - je tam preto, lebo sort by upradal bojovnikov od najmensieho po najvacsieho a my
            // to chceme invertovat.
            hero.initRoll = 100 - (dices.ThrowDiceX(3,6) + party.members[0].GetAttribute(Attribute.DEXTERITY));
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
                    hero.initRoll = 100 - (dices.ThrowDiceX(3,6) + en.attr[(int)Attribute.DEXTERITY]);
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
                case "hands": res=Attribute.FIGHTING; break;
                case "axe": res=Attribute.FIGHTING; break;
                case "bludgeon": res=Attribute.FIGHTING; break;
                case "heavy_blade": res=Attribute.FIGHTING; break;
                case "lance": res=Attribute.FIGHTING; break;
                case "polearm": res=Attribute.FIGHTING; break;
                default: res = Attribute.ACCURACY; break;
            }

            return res;
        }


        participant ChooseEnemyForAttack()
        {
            List<participant> pt = new List<participant>();
            int counter = 0;

            Console.WriteLine("Vyber, na koho postava zautoci:");
            foreach (participant lp in battlefield)
            {
                if (lp.isEnemy)
                {
                    if (lp.health>0)
                    {
                        counter++;
                        pt.Add(lp);
                        Console.Write("{0}. {1} (zdravie:", counter.ToString(), lp.enemy.name);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(lp.health.ToString());
                        Console.ResetColor();
                        Console.WriteLine(")");
                    }
                }
            }

            string input = Console.ReadLine();
            int x = int.Parse(input);
            return pt[x-1];
        }

        public BattleStatus DoBattle()
        {
            BattleStatus status = BattleStatus.BATTLING;

            Console.Clear();
            //Console.WriteLine("Combat!");
            PrepareBattlefield();

            // Hlavny battle cyklus
            int round = 0;
            do
            {
                round++;
                Console.WriteLine("Suboj, kolo {0} ----\n", round.ToString());

                foreach (participant p in battlefield)
                {
                    if (p.isEnemy)
                    {
                        if (p.health>0)
                        {
                            // Kto je na tahu
                            Console.Write("Na tahu je: ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(p.enemy.name);
                            Console.ResetColor();

                            //Utoci nepriatel
                            Console.WriteLine("{0} ({1} ziv.) utoci na {2} ({3} ziv.)!",
                            p.enemy.name, p.health.ToString(), party.members[0].name, party.members[0].health.ToString());

                            DicesRoll dc = new DicesRoll();
                            int uc = p.enemy.wheapon.attackRoll + dc.total;
                            int oc = party.members[0].defense;
                            int dmg = -1;
                            if (uc>oc)
                            {
                                dmg = dices.ThrowDiceString(p.enemy.wheapon.damage);
                                party.members[0].health -= dmg;
                                if (party.members[0].health<1) status = BattleStatus.LOOSE;
                            }
                            Console.WriteLine("{0} pouziva {1}!", p.enemy.name, p.enemy.wheapon.name);
                            Console.WriteLine("UC:{0} vs OC:{1}", uc.ToString(), oc.ToString());
                            if (dmg>-1) Console.WriteLine("Uspech! {0} sposobuje {1} bod(y) poskodenia!",
                            p.enemy.name,dmg.ToString());
                            
                            if (status == BattleStatus.LOOSE) 
                                Console.WriteLine("Postava {0} zahynula!", party.members[0].name);

                            Console.WriteLine("");
                        }
                    }
                    else
                    {
                        // Kto je na tahu
                        Console.Write("Na tahu je: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(party.members[0].name);
                        Console.ResetColor();
                        Console.WriteLine("(zdravie:{0})",party.members[0].health);

                        //Utoci hrac
                        if (party.members[0].health>0)
                        { 
                            // 1. Najprv si vyberieme ciel utoku
                            participant defender = ChooseEnemyForAttack();

                            Console.WriteLine("{0} ({1} ziv.) utoci na {2} ({3} ziv.)!",
                            party.members[0].name, party.members[0].health.ToString(), defender.enemy.name, defender.health.ToString());

                            // Actual wheapon
                            string wheaponTestString;
                            string wheaponDmg;
                            string wheaponName;
                            string act_wheapon_id = party.members[0].GetActualWheaponId();
                            if (act_wheapon_id!="")
                            {
                                Item actWheaponItem = lib.GetItem(act_wheapon_id);
                                wheaponTestString = actWheaponItem.GetAttribute("wheapon_type");
                                wheaponDmg = actWheaponItem.GetAttribute("damage");
                                wheaponName = actWheaponItem.name;
                            }
                            else
                            {
                                wheaponTestString = "hands";
                                wheaponDmg = "1d6";
                                wheaponName = "prazdne ruky";
                            }

                            Attribute wheaponTest = GetTestAttribute(wheaponTestString);

                            Console.WriteLine("{0} pouziva {1}!", party.members[0].name, wheaponName);
                            DicesRoll dc = new DicesRoll();
                            int uc = party.members[0].GetAttribute(wheaponTest) + dc.total;
                            int oc = party.members[0].defense;
                            int dmg = -1;
                            
                            Console.WriteLine("UC:{0} vs OC:{1}", uc.ToString(), oc.ToString());
                            if (uc>oc)
                            {
                                dmg = dices.ThrowDiceString(wheaponDmg);
                                defender.health -= dmg;
                                if (defender.health<1) status = BattleStatus.WIN;
                            }
                            
                            if (dmg>-1) Console.WriteLine("Uspech! {0} sposobuje {1} bod(y) poskodenia!",
                            party.members[0].name,dmg.ToString());
                            Console.WriteLine("");
                        }
                    }
                }
            } while (status==BattleStatus.BATTLING);

            return status;
        }
    }
}