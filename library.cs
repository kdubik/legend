using System;
using System.IO;
using System.Collections.Generic;

namespace legend
{
    public enum Block { NONE, ROOM, ROAD, TEXT, ACTION };
    public class Library
    {
        public List<Room> rooms = new List<Room>();
        public List<Road> roads = new List<Road>();
        public List<Item> items = new List<Item>();
        public List<GameItem> gameItems = new List<GameItem>();
        public List<Action> actions = new List<Action>();

        public Dictionary<string,string> texts = new Dictionary<string,string>();

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

        public void LoadLMFile(string fname)
        {
            Block blok = Block.NONE;
            Room tmpRoom = null;
            Road tmpRoad = null;
            Action tmpAct = null;
            
            int roomsCount = 0;
            int roadsCount = 0;
            int textCount = 0;
            int actionCount = 0;

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
                                if (words[0]=="desc") tmpRoom.desc = Tools.MergeString(words,1);
                                // if (words[0]=="desc") tmpRoom.desc = words[1];
                                if (words[0]=="water") tmpRoom.water = false;
                                if (words[0]=="teleport") tmpRoom.teleport = false;
                                if (words[0]=="end")
                                {
                                    rooms.Add(tmpRoom);
                                    roomsCount++;
                                    blok=Block.NONE;
                                }
                                if (words[0]=="add_item")
                                {
                                    // Add item into this room
                                    GameItem tmpGameItem = new GameItem(words[1],tmpRoom.id);
                                    Item tmpItem = GetItem(words[1]);
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

                            if (blok==Block.ACTION)
                            {
                                if (words[0]=="enabled")
                                {
                                    if (words[1]=="true") tmpAct.enabled = true;
                                    if (words[1]=="false") tmpAct.enabled = false;
                                }
                                if (words[0]=="text") tmpAct.desc = Tools.MergeString(words,1);

                                if (words[0]=="atribute")
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
                                if (words[0]=="sucess")
                                {
                                    tmpAct.successActions.Add(Tools.MergeString(words,1));
                                }
                                if (words[0]=="fail")
                                {
                                    tmpAct.faliedActions.Add(Tools.MergeString(words,1));
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
                                if (words[0].ToLower()=="room")
                                {
                                    blok = Block.ROOM;
                                    tmpRoom = new Room(words[1]);                              
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
                                if (words[0].ToLower()=="action")
                                {
                                    blok = Block.ACTION;
                                    tmpAct = new Action(words[1],words[2]);
                                    tmpAct.id = words[3];                             
                                }
                            }
                        }
                    }
                }
            }
        
            Console.WriteLine(" - Rooms loaded: {0}", roomsCount.ToString());
            Console.WriteLine(" - Roads loaded: {0}", roadsCount.ToString());
            Console.WriteLine(" - Text blocks loaded: {0}", textCount.ToString());
            Console.WriteLine(" - Actions loaded: {0}", actionCount.ToString());
        }

        public void LoadItemFile(string fname)
        {
            int wheaponCount = 0;
            int armorCount = 0;
            int assetCount = 0;
            int miscCount = 0;

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
                            Item tmpItem = new Item(s);
                            items.Add(tmpItem);

                            if (tmpItem.type == ItemType.WHEAPON) wheaponCount++;
                            if (tmpItem.type == ItemType.ARMOR) armorCount++;
                            if (tmpItem.type == ItemType.ASSET) assetCount++;
                            if (tmpItem.type == ItemType.MISC) miscCount++;
                        }
                    }
                }
            }

            Console.WriteLine(" - Wheapons loaded: {0}", wheaponCount.ToString());
            Console.WriteLine(" - Armor loaded: {0}", armorCount.ToString());
            Console.WriteLine(" - Assets loaded: {0}", armorCount.ToString()); 
            Console.WriteLine(" - Misc loaded: {0}", miscCount.ToString());  
        }
        public void LoadDataFiles()
        {
            // Search for *lm files (legend map)
            string[] files = Directory.GetFiles("maps","*.lm");

            // Load items first
            Console.WriteLine("Loading items:");
            LoadItemFile(@"data/items.dat");

            // Load map file(s)
            foreach( string fname in files)
            {
                Console.WriteLine("Loading LM file: " + fname);
                LoadLMFile(fname);
            }
        }
    }
}