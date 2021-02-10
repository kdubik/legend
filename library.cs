using System;
using System.IO;
using System.Collections.Generic;

namespace legend
{
    public enum Block { NONE, ROOM, ROAD, TEXT, ACTION, ENEMY, NPC, ENEMYGROUP, ITEM, GAMEINFO };
    public class Library
    {
        public GameInfo gameInfo = new GameInfo();
        public List<Room> rooms = new List<Room>();
        public List<Road> roads = new List<Road>();
        public List<Item> items = new List<Item>();
        public List<GameItem> gameItems = new List<GameItem>();
        public List<Action> actions = new List<Action>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<NPC> NPCs = new List<NPC>();
        public List<EnemyGroup> enemyGroups = new List<EnemyGroup>();

        public Dictionary<string,string> texts = new Dictionary<string,string>();

        int ACTBcount = 0;   // Automatically created text block counter

        public Room GetRoom(string roomId)
        {
            Room ret = null;
            foreach (Room rm in rooms)
            {
                if (rm.id==roomId)
                {
                    ret = rm;
                    break;
                }
            }
            return ret;
        }

        public Item GetItem(string itemId)
        {
            Item ret = null;
            foreach (Item rm in items)
            {
                if (rm.id==itemId)
                {
                    ret = rm;
                    break;
                }
            }
            return ret;
        }
     
        public Enemy GetEnemy(string id)
        {
            Enemy ret = null;
            foreach (Enemy eg in enemies)
            {
                if (eg.id==id)
                {
                    ret = eg;
                    break;
                }
            }
            return ret;
        }

        public string GetTextBlock(string id)
        {
            string res = "";
            if (!texts.TryGetValue(id,out res))
                Console.Write("Could not find text block id: {0}", id);
            return res;
        }  

        public EnemyGroup GetEnemyGroup(string id)
        {
            EnemyGroup ret = null;
            foreach (EnemyGroup eg in enemyGroups)
            {
                if (eg.id==id)
                {
                    ret = eg;
                    break;
                }
            }
            return ret;
        }
     
        public GameItem GetGameItem(string gameItemId)
        {
            GameItem ret = null;
            foreach (GameItem gm in gameItems)
            {
                if (gm.id==gameItemId)
                {
                    ret = gm;
                    break;
                }
            }
            return ret;
        }

        public Action GetAction(string actionId)
        {
            Action ret = null;
            foreach (Action ac in actions)
            {
                if (ac.id==actionId)
                {
                    ret = ac;
                    break;
                }
            }
            return ret;
        }
        public Road GetRoad(string sourceRoom, Path pathway)
        {
            Road ret = null;

            foreach (Road rm in roads)
            {
                if (((rm.sourceRoom==sourceRoom) && (rm.direction1==pathway)) ||
                ((rm.targetRoom==sourceRoom) && ( Road.GetOpositePath(rm.direction1)==pathway)))
                {
                    ret = rm;
                    break;
                }
            }

            return ret;
        }

