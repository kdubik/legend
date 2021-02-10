using System;
using System.Collections.Generic;

namespace legend
{
    public enum ErrorCode { OK, ROOM_NOT_FOUND, EMPTY_PATH, NOT_ENABLED, ACTION_NOT_FOUND, ERROR };

    public class Engine
    {
        public Library lib = new Library();
        public Party party = new Party();
        public List<InvertoryItem> gameItems = new List<InvertoryItem>();
        public Engine()
        {
            Console.Write("Initializing engine:");
            lib.LoadDataFiles();
            party.actualRoomID = lib.gameInfo.startRoom ;
        }

        public void EraseEnemiesInActualRoom()
        {
            Room actRoom = lib.GetRoom(party.actualRoomID);
            actRoom.enemyGroup = "";
        }
        public void EquipItem(Item itm)
        {
            if (itm.type==ItemType.WHEAPON)
                party.members[0].bodySlots[(int)BodySlot.WHEAPON] = itm.id;
            if (itm.type==ItemType.ARMOR)
                party.members[0].bodySlots[(int)BodySlot.ARMOR] = itm.id;
            if (itm.type==ItemType.SHIELD)
                party.members[0].bodySlots[(int)BodySlot.SHIELD] = itm.id;          
        }

        public void EquipItem(string itemId)
        {
            Item itm = lib.GetItem(itemId);
            if (itm!=null) EquipItem(itm);
        }

        public GameItem GiveItemToPlayer(string itemId, bool eqip)
        {
            GameItem gmi = null;
            Item tmpItem = lib.GetItem(itemId);
            
            if (tmpItem!=null)
            {
                gmi = new GameItem(itemId,"player");
                if (gmi!=null)
                {
                    UpdateGameItemInfo(ref gmi);   // Ziska zaujimave informacie z objektu a napise od game itemu
                    lib.gameItems.Add(gmi);

                    // We want also to equip item imediatelly
                    if (eqip) EquipItem(tmpItem);
                }
            }

            return gmi;
        }
        public bool DoTest(Attribute inAttribute, int testLevel)
        {
            bool res = false;

            Dices dice = new Dices();
            int rollValue = dice.ThrowDiceString("3k6");
            int attValue = party.members[0].GetAttribute(inAttribute);
            int totalNo = rollValue + attValue;

            Console.WriteLine("Testovany atribut ({0}): {1}", inAttribute.ToString(), attValue.ToString());
            Console.WriteLine("Hod kockami: {0}", rollValue.ToString());
            Console.WriteLine("Postava: {0} vs Test level: {1}", totalNo.ToString(), testLevel.ToString());

            if (totalNo>testLevel) 
            {
                res = true;
                Console.WriteLine("Test bol uspesny!");
            } else Console.WriteLine("Test bol neuspesny!");
            Console.WriteLine("");

            return res;
        }

        public void UpdateGameItemInfo(ref GameItem gi)
        {
            Item itm = lib.GetItem(gi.id);
            gi.itemName = itm.name;
            gi.itemType = itm.type;
        }

        public void Print(string msg)
        {
           Console.WriteLine(Tools.RemoveQuotes(msg));
        } 

        public bool ExecuteCommand(string cmd)
        {
            bool res = true;

            string[] words = cmd.Split(" ");

            // Show message
            if (words[0]=="show_msg")
            {               
                if ( words[1][0]!='"')
                {
                    if (lib.texts.ContainsKey(words[1]))
                    {
                        Console.WriteLine(lib.texts[words[1]]);
                    }
                    else
                    {
                        Console.WriteLine("Unable to find text block: '{0}'", words[1]);
                    }
                }
                else
                {
                    string msg = Tools.MergeString(words,1);
                    Print(msg);
                }                
            }

            // Show message and wait for ENTER
            if (words[0]=="show_msg_wait")
            {
                if (words[1]!="\"")
                {
                    if (lib.texts.ContainsKey(words[1]))
                    {
                        Console.WriteLine(lib.texts[words[1]]);
                        Console.WriteLine("\nStlac ENTER pre pokracovanie...");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Unable to find text block: '{0}'", words[1]);
                    }
                }
                else
                {
                    string msg = Tools.MergeString(words,1);
                    Console.WriteLine(msg);
                    Console.WriteLine("\nStlac ENTER pre pokracovanie...");
                    Console.ReadLine();
                }  
                   
            }

            // Give item to player
            if (words[0]=="give_item")
            {
                GameItem gmi = GiveItemToPlayer(words[1],false);
                Console.WriteLine("Ziskavas '{0}'!", lib.GetTextBlock(gmi.itemName));
            }

            // Enable target action
            if (words[0]=="enable_action")
            {
                Action act = lib.GetAction(words[1]);
                if (act!=null)
                {
                    act.enabled = true;
                }
            }

            // Disable target action
            if (words[0]=="disable_action")
            {
                Action act = lib.GetAction(words[1]);
                if (act!=null)
                {
                    act.enabled = false;
                }
            }

            // Move player to target destination
            if (words[0]=="teleport")
            {
                party.actualRoomID = words[1];
            }

            return res;
        }
        public ErrorCode ExecuteActionList(List<string> commands)
        {
            ErrorCode ret = ErrorCode.OK;

            foreach(string cmdLine in commands)
            {
                if (!ExecuteCommand(cmdLine)) ret = ErrorCode.ERROR;
            }

            return ret;
        }

        /// <summary>
        /// Checks, whether there is a combat situation,
        /// after party enters some location.
        /// </summary>
        public BattleStatus Check_combat()
        {
            BattleStatus bs = BattleStatus.NOBATTLE;
            // Check for "planned" encounter
            Room lRoom = lib.GetRoom(party.actualRoomID);
            if (lRoom.enemyGroup!="")
            {
                Combat combat = new Combat(lib, party, lRoom);
                Console.WriteLine("Stlac ENTER pre zaciatok suboja");
                Console.ReadLine();
                bs = combat.DoBattle();
            }

            // Check for random encounter

            return bs;
        }

        public ErrorCode Go(Path path)
        {
            ErrorCode ec = ErrorCode.OK;
            Road lRoad = lib.GetRoad(party.actualRoomID, path);

            if (lRoad!=null)
            {
                //Console.Write(lRoad.enabled.ToString());
                if (lRoad.enabled)
                {
                    if (lRoad.sourceRoom==party.actualRoomID)
                    {
                        if ((lRoad.bothWay == Direction.BOTH) ||  (lRoad.bothWay == Direction.TO_TARGET))
                        {
                            party.actualRoomID = lRoad.targetRoom;
                        } else ec=ErrorCode.EMPTY_PATH;
                    }
                    else
                    {
                        if ((lRoad.bothWay == Direction.BOTH) || (lRoad.bothWay == Direction.TO_SOURCE))
                        {
                            party.actualRoomID = lRoad.sourceRoom;
                        } else ec=ErrorCode.EMPTY_PATH;
                    }
                }
                else
                { 
                    ec=ErrorCode.EMPTY_PATH;
                    //Console.WriteLine("DISABLED");
                }
            } 
            else ec=ErrorCode.EMPTY_PATH;
           
            return ec;
        }
    }
}