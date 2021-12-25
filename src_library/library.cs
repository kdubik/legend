using System;
using System.IO;
using System.Collections.Generic;

using LegendTools;

namespace LegendLibrary
{
    public enum Block { NONE, ROOM, ROAD, TEXT, ACTION, ENEMY, NPC, ENEMYGROUP, ITEM, GAMEINFO, ADVENTUREINFO };
    public class Library
    {
        // Global info about whole game
        public GameInfo gameInfo = new GameInfo();

        // Info about actual map (adventure)
        public AdventureInfo adventureInfo = new AdventureInfo();

        // Actual map (adventure) content
        public List<Room> rooms = new List<Room>();
        public List<Road> roads = new List<Road>();
        public List<Item> items = new List<Item>();
        public List<GameItem> gameItems = new List<GameItem>();
        public List<SpecialAction> actions = new List<SpecialAction>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<NPC> NPCs = new List<NPC>();
        public List<EnemyGroup> enemyGroups = new List<EnemyGroup>();
        public Dictionary<string,string> texts = new Dictionary<string,string>();

        int ACTBcount = 0;   // Automatically created text block counter

        /// <summary>
        /// Clean all gane databases.
        /// </summary>
        public void CleanAll()
        {
            rooms.Clear();
            roads.Clear();
            items.Clear();
            gameItems.Clear();
            actions.Clear();
            enemies.Clear();
            NPCs.Clear();
            enemyGroups.Clear();
            texts.Clear();
        }

        /// <summary>
        /// Find Room in database and returns object, that coresponds to searched id.
        /// </summary>
        /// <param name="roomId">id of object, that we want to find</param>
        /// <returns>searched object</returns>
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

