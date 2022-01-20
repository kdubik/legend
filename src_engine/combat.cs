using System;
using System.Collections.Generic;

using LegendLibrary;
using LegendTools;

namespace LegendEngine
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
            hero.initRoll = 100 - (dices.ThrowDiceX(3,6) + party.members[0].GetAttribute(CharAttr.DEXTERITY));
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
                    hero.initRoll = 100 - (dices.ThrowDiceX(3,6) + en.attr[(int)CharAttr.DEXTERITY]);
                    battlefield.Add(hero);
                }
            }
            SortBattlefield();  // Upraceme bojisko podla hodu na iniciativu
        }

        CharAttr GetTestAttribute(string wheaponType)
        {
            CharAttr res = CharAttr.ACCURACY;

            switch(wheaponType)
            {
                case "hands": res=CharAttr.FIGHTING; break;
                case "axe": res=CharAttr.FIGHTING; break;
                case "bludgeon": res=CharAttr.FIGHTING; break;
                case "heavy_blade": res=CharAttr.FIGHTING; break;
                case "lance": res=CharAttr.FIGHTING; break;
                case "polearm": res=CharAttr.FIGHTING; break;
                default: res = CharAttr.ACCURACY; break;
            }

            return res;
        }

        participant ChooseEnemyForAttack()
        {
            List<participant> pt = new List<participant>();
            int counter = 0;
            string lname = "";

            Console.WriteLine("Vyber, na koho postava zautoci:");
            foreach (participant lp in battlefield)
            {
                if (lp.isEnemy)
                {
                    if (lp.health>0)
                    {
                        counter++;
                        pt.Add(lp);
                        lname = lib.GetTextBlock(lp.enemy.name);
                        Console.Write("{0}. {1} (zdravie:", counter.ToString(), lname);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(lp.health.ToString());
                        Console.ResetColor();
                        Console.WriteLine(")");
                    }
                }
            }

            int x = Textutils.GetNumberRange(1,counter);
            return pt[x-1];
        }

        public BattleStatus DoBattle()
        {
            BattleStatus status = BattleStatus.BATTLING;
            string lname = "";  // local attacker name
            string wname = "";  // local wheapon name
            string dname = "";  // local defender name

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(lib.GetTextBlock(enemies.name_group));
            Console.ResetColor();

            PrepareBattlefield();

            // MAIN BATTLE CYCLUS
            int round = 0;
            do
            {
                round++;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Suboj, kolo {0} ----\n", round.ToString());
                Console.ResetColor();

                foreach (participant p in battlefield)
                {
                    if (p.isEnemy)
                    {
                        // Kto je na tahu - ENEMY
                        if (p.health>0)
                        {
                            // Kto je na tahu
                            lname = lib.GetTextBlock(p.enemy.name);
                            wname = lib.GetTextBlock(p.enemy.wheapon.name);

                            Console.Write("Na tahu je: ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(lname);
                            Console.ResetColor();
                            Console.WriteLine(" (zdravie:{0})",p.health.ToString());

                            Console.WriteLine("{0} utoci na {1}!",
                            lname, party.members[0].name);

                            // Actual player ARMOR
                            string usedArmor = party.members[0].GetActualArmorId();
                            string armorName = "";
                            string armorType = "";
                            int armorBonus = 0; // No armor at all
                            bool isArmorInArmorGroup = false;
                            int penalty = 0;
                            bool isArmorValid = false;
                            if (usedArmor!="")
                            {
                                Item actArmorItem = lib.GetItem(usedArmor);
                                armorName = lib.GetTextBlock(actArmorItem.say);
                                armorType = actArmorItem.GetAttribute("armor_type");
                                armorBonus = actArmorItem.GetArmorBonus();
                                if (armorBonus>0) isArmorValid=true;
                                isArmorInArmorGroup = party.members[0].CheckArmorGroup(armorType);
                                if (isArmorValid)
                                {
                                    if (!isArmorInArmorGroup) penalty = actArmorItem.GetArmorPenalty();
                                }
                            }

                            if (!isArmorValid) Console.WriteLine("Postava nepouziva ziadne brnenie.");
  
                            DicesRoll dc = new DicesRoll();
                            int uc = p.enemy.wheapon.attackRoll + dc.total;
                            int oc = party.members[0].defense;
                            
                            if (isArmorValid)
                            {
                                Console.WriteLine("Postava pouziva brnenie ({0})!", armorName);
                                if (!isArmorInArmorGroup)
                                {
                                    Console.WriteLine("Postava nie je trenovana pre tento typ zbroje!");
                                    Console.WriteLine("Postava dostava penaltu {0} k obrane a rychlosti!",
                                        penalty.ToString());
                                    Console.WriteLine("Obrana znizena {0} -> {1}!", oc.ToString(),
                                        (oc-penalty).ToString());
                                    oc-=penalty;
                                }
                            }
                            
                            int dmg = -1;
                            if (uc>oc)
                            {
                                dmg = dices.ThrowDiceString(p.enemy.wheapon.damage);
                                party.members[0].health -= dmg;
                                if (party.members[0].health<1) status = BattleStatus.LOOSE;
                            }
                            Console.WriteLine("{0} pouziva {1}! (uc:{2} vs oc:{3})", lname, wname,
                            uc.ToString(), oc.ToString());  

                            if (dmg>-1) 
                            {
                                if (isArmorValid)
                                {
                                    int newDmg = dmg-armorBonus;
                                    if (newDmg<0) newDmg = 0;
                                    Console.WriteLine("Zbroj (+{0}) zachytila uder a znizila poskodenie ({1}->{2})!",
                                        armorBonus.ToString(), dmg.ToString(), newDmg.ToString());
                                }
                                Console.Write("Uspech! {0} sposobuje ",lname);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(dmg.ToString());
                                Console.ResetColor();
                                Console.WriteLine(" bod(y) poskodenia!");
                            }
                            Console.WriteLine("");

                            if (status == BattleStatus.LOOSE) 
                                Console.WriteLine("Postava {0} zahynula!", party.members[0].name);

                            Console.WriteLine("");
                        }
                    }
                    else
                    {
                        // Kto je na tahu - HRAC
                        lname = party.members[0].name;
                        Console.Write("Na tahu je: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(lname);
                        Console.ResetColor();
                        Console.Write(" (zdravie:");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("{0}",party.members[0].health);
                        Console.ResetColor();
                        Console.WriteLine(")");

                        //Utoci hrac
                        if (party.members[0].health>0)
                        { 
                            // 1. Najprv si vyberieme ciel utoku
                            participant defender = ChooseEnemyForAttack();
                            dname = lib.GetTextBlock(defender.enemy.name);
                            
                            //Console.WriteLine("{0} ({1} ziv.) utoci na {2} ({3} ziv.)!",
                            //lname, party.members[0].health.ToString(), dname, defender.health.ToString());
                            Console.WriteLine("{0} utoci na {1}!",
                            lname, dname);

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
                                wheaponName = lib.GetTextBlock(actWheaponItem.say);
                            }
                            else
                            {
                                wheaponTestString = "hands";
                                wheaponDmg = "1d6";
                                wheaponName = "prazdne ruky";
                            }

                            CharAttr wheaponTest = GetTestAttribute(wheaponTestString);
                            bool focusedWheapon = party.members[0].IsUsedWheaponFocused(wheaponTestString);
                            bool knownWheaponGroup = party.members[0].CheckWheaponGroup(wheaponTestString);

                            DicesRoll dc = new DicesRoll();
                            int uc = party.members[0].GetAttribute(wheaponTest) + dc.total;
                            int oc = party.members[0].defense;
                            int dmg = -1;
                            if (focusedWheapon)
                            {
                                Console.WriteLine("Postava ma focus na zbran {0} ({1}). Ziskava bonus +2 na utok!",wheaponName,wheaponTestString);
                                Console.WriteLine("UC {0} + bonus = {1}\n", uc.ToString(), (uc+2).ToString());
                                uc+=2;
                            }
                            if (knownWheaponGroup)
                            {
                                Console.WriteLine("{0} pouziva zname zbrane ({1}).", lname, wheaponTestString );
                            }
                            else
                            {
                                Console.WriteLine("{0} pouziva zbrane, ktore nepozna! ({1}).", lname, wheaponTestString );
                                Console.WriteLine("Penalta -2 na útok (UC znizene na: {0}), polovičné škody!", (uc-2).ToString());
                                uc-=2;
                            }
                            
                            Console.WriteLine("{0} pouziva {1}! (UC:{2} vs OC:{3})", lname, wheaponName,
                             uc.ToString(), oc.ToString());
                             
                            if (uc>oc)
                            {
                                //Console.WriteLine("Dmg dice - {0}", wheaponDmg);
                                dmg = dices.ThrowDiceString(wheaponDmg);
                                defender.health -= dmg;
                                if (defender.health<1) status = BattleStatus.WIN;
                            }
                            
                            if (dmg>-1) 
                            {
                                if (!knownWheaponGroup) 
                                {
                                    int ndmg = dmg / 2;
                                    Console.Write("Sposobnene zranenia su polovicne! ({0}->{1}) ",
                                    dmg.ToString(), ndmg.ToString());
                                    dmg = ndmg;
                                }
                                Console.Write("Uspech! {0} sposobuje ",lname);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(dmg.ToString());
                                Console.ResetColor();
                                Console.WriteLine(" bod(y) poskodenia!");
                            }
                            Console.WriteLine("");
                        }
                    }
                }
            } while (status==BattleStatus.BATTLING);

            return status;
        }
    }
}