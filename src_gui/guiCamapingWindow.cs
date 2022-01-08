using System;
using System.IO;
using System.Collections.Generic;

using LegendTools;
using LegendEngine;
using LegendLibrary;
using Ascidraw;

namespace Legend
{
    /*
    public class AdventureMapData
    {
        public string name, filename;
        public string guildId, startRoom;

    }
    */

    public class GuiCampaignWindow
    {
        private Engine eng;
        public GuiCampaignWindow(Engine inEngine)
        {
            eng = inEngine;
            eng.GiveItemToPlayer("cestovatelske_oblecenie", true);
            eng.GiveItemToPlayer("dyka", true);

            
        }

        public AdventureInfo[] LoadMapDescriptions(int characterLevel)
        {
            // Search for *lm files (legend map)
            string[] files = Directory.GetFiles("maps","*.lm");

            List<AdventureInfo> adventureData = new List<AdventureInfo>();
            AdventureInfo tmpData = null;

            // Load map file(s)
            foreach( string fname in files)
            {
                //Console.WriteLine("Loading LM file: " + fname);
                
                bool infoBlock = false;
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

                                if (infoBlock)
                                {
                                    if (words[0]=="name") tmpData.mapName = Tools.RemoveQuotes(Tools.MergeString(words,1));
                                    if (words[0]=="start_room") tmpData.startLocation = words[1];
                                    if (words[0]=="type") 
                                    {
                                        if (words[1]=="interior") tmpData.exterior = false; else tmpData.exterior = true;
                                    }
                                    if (words[0]=="end")
                                    {
                                        infoBlock=false;
                                        adventureData.Add(tmpData);
                                    }
                                }
                                if (!infoBlock)
                                {
                                    if (words[0].ToLower()=="mapinfo")
                                    {
                                        infoBlock=true;
                                        tmpData = new AdventureInfo();
                                        tmpData.fileName = fname;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return adventureData.ToArray();
        }

        public int ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Vitaj v hlavnom meste, Aldis!");
            Console.WriteLine("V tomto meste je kopa príležitostí, čo robiť!");
            Console.WriteLine("Vyber si, čo ďalej:\n");

            AdventureInfo[] data = LoadMapDescriptions(eng.party.members[0].level);
            int a;
            for (a=1;a<=data.Length;a++)
            {
                Console.WriteLine("{0}. {1}", a.ToString(), data[a-1].mapName);
            }
            Console.WriteLine("{0}. KONEC HRY", (a+1).ToString());
            return a+1;
        }

        public void Show()
        {
            GuiMainWin gameWin = new GuiMainWin(eng);
            GameStatus actualGameStatus = GameStatus.PLAYING;

            do
            {
                // Run initial dungeon      
                actualGameStatus = gameWin.Show();

                if (actualGameStatus == GameStatus.WIN)
                {
                    // Show win message + statistics
                    actualGameStatus = GameStatus.PLAYING;             
                }

                // If not playing, than quit
                if (actualGameStatus == GameStatus.LOOSE)
                {
                    // Show loose message + statistics 
                    Console.Clear();
                    Console.WriteLine("Prehral si...");
                    Console.ReadLine();             
                }

                if (actualGameStatus == GameStatus.QUIT)
                {
                    // Save game + message       
                }

                if (actualGameStatus == GameStatus.PLAYING)
                {
                    // If still playing, SHOW desc + campaigns
                    int ic = ShowMenu();
                    int x = Textutils.GetNumberRange(1,ic);

                    if (ic==x) actualGameStatus = GameStatus.QUIT;
                }

            }
            while (actualGameStatus==GameStatus.PLAYING);
            
        }
    }
}