        public string GetRoomName(string roomId)
        {
            string ret = "error";
            foreach (Room rm in rooms)
            {
                if (rm.id==roomId)
                {
                    texts.TryGetValue(rm.name,out ret);
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Find Item in database and returns object, that coresponds to searched id.
        /// </summary>
        /// <param name="id">id of object, that we want to find</param>
        /// <returns>searched object</returns>
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
     
        /// <summary>
        /// Find Enemy in database and returns object, that coresponds to searched id.
        /// </summary>
        /// <param name="id">id of object, that we want to find</param>
        /// <returns>searched object</returns>
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

        /// <summary>
        /// Find Text block in database and returns object, that coresponds to searched id.
        /// </summary>
        /// <param name="id">id of object, that we want to find</param>
        /// <returns>searched object</returns>
        public string GetTextBlock(string id)
        {
            string res = "";
            if (!texts.TryGetValue(id,out res))
                Console.Write("Could not find text block id: {0}", id);
            return res;
        }  

        /// <summary>
        /// Find Enemy group in database and returns object, that coresponds to searched id.
        /// </summary>
        /// <param name="id">id of object, that we want to find</param>
        /// <returns>searched object</returns>
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
     
        /// <summary>
        /// Find Game item in database and returns object, that coresponds to searched id.
        /// </summary>
        /// <param name="gameItemId">id of object, that we want to find</param>
        /// <returns>searched object</returns>
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

        public SpecialAction GetAction(string actionId)
        {
            SpecialAction ret = null;
            foreach (SpecialAction ac in actions)
            {
                if (ac.id==actionId)
                {
                    ret = ac;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Find Road in database and returns object.
        /// </summary>
        /// <param name="sourceRoom">actual room id</param>
        /// <param name="pathway">direction we want to look at</param>
        /// <returns>searched object</returns>
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

        /// <summary>
        /// Find NPC in database and returns object, that coresponds to searched id.
        /// </summary>
        /// <param name="id">id of object, that we want to find</param>
        /// <returns>searched object</returns>
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
        /// <returns>searched object</returns>
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
        string ReviewString(string msg)
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

        /// <summary>
        /// Get string used for "action". During "Load LM" file, when "action" chunk is beging
        /// loaded, string can be present after "succes" and "fail" keywords.
        /// This method correctly creates "text block" if neccessary.
        /// </summary>
        /// <param name="msg">input string, which should be analysed/param>
        /// <returns>id of text block, where "text" is now stored</returns>
        string AnalyzeAction(string msg)
        {
            string[] lines = msg.Split(' ');

            Console.WriteLine("MSG: {0}",msg);
            Console.WriteLine("first WORD {0}",lines[0]);
            if ((lines[0]=="show_msg") || (lines[0]=="show_msg_wait"))
            {
                string tmp = Tools.MergeString(lines,1);
                Console.WriteLine("TMP created: {0}",tmp);
                msg = lines[0] + " " + ReviewString(tmp);
                Console.WriteLine("New MSG: {0}",msg);
            }

            return msg;
        }

        /// <summary>
        /// After LM file is loaded, we need to add NPCs and items into
        /// desired locations. This have to be done after items/NPC etc.
        /// are loaded, so after LM file is loaded.
        /// </summary>
        /// <param name="postLoadJobs">List(string) of taks (addItem, addNPC...)</param>
        /// <returns>Number of processed items from the list</returns>
        int DoPostLoadJobs(List<string> postLoadJobs)
        {
            int res = 0;

            foreach (string job in postLoadJobs)
            {
                Console.WriteLine(job);
                string[] words = job.Split(" ");

                string id = words[2];
                string targetRoom = words[1];
                Console.WriteLine("target, 1 - {0} ",targetRoom);
                Console.WriteLine("id, 2 - {0} ",id);
                
                if (words[0]=="addNPC")
                {
                    // Add NPC into target room
                    NPC tn = GetNPC(id);
                    tn.position = targetRoom;
                    res++;
                }

                if (words[0]=="addItem")
                {
                    // Add item into target room
                    Item tmpItem = GetItem(id);
                    if (tmpItem==null)
                    {
                        Console.WriteLine(" - Error: Unable to find object '{0}'!", words[1]);
                    }
                    else
                    {
                        GameItem tmpGameItem = new GameItem(id,targetRoom,tmpItem);

                        int paramsCount = words.Length - 3;
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
                        res++;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Load data from standard LM data file.
        /// </summary>
        /// <param name="fname">(Path and) name of file, which would be loaded</param>
        public void LoadLMFile(string fname)
        {
            List<string> postLoadJobs = new List<string>();

            Block blok = Block.NONE;
            Room tmpRoom = null;
            Road tmpRoad = null;
            SpecialAction tmpAct = null;
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
            string tmpString = "";

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
                                    string tmpStr = "addNPC" + " " + tmpID + " " + words[1];
                                    postLoadJobs.Add(tmpStr);
                                }

                                if (words[0]=="add_group")
                                {
                                    // Add group of enemies into this room
                                    tmpRoom.enemyGroup = words[1];
                                }

                                if (words[0]=="add_item")
                                {
                                    string tmpStr = "addItem" + " " + tmpID + " " + Tools.MergeString(words,1);
                                    postLoadJobs.Add(tmpStr);
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
                    
                                if (words[0]=="attribute")
                                {
                                    CharAttr ta = Character.GetAttributeFromString(words[1]);
                                    tmpEnemy.SetAttribute(ta,int.Parse(words[2]));
                                }
                                if (words[0]=="wheapon")
                                {
                                    int an = int.Parse(words[1]);   // Attack bonus
                                    CharAttr ta = Character.GetAttributeFromString(words[3]);  // Tested attribute
                                    string nm = ReviewString(Tools.MergeString(words,4)); // Name
                                    tmpEnemy.AddWheapon(nm, an, words[2],ta);  // All data + DMG
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
                                if (words[0]=="friendliness") tmpNPC.friendliness = int.Parse(words[1]);

                                if (words[0]=="desc") tmpNPC.desc = Tools.MergeString(words,1);
                                if (words[0]=="attribute")
                                {
                                    CharAttr ta = Character.GetAttributeFromString(words[1]);
                                    tmpNPC.SetAttribute(ta,int.Parse(words[2]));
                                }
                                if (words[0]=="wheapon")
                                {
                                    int an = int.Parse(words[1]);   // Attack bonus
                                    CharAttr ta = Character.GetAttributeFromString(words[3]);  // Tested attribute
                                    string nm = ReviewString(Tools.MergeString(words,4)); // Name
                                    tmpNPC.AddWheapon(nm, an, words[2],ta);  // All data + DMG
                                }
                                if (words[0]=="greeting") tmpNPC.greeting = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="end")
                                {
                                    NPCs.Add(tmpNPC);
                                    NPC_count++;
                                    blok=Block.NONE;
                                }
                            }

                            if (blok==Block.ENEMYGROUP)
                            {
                                if (words[0]=="name_group") tmpGroup.name_group = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="name_attack") tmpGroup.name_attack = ReviewString(Tools.MergeString(words,1));
                                if (words[0]=="enemy") tmpGroup.AddEnemy(words[1], int.Parse(words[2]));
                                if (words[0]=="treasure") tmpGroup.AddTreasure(words[1]);
                                if (words[0]=="friendliness") tmpGroup.friendliness = int.Parse(words[1]);
                                if (words[0]=="end")
                                {
                                    enemyGroups.Add(tmpGroup);
                                    group_count++;
                                    blok=Block.NONE;
                                }
                            }

                            if (blok==Block.GAMEINFO)
                            {
                                if (words[0]=="name") gameInfo.name = Tools.RemoveQuotes(Tools.MergeString(words,1));
                                if (words[0]=="author") gameInfo.author = Tools.RemoveQuotes(Tools.MergeString(words,1));
                                if (words[0]=="start_map") gameInfo.startMap = words[1];
                                if (words[0]=="start_room") gameInfo.startRoom = words[1];
                                if (words[0]=="game_version") gameInfo.gameVersion = words[1];
                                if (words[0]=="engine_version") gameInfo.engineVersion = words[1];
                                if (words[0]=="game_desc") gameInfo.gameDesc = Tools.RemoveQuotes(Tools.MergeString(words,1));
                                if (words[0]=="game_intro") gameInfo.gameIntro = Tools.RemoveQuotes(Tools.MergeString(words,1));
                                if (words[0]=="end")
                                {
                                    gameInfoLoaded = true;
                                    blok=Block.NONE;
                                }
                            }

                            if (blok==Block.ADVENTUREINFO)
                            {
                                if (words[0]=="name") adventureInfo.mapName = Tools.RemoveQuotes(Tools.MergeString(words,1));
                                if (words[0]=="start_room") adventureInfo.startLocation = words[1];
                                if (words[0]=="target_room") adventureInfo.targetLocation = words[1];
                                if (words[0]=="type") 
                                {
                                    if (words[1]=="interior") adventureInfo.exterior = false; else adventureInfo.exterior = true;
                                }
                                if (words[0]=="end")
                                {
                                    blok=Block.NONE;
                                    adventureInfo.id = tmpID;
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
                                if (words[0]=="name") 
                                {
                                    tmpItem.name = ReviewString(Tools.MergeString(words,1));
                                    if (tmpItem.say=="") tmpItem.say = tmpItem.name;
                                }
                                if (words[0]=="say") tmpItem.say = ReviewString(Tools.MergeString(words,1));
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
                                if (words[0]!="end")
                                {
                                    if (tmpString!="") tmpString = tmpString + " ";
                                    tmpString += s;
                                }
                                else
                                {
                                    texts.Add(tmpID,tmpString);
                                    textCount++;
                                    blok=Block.NONE;
                                    tmpString = "";
                                }
                            }

                            if (blok==Block.NONE)
                            {
                                if (words[0].ToLower()=="game")
                                {
                                    blok = Block.GAMEINFO;
                                }

                                if (words[0].ToLower()=="map")
                                {
                                    blok = Block.ADVENTUREINFO;
                                    tmpID = words[1];
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
                                    tmpAct = new SpecialAction(words[1],words[2], at);
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
        
            int jobsDone = DoPostLoadJobs(postLoadJobs);

            using (StreamWriter log = new StreamWriter("lmloader.log",true))
            {
                log.WriteLine("Loaded file: {0}",fname);
                log.WriteLine("------------------\n");
                log.WriteLine("General: ");
                log.WriteLine(" - main game information loaded: {0}", gameInfoLoaded.ToString());
                log.WriteLine(" - post load jobs done: {0}", jobsDone.ToString());

                log.WriteLine("Objects loaded: ");
                log.WriteLine(" - Rooms loaded: {0}", roomsCount.ToString());
                log.WriteLine(" - Roads loaded: {0}", roadsCount.ToString());
                log.WriteLine(" - Text blocks loaded: {0}", textCount.ToString());
                log.WriteLine(" - Actions loaded: {0}", actionCount.ToString());
                log.WriteLine(" - Enemies loaded: {0}", enemiesCount.ToString());
                log.WriteLine(" - NPCs loaded: {0}", NPC_count.ToString());
                log.WriteLine(" - EnemyGroups loaded: {0}", group_count.ToString());
                log.WriteLine(" - Items loaded: {0}", item_count.ToString());

                log.WriteLine("\nItems loaded: ");
                log.WriteLine(" - Wheapons loaded: {0}", wheaponCount.ToString());
                log.WriteLine(" - Armors loaded: {0}", armorCount.ToString());
                log.WriteLine(" - Shields loaded: {0}", shieldCount.ToString());
                log.WriteLine(" - Assets loaded: {0}", assetCount.ToString()); 
                log.WriteLine(" - Miscs loaded: {0}", miscCount.ToString());

                // List of NPCs
                log.WriteLine("\nNPCs loaded: ");
                foreach (NPC en in NPCs)
                {
                    log.WriteLine("-> {0}",en.name);
                }
            }
        }

        /// <summary>
        /// Load data from "maps" folder, where LM data files are stored.
        /// </summary>
        public void LoadDataFilesFromFolder(string folderName)
        {
            // Search for *lm files (legend map)
            string[] files = Directory.GetFiles(folderName,"*.lm");

            // Erase log file
            //File.Delete("lmloader.log");

            // Load map file(s)
            foreach( string fname in files)
            {
                Console.WriteLine("Loading LM file: " + fname);
                LoadLMFile(fname);
            }
        }
    
        /// <summary>
        /// Load data from standard LM data file, that are stored in "config" folder.
        /// These are LM/config files, which are loaded only once, during application starts
        /// </summary>
        public void LoadConfigFiles()
        {
            // Search for *lm files (legend map)
            string[] files = Directory.GetFiles("maps","*.lm");

            // Erase log file
            File.Delete("lmloader.log");

            // Load config file(s)
            Console.WriteLine("Loading config file: ");
            LoadLMFile("config/main.lm");
        }
    }
}