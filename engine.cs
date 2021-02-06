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
            party.actualRoomID = "entrance";
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
        
        public bool ExecuteCommand(string cmd)
        {
            bool res = true;

            string[] words = cmd.Split(" ");
            if (words[0]=="show_msg")
            {
                Console.WriteLine(lib.texts[words[1]]);
            }

            if (words[0]=="add_item")
            {
                GameItem gmi = GiveItemToPlayer(words[1],false);
                Console.WriteLine("Ziskavas '{0}'!", gmi.itemName);
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