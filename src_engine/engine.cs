using System;
using System.Collections.Generic;

using LegendLibrary;
using LegendTools;

namespace LegendEngine
{
    public enum ErrorCode { OK, ROOM_NOT_FOUND, EMPTY_PATH, NOT_ENABLED, ACTION_NOT_FOUND, ERROR };
    public enum GameStatus { PLAYING, LOOSE, WIN, QUIT, QUIT_SAVE };
    public class Engine
    {
        public GameStatus actualGameStatus = GameStatus.PLAYING;

        public Library lib = new Library();
        public Party party = new Party();
        public List<InvertoryItem> gameItems = new List<InvertoryItem>();
        public Engine()
        {
            Console.Write("Initializing engine:");
            //lib.LoadDataFiles();
            //party.actualRoomID = lib.gameInfo.startRoom;
        }

        public void PrepareNewGame(bool prepareCharacter)
        {
            lib.CleanAll();         // Erase all data in the memory
            lib.LoadDataFilesFromFolder("data");    // Load game data
            lib.LoadDataFilesFromFolder("maps");    // Load game maps

            // Prepare default character (party)
            party.Clean();
            if (prepareCharacter)
            {
                Character hero = new Character(true);
                party.members.Add(hero);
            }
            party.actualRoomID = lib.gameInfo.startRoom;
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
        public bool DoTest(CharAttr inAttribute, int testLevel)
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
            gi.itemSay = itm.say;
            gi.itemType = itm.type;
        }

