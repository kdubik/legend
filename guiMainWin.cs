using System;
using System.Collections.Generic;

namespace legend
{
    public class GuiMainWin
    {
        public Engine eng;
        public GuiMainWin(Engine inEngine)
        {
            eng = inEngine;
        }
        
        public void DescribeRoom(string roomId)
        {
            Room lRoom = eng.lib.GetRoom(roomId);
            if (lRoom!=null)
            {
                // Hlavny opis miestnosti
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(lRoom.name);
                Console.ResetColor();
                Console.WriteLine(lRoom.desc);

                // Opisat, ci su tu nejake static predmety
                foreach (GameItem git in eng.lib.gameItems)
                {
                    if (git.position==eng.party.actualRoomID)
                    {
                        Console.WriteLine("{0}", git.hidden.ToString());
                        if (!git.hidden)
                        {
                            Console.Write("Vidis tu ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(git.itemName);
                            Console.ResetColor();
                            Console.WriteLine(".");
                        }
                    }
                }

                // Opisat moznosti cestovania:
                Console.Write("Mozes ist: ");
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (Road rd in eng.lib.roads)
                {
                    if (rd.enabled)
                    {
                        if (rd.sourceRoom==eng.party.actualRoomID)
                        {
                            if ((rd.bothWay == Direction.BOTH) ||  (rd.bothWay == Direction.TO_TARGET))
                            {                           
                                Console.Write("{0} ", Road.GetPathName(rd.direction1));
                            }
                        }
                        if (rd.targetRoom==eng.party.actualRoomID)
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
            }
            else Console.WriteLine("Engine error: Unable to find room '{0}'", roomId);
        }

        public void ShowRoom()
        {
            DescribeRoom(eng.party.actualRoomID);
            // Show Container
        }

        public void ShowPotencialActions()
        {
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            List<string> actList = new List<string>();
            foreach(GameItem tmpGItem in eng.lib.gameItems)
            {
                if (tmpGItem.position==eng.party.actualRoomID)
                {
                    if (!tmpGItem.hidden)
                    {
                        foreach(Action tmpAct in eng.lib.actions)
                        {
                            if (tmpAct.id==tmpGItem.id)
                            {
                                //Console.WriteLine("Act "+tmpAct.desc);
                                //dict.Add(tmpAct.id, );
                                actList.Add(tmpAct.id);
                            }
                        }
                    }
                }
            }
       
            for(int a=0;a<actList.Count;a++)
            {
                Action tmpAct = eng.lib.GetAction(actList[a]);
                Console.WriteLine("{0} {1}", (a+1).ToString(), tmpAct.desc);
            }
            Console.WriteLine("{0} Konec", (actList.Count+1).ToString());
        }

        public void Show()
        {
            Console.Clear();
            ShowRoom();

            string line = "";
            do
            {
                Console.Write("> ");
                line = Console.ReadLine();

                if (line=="s")
                {
                    var ec = eng.Go(Path.NORTH);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else 
                    {
                        Console.WriteLine("Siel si na sever.\n");
                        ShowRoom();
                    }
                }

                if (line=="j") 
                {
                    var ec = eng.Go(Path.SOUTH);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else
                    {
                        Console.WriteLine("Siel si na juh.\n");
                        ShowRoom();
                    }
                }

                if (line=="z") 
                {
                    var ec = eng.Go(Path.WEST);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else
                    {
                        Console.WriteLine("Siel si na zapad.\n");
                        ShowRoom();
                    }
                    //if (ec==ErrorCode.NOT_ENABLED) Console.WriteLine("DISABLED.\n");
                }

                if (line=="v") 
                {
                    var ec = eng.Go(Path.EAST);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else
                    {
                        Console.WriteLine("Siel si na vychod.\n");
                        ShowRoom();
                    }
                    //if (ec==ErrorCode.NOT_ENABLED) Console.WriteLine("DISABLED.\n");
                }

                if (line=="a") 
                {
                    ShowPotencialActions();
                }

            } while (line!="ko");
        }
    }
}