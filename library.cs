using System;
using System.IO;
using System.Collections.Generic;

namespace legend
{
    public enum Block { NONE, ROOM, ROAD, TEXT, ASSET};
    public class Library
    {
        public List<Room> rooms = new List<Room>();
        public List<Road> roads = new List<Road>();
        public List<Item> items = new List<Item>();
        public List<Asset> assets = new List<Asset>();
        public Dictionary<string,string> texts = new Dictionary<string,string>();

        public int LoadData()
        {
            /*
            // Rooms, map 1
            Room lRoom = new Room();
            lRoom.id = "1";
            lRoom.name = "Brana k domu";
            lRoom.desc = "Stojis pri hlavnej brane.\nJe to brana";
            rooms.Add(lRoom);

            lRoom = new Room();
            lRoom.id = "2";
            lRoom.name = "Vstupna hala";
            lRoom.desc = "Si uprostred vstupnej haly. Chodba je dlha\na plna roznych gotickych soch.";
            rooms.Add(lRoom);

            Road lRoad = new Road("1","2",Path.NORTH);  // Spojime obe miestnosti
            roads.Add(lRoad);
            */

            Item lItem = new Item("dyka",ItemType.WHEAPON);
            items.Add(lItem);

            lItem = new Item("mec",ItemType.WHEAPON);
            items.Add(lItem);

            return 0;
        }

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
            
            int roomsCount = 0;
            int roadsCount = 0;
            int textCount = 0;

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
                            }
                        }
                    }
                }
            }
        
            Console.WriteLine("Rooms loaded: {0}", roomsCount.ToString());
            Console.WriteLine("Roads loaded: {0}", roadsCount.ToString());
            Console.WriteLine("Text blocks loaded: {0}", textCount.ToString());
        }

        public void LoadDataFiles()
        {
            // Search for *lm files (legend map)
            string[] files = Directory.GetFiles("maps","*.lm");

            foreach( string fname in files)
            {
                Console.WriteLine(fname);
                LoadLMFile(fname);
            }
        }
    }
}