        public void DescribeRoom()
        {
            Room lRoom = lib.GetRoom(party.actualRoomID);
            if (lRoom!=null)
            {
                // Hlavny opis miestnosti
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(lib.GetTextBlock(lRoom.name));
                Console.ResetColor();

                Print pr = new Print(lib.GetTextBlock(lRoom.desc));
                pr.Render();
                Console.WriteLine("");

                // Opisat, ci su tu nejake static predmety
                foreach (GameItem git in lib.gameItems)
                {
                    if (git.position==party.actualRoomID)
                    {
                        //Console.WriteLine("{0}", git.hidden.ToString());
                        if (!git.hidden)
                        {
                            Console.Write("Vidis tu ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(lib.GetTextBlock(git.itemSay).ToLower());
                            Console.ResetColor();
                            Console.WriteLine(".");
                        }
                    }
                }

                // Opisat, ci su tu nejake NPC postavy
                foreach (NPC tmpNPC in lib.NPCs)
                {
                    if (tmpNPC.position==party.actualRoomID)
                    {
                        // Console.WriteLine("{0}", git.hidden.ToString());
                        if (tmpNPC.alive)
                        {
                            Console.Write("Stoji tu ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(lib.GetTextBlock(tmpNPC.name));
                            //Console.Write(" ({0})",tmpNPC.friendliness.ToString());
                            Console.ResetColor();
                            Console.WriteLine(".");
                        }
                    }
                }
             
                // Opisat, ci su tu nejake ENEMIES GRUPY
                if (lRoom.enemyGroup!="")
                {
                    EnemyGroup leg = lib.GetEnemyGroup(lRoom.enemyGroup);
                    Console.Write("Je tu este niekto dalsi: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(lib.GetTextBlock(leg.name_group));
                    Console.ResetColor();
                    Console.WriteLine(".");
                }

                // Opisat moznosti cestovania:
                Console.Write("Mozes ist: ");
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (Road rd in lib.roads)
                {
                    if (rd.enabled)
                    {
                        if (rd.sourceRoom==party.actualRoomID)
                        {
                            if ((rd.bothWay == Direction.BOTH) ||  (rd.bothWay == Direction.TO_TARGET))
                            {                           
                                Console.Write("{0} ", Road.GetPathName(rd.direction1));
                            }
                        }
                        if (rd.targetRoom==party.actualRoomID)
                        {
                            if ((rd.bothWay == Direction.BOTH) || (rd.bothWay == Direction.TO_SOURCE))
                            {
                                Console.Write("{0} ", Road.GetPathName(rd.direction2));
                            }
                        }
                    }
                }
                Console.ResetColor();
                Console.WriteLine("\n");

                // Ak sú tu NPCcka, tak chcu nas pozdravit?
                foreach (NPC tmpNPC in lib.NPCs)
                {
                    if (tmpNPC.position==party.actualRoomID)
                    {
                        // Console.WriteLine("{0}", git.hidden.ToString());
                        if (tmpNPC.alive)
                        {
                            if (!tmpNPC.alreadyGreet)
                            {
                                Console.Write("Len čo sa priblížiš k {0}, prehovorí:\n", lib.GetTextBlock(tmpNPC.name));
                                pr.SetMessage(lib.GetTextBlock(tmpNPC.greeting));
                                pr.Render();
                                tmpNPC.alreadyGreet = true;
                                Console.WriteLine("");
                            }
                        }
                    }
                }
            }
            else Console.WriteLine("Engine error: Unable to find room '{0}'", party.actualRoomID);
        }

/*
        public void Print(string msg)
        {
            if (msg.Length>0)
            {
                string[] words = msg.Split(" ");
                int actualWord = 0;
                string s = "";
                string finalLine = "";

                bool mameVetu = false;
                // 1. ak sa da, vezmi slovo
                if (actualWord<=words.Length)
                {
                    if (s!="") s = s + " ";
                    s = s + words[actualWord];
                }

                // 2. mame uz vetu?
                if (s>80) mameVetu = true;


            }
            Console.WriteLine(Tools.RemoveQuotes(msg));
        } 
*/
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
                        //Console.WriteLine(lib.texts[words[1]]);
                        Print pr = new Print(lib.texts[words[1]]);
                        pr.Render();
                    }
                    else
                    {
                        Console.WriteLine("Unable to find text block: '{0}'", words[1]);
                    }
                }
                else
                {
                    string msg = Tools.MergeString(words,1);
                    //Print(msg);
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
                Console.WriteLine("Ziskavas '{0}'!", lib.GetTextBlock(gmi.itemSay).ToLower());
            }

            // Enable target action
            if (words[0]=="enable_action")
            {
                SpecialAction act = lib.GetAction(words[1]);
                if (act!=null)
                {
                    act.enabled = true;
                }
            }

            // Disable target action
            if (words[0]=="disable_action")
            {
                SpecialAction act = lib.GetAction(words[1]);
                if (act!=null)
                {
                    act.enabled = false;
                }
            }

            // Move player to target destination
            if (words[0]=="teleport")
            {
                party.actualRoomID = words[1];
                DescribeRoom();
            }

            // Decrease friendliness of target group
            if (words[0]=="decrease_group_friendliness")
            {
                // group_name = words[1];
                EnemyGroup leg = lib.GetEnemyGroup(words[1]);

                leg.friendliness--;
                if (leg.friendliness<-2) leg.friendliness=-2;
            }

            // Decrease target NPC friendliness
            if (words[0]=="decrease_npc_friendliness")
            {
                // npc_name = words[1];
                NPC lnpc = lib.GetNPC(words[1]);
                
                lnpc.friendliness--;
                if (lnpc.friendliness<-2) lnpc.friendliness=-2;
            }

            // Increase friendliness of target group
            if (words[0]=="increase_group_friendliness")
            {
                // group_name = words[1];
                EnemyGroup leg = lib.GetEnemyGroup(words[1]);

                leg.friendliness++;
                if (leg.friendliness>1) leg.friendliness=1;
            }

            // Increase target NPC friendliness
            if (words[0]=="increase_npc_friendliness")
            {
                // npc_name = words[1];
                NPC lnpc = lib.GetNPC(words[1]);
                
                lnpc.friendliness++;
                if (lnpc.friendliness>1) lnpc.friendliness=1;
            }

            // Make target NPC angry
            if (words[0]=="make_npc_angry")
            {
                // npc_name = words[1];
                NPC lnpc = lib.GetNPC(words[1]);              
                lnpc.friendliness=-2;
            }

            // Make target enemy group angry
            if (words[0]=="make_group_angry")
            {
                // enemy group_name = words[1];
                EnemyGroup eg = lib.GetEnemyGroup(words[1]);              
                eg.friendliness=-2;
            }

            // Make target enemy group angry
            if (words[0]=="attack_enemy_group")
            {
                Room actualRoom = lib.GetRoom(party.actualRoomID);

                // Decrease enemyGroup friendliness
                EnemyGroup eg = lib.GetEnemyGroup(actualRoom.enemyGroup);              
                eg.friendliness--;

                Combat combat = new Combat(lib, party, actualRoom);
                BattleStatus bs = combat.DoBattle();

                // Suboj prebehol a prehrali sme !!!
                if (bs==BattleStatus.LOOSE)
                {
                    Console.WriteLine("Prebehol suboj a tvoja partia prehrala!");
                    Console.WriteLine("Stlac ENTER pre ukoncenie a vyhodnotenie");
                    Console.ReadLine();
                    actualGameStatus = GameStatus.LOOSE;
                }
                if (bs==BattleStatus.WIN)
                {
                    EraseEnemiesInActualRoom(); // Erase enemies in this room
                    Console.WriteLine("Prebehol suboj a tvoja partia vyhrala!");
                    Console.WriteLine("Stlac ENTER pre pokracovanie");
                    Console.ReadLine();
                    Console.Clear();
                    DescribeRoom();
                }
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
                EnemyGroup leg = lib.GetEnemyGroup(lRoom.enemyGroup);

                if (leg.friendliness<0)
                {
                    //Console.Write("Neriatel: ");
                    Console.Write("Prepad! Útočí na teba ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(lib.GetTextBlock(leg.name_group));
                    //Console.Write(" ({0})",leg.friendliness.ToString());
                    Console.ResetColor();
                    Console.WriteLine("!");

                    Console.WriteLine("Stlac ENTER pre zaciatok suboja");
                    Console.ReadLine();
                    Combat combat = new Combat(lib, party, lRoom);
                    bs = combat.DoBattle();

                    if (bs==BattleStatus.LOOSE)
                    {
                        Console.WriteLine("Prebehol suboj a tvoja partia prehrala!");
                        Console.WriteLine("Stlac ENTER pre ukoncenie a vyhodnotenie");
                        Console.ReadLine();
                        actualGameStatus = GameStatus.LOOSE;
                    }
                    if (bs==BattleStatus.WIN)
                    {
                        EraseEnemiesInActualRoom(); // Erase enemies in this room
                        Console.WriteLine("Prebehol suboj a tvoja partia vyhrala!");
                        Console.WriteLine("Stlac ENTER pre pokracovanie");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                //else Console.WriteLine("Friendlines: {0}. Not attacking.", leg.friendliness.ToString());
            }

            // Check for random encounter HERE

            return bs;
        }

        /// <summary>
        /// Returns list of rooms, that belongs to selected map
        /// </summary>
        /// <param name="mapId">Map, where rooms should be searched for</param>
        /// <returns>(Strings[]) list of rooms</returns>
        public string[] GetRoomsOfActualMap(string mapId)
        {
            List<string> listOfRooms = new List<string>();
            foreach (Room tmpRoom in lib.rooms)
            {
                if (tmpRoom.map==mapId) listOfRooms.Add(tmpRoom.id);
            }
            return listOfRooms.ToArray();
        }

        public string GetActualMapName(string roomId) => lib.GetRoom(roomId).map;

        /// <summary>
        /// This method moves with every NPC, when it is set to move.
        /// </summary>
        public void MoveNPCsOnActualMap()
        {
            string actualMap = GetActualMapName(party.actualRoomID);

            // Ak sú tu NPCcka, tak chcu nas pozdravit?
            foreach (NPC tmpNPC in lib.NPCs)
            {
                if (tmpNPC.alive)
                {
                    string NPCmapName=GetActualMapName(tmpNPC.position);
                    if (NPCmapName==actualMap)
                    {
                        Dices dc = new Dices();

                        // So now we know, that NPC is on desired map
                        // and we can make a move with it.
                        if (tmpNPC.movement==Movement.RANDOM_MAP)
                        {
                            // Easiest movement - random over a map
                            string[] lrooms = GetRoomsOfActualMap(actualMap);   // get list of avaiable rooms
                            int target = dc.ThrowDiceX(1,lrooms.Length)-1;        // pick a random number
                            tmpNPC.position = lrooms[target];                   // change position to random room
                        }
                    }
                }
            }
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
           
            // If movement was done correctly, we can move NPC characters...
            if (ec==ErrorCode.OK) MoveNPCsOnActualMap();

            return ec;
        }
    }
}