        public NPC GetNPC(string id)
        {
            NPC ret = null;
            foreach (NPC ac in NPCs)
            {
                if (ac.id==id)
                {
                    ret = ac;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// After analyze of input id, returns, whether this id belongs to item, or NPC
        /// </summary>
        /// <param name="id">id of targer</param>
        /// <returns></returns>
        public ActionTarget DecideActionTarget(string id)
        {
            // By default we expect, that target is ITEM. Then, we search in
            // NPC database (there is lesser number of NPCs than items, so it
            // would be faster), whether this belongs rather to NPC
            ActionTarget ret = ActionTarget.ITEM;

            foreach (NPC ac in NPCs)
            {
                if (ac.id==id)
                {
                    ret = ActionTarget.NPC;
                    break;
                }
            }

            return ret;
        }
        
        /// <summary>
        /// Tato funkcia overi, ci je zadany text ID textoveho bloku, alebo
        /// priamo text v uvodzovkach. Ak je to priamo text, tak ho zbavi
        /// uvodzovek a vyrobi novy text blok, kam ten text napise.
        /// V kazdom pripade vrati ID textoveho bloku.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>Text blok ID</returns>
        public string ReviewString(string msg)
        {
            if (Tools.CheckForQuotes(msg))
            {    
                ACTBcount++;    // Pocet vygenerovanych text blokov stupa
                string textBlockKey = "ACTB_" + ACTBcount.ToString();    // Meno pre novy textblok
                msg = Tools.RemoveQuotes(msg);  // Vygenerujeme text pre novy textblok
                texts.Add(textBlockKey,msg);    // Pridame novy text do zoznamu textov
                msg = textBlockKey; // vratime ID textoveho bloku
            }
            return msg;
        }

        public string AnalyzeAction(string msg)
        {
            string[] lines = msg.Split(' ');

            if ((lines[1]=="show_msg") || (lines[1]=="show_msg_wait"))
            {
                string tmp = Tools.MergeString(lines,2);
                msg = ReviewString(tmp);
            }

            return msg;
        }


        public void LoadLMFile(string fname)
        {
            Block blok = Block.NONE;
            Room tmpRoom = null;
            Road tmpRoad = null;
            Action tmpAct = null;
            Enemy tmpEnemy = null;
            NPC tmpNPC = null;
            EnemyGroup tmpGroup = null;
            Item tmpItem = null;
            
            bool gameInfoLoaded = false;
            int roomsCount = 0;
            int roadsCount = 0;
            int textCount = 0;
            int actionCount = 0;
            int enemiesCount = 0;
            int NPC_count = 0;
            int group_count = 0;
            int item_count = 0;

            int wheaponCount = 0;
            int armorCount = 0;
            int shieldCount = 0;
            int assetCount = 0;
            int miscCount = 0;

            string tmpID = "";

            // Open the stream and read it back.
            using (StreamReader sr = File.OpenText(fname))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    if (s!="")  // No empty string
                    {
                        if (s[0]!='#')  // No comments
                        {
                            string[] words = s.Split(" ");
                            //Console.WriteLine(s);
                            //Console.WriteLine(words.Length.ToString());
                            if (blok==Block.ROOM)
                            {
                                if (words[0]=="name") tmpRoom.name = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="map") tmpRoom.map = words[1];
                                if (words[0]=="desc") tmpRoom.desc = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="water") tmpRoom.water = false;
                                if (words[0]=="teleport") tmpRoom.teleport = false;
                                if (words[0]=="end")
                                {
                                    rooms.Add(tmpRoom);
                                    roomsCount++;
                                    blok=Block.NONE;
                                }
                                
                                if (words[0]=="add_NPC")
                                {
                                    // Add NPC into this room
                                    NPC tn = GetNPC(words[1]);
                                    tn.position = tmpID;
                                }

                                if (words[0]=="add_group")
                                {
                                    // Add group of enemies into this room
                                    tmpRoom.enemyGroup = words[1];
                                }

                                if (words[0]=="add_item")
                                {
                                    // Add item into this room
                                    GameItem tmpGameItem = new GameItem(words[1],tmpRoom.id);
                                    tmpItem = GetItem(words[1]);
                                    if (tmpItem==null)
                                    {
                                        Console.WriteLine(" - Error: Unable to find object '{0}'!", words[1]);
                                    }
                                    else
                                    {
                                        tmpGameItem.itemType = tmpItem.type;
                                        tmpGameItem.itemName = tmpItem.name;

                                        int paramsCount = words.Length - 2;
                                        if (paramsCount>0)
                                        {
                                            for (int a=0;a<paramsCount;a++)
                                            {
                                                string[] data = words[2+a].Split(":");
                                                if (data.Length!=1)
                                                {
                                                    if (data[0]=="trapped")
                                                    {
                                                        tmpGameItem.AddTrap(data[1]);
                                                    }
                                                    if (data[0]=="hidden")
                                                    {
                                                        tmpGameItem.AddHide(data[1]);
                                                    }
                                                } else Console.WriteLine("Error: syntax error during processing item {0}!",tmpItem.name);
                                            }
                                        }

                                        gameItems.Add(tmpGameItem);
                                    }
                                }
                            }

                            if (blok==Block.ENEMY)
                            {
                                if (words[0]=="desc") tmpEnemy.desc = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="name") tmpEnemy.name = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="speed") tmpEnemy.speed = int.Parse(words[1]);
                                if (words[0]=="health") tmpEnemy.health = int.Parse(words[1]);
                                if (words[0]=="defense") tmpEnemy.defense = int.Parse(words[1]);
                                if (words[0]=="armor") tmpEnemy.armor = int.Parse(words[1]);
                                if (words[0]=="wheapon")
                                {
                                    int an = int.Parse(words[1]);
                                    string nm = Tools.MergeString(words,3);
                                    tmpEnemy.AddWheapon(nm, an, words[2]);
                                }

                                if (words[0]=="end")
                                {
                                    enemies.Add(tmpEnemy);
                                    enemiesCount++;
                                    blok=Block.NONE;
                                }

                            }

                            if (blok==Block.NPC)
                            {
                                if (words[0]=="desc") tmpNPC.desc = words[1];
                                if (words[0]=="name") tmpNPC.name = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="speed") tmpNPC.speed = int.Parse(words[1]);
                                if (words[0]=="health") tmpNPC.health = int.Parse(words[1]);
                                if (words[0]=="defense") tmpNPC.defense = int.Parse(words[1]);
                                if (words[0]=="armor") tmpNPC.armor = int.Parse(words[1]);

                                if (words[0]=="desc") tmpNPC.desc = Tools.MergeString(words,1);
                                if (words[0]=="wheapon")
                                {
                                    int an = int.Parse(words[1]);
                                    string nm = Tools.MergeString(words,3);
                                    tmpNPC.AddWheapon(nm, an, words[2]);
                                }

                                if (words[0]=="end")
                                {
                                    NPCs.Add(tmpNPC);
                                    NPC_count++;
                                    blok=Block.NONE;
                                }
                            }

                            if (blok==Block.ENEMYGROUP)
                            {
                                if (words[0]=="name") tmpGroup.name = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="enemy") tmpGroup.AddEnemy(words[1], int.Parse(words[2]));
                                if (words[0]=="treasure") tmpGroup.AddTreasure(words[1]);
                                if (words[0]=="end")
                                {
                                    enemyGroups.Add(tmpGroup);
                                    group_count++;
                                    blok=Block.NONE;
                                }
                            }

                            if (blok==Block.GAMEINFO)
                            {
                                if (words[0]=="name") gameInfo.name = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="author") gameInfo.author = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="start_map") gameInfo.startMap = words[1];
                                if (words[0]=="start_room") gameInfo.startRoom = words[1];
                                if (words[0]=="end")
                                {
                                    gameInfoLoaded = true;
                                    blok=Block.NONE;
                                }
                            }

                            if (blok==Block.ROAD)
                            {
                                if (words[0]=="enabled")
                                {
                                    if (words[1]=="true") tmpRoad.enabled = true;
                                    if (words[1]=="false") tmpRoad.enabled = false;
                                } 
                                if (words[0]=="direction") 
                                {
                                    if (words[1]=="to_target") tmpRoad.bothWay = Direction.TO_TARGET;
                                    if (words[1]=="to_source") tmpRoad.bothWay = Direction.TO_SOURCE;
                                    if (words[1]=="both") tmpRoad.bothWay = Direction.BOTH;
                                }
                                if (words[0]=="end")
                                {
                                    roads.Add(tmpRoad);
                                    roadsCount++;
                                    blok=Block.NONE;
                                }
                            }
                    
                            if (blok==Block.ITEM)
                            {
                                if (words[0]=="name") tmpItem.name = ReviewString(Tools.MergeString(words,1));;
                                if (words[0]=="type") tmpItem.type = tmpItem.GetItemTypeFromString(words[1]);
                                if (words[0]=="value") tmpItem.value = int.Parse(words[1]);
                                if (words[0]=="weight") tmpItem.weight = int.Parse(words[1]);
                                if (words[0]=="attributes") tmpItem.AppendAttributes(words[1]);

                                if (words[0]=="end")
                                {
                                    // Create dictionary with item attributes
                                    tmpItem.ConverParamToDict();

                                    if (tmpItem.type == ItemType.WHEAPON) wheaponCount++;
                                    if (tmpItem.type == ItemType.ARMOR) armorCount++;
                                    if (tmpItem.type == ItemType.SHIELD) shieldCount++;
                                    if (tmpItem.type == ItemType.ASSET) assetCount++;
                                    if (tmpItem.type == ItemType.MISC) miscCount++;

                                    items.Add(tmpItem);
                                    item_count++;
                                    blok=Block.NONE;
                                }
                            }

                            if (blok==Block.ACTION)
                            {
                                if (words[0]=="enabled")
                                {
                                    if (words[1]=="true") tmpAct.enabled = true;
                                    if (words[1]=="false") tmpAct.enabled = false;
                                }
                                if (words[0]=="text") tmpAct.desc = ReviewString(Tools.MergeString(words,1));

                                if (words[0]=="attribute")
                                {
                                    tmpAct.attribute = Character.GetAttributeFromString(words[1]);
                                }
                                if (words[0]=="level")
                                {
                                    tmpAct.level = int.Parse(words[1]);
                                }
                                if (words[0]=="focus")
                                {
                                    tmpAct.focus = words[1].ToUpper();
                                }
                                if (words[0]=="success")
                                {
                                    tmpAct.successActions.Add(AnalyzeAction(Tools.MergeString(words,1)));
                                    //tmpAct.successActions.Add(Tools.MergeString(words,1));
                                }
                                if (words[0]=="fail")
                                {
                                    tmpAct.failedActions.Add(AnalyzeAction(Tools.MergeString(words,1)));
                                    //tmpAct.failedActions.Add(Tools.MergeString(words,1));
                                }
                                if (words[0]=="end")
                                {
                                    actions.Add(tmpAct);
                                    actionCount++;
                                    blok=Block.NONE;
                                }
                            }

                            if (blok==Block.TEXT)
                            {
                                texts.Add(tmpID,s);
                                textCount++;
                                blok=Block.NONE;
                            }

                            if (blok==Block.NONE)
                            {
                                if (words[0].ToLower()=="game")
                                {
                                    blok = Block.GAMEINFO;
                                }

                                if (words[0].ToLower()=="room")
                                {
                                    blok = Block.ROOM;
                                    tmpRoom = new Room(words[1]); 
                                    tmpID = words[1];                             
                                }
                                if (words[0].ToLower()=="path")
                                {
                                    blok = Block.ROAD;
                                    tmpRoad = new Road(words[1], words[2], Road.PathFromString(words[3]));                              
                                }
                                if (words[0].ToLower()=="text")
                                {
                                    blok = Block.TEXT;
                                    tmpID = words[1];                              
                                }
                                if (words[0].ToLower()=="item")
                                {
                                    blok = Block.ITEM;
                                    tmpItem = new Item(words[1]);                              
                                }
                                if (words[0].ToLower()=="action")
                                {
                                    blok = Block.ACTION;
                                    ActionTarget at = DecideActionTarget(words[1]);
                                    tmpAct = new Action(words[1],words[2], at);
                                    tmpAct.itemId = words[3];                             
                                }
                                if (words[0].ToLower()=="enemy")
                                {
                                    blok = Block.ENEMY;
                                    tmpEnemy = new Enemy(words[1]);                              
                                }
                                if (words[0].ToLower()=="npc")
                                {
                                    blok = Block.NPC;
                                    tmpNPC = new NPC(words[1]);                              
                                }
                                if (words[0].ToLower()=="group")
                                {
                                    blok = Block.ENEMYGROUP;
                                    tmpGroup = new EnemyGroup(words[1]);                               
                                }
                            }
                        }
                    }
                }
            }
        
            Console.WriteLine("General: ");
            Console.WriteLine(" - main game information loaded: {0}", gameInfoLoaded.ToString());
            Console.WriteLine("Objects loaded: ");
            Console.WriteLine(" - Rooms loaded: {0}", roomsCount.ToString());
            Console.WriteLine(" - Roads loaded: {0}", roadsCount.ToString());
            Console.WriteLine(" - Text blocks loaded: {0}", textCount.ToString());
            Console.WriteLine(" - Actions loaded: {0}", actionCount.ToString());
            Console.WriteLine(" - Enemies loaded: {0}", enemiesCount.ToString());
            Console.WriteLine(" - NPCs loaded: {0}", NPC_count.ToString());
            Console.WriteLine(" - EnemyGroups loaded: {0}", group_count.ToString());
            Console.WriteLine(" - Items loaded: {0}", item_count.ToString());

            Console.WriteLine("\nItems loaded: ");
            Console.WriteLine(" - Wheapons loaded: {0}", wheaponCount.ToString());
            Console.WriteLine(" - Armors loaded: {0}", armorCount.ToString());
            Console.WriteLine(" - Shields loaded: {0}", shieldCount.ToString());
            Console.WriteLine(" - Assets loaded: {0}", assetCount.ToString()); 
            Console.WriteLine(" - Miscs loaded: {0}", miscCount.ToString());

            /*
            foreach (NPC en in NPCs)
            {
                Console.WriteLine(en.name);
            }*/
        }

        public void LoadDataFiles()
        {
            // Search for *lm files (legend map)
            string[] files = Directory.GetFiles("maps","*.lm");

            // Load items first
            Console.WriteLine("Loading items:");

            // Load map file(s)
            foreach( string fname in files)
            {
                Console.WriteLine("Loading LM file: " + fname);
                LoadLMFile(fname);
            }
        }
    }
}