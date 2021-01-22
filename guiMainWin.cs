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
                Console.Write("  ");
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
                            //Console.WriteLine(".");
                        }
                    }
                }

                // Opisat, ci su tu nejake NPC postavy
                foreach (NPC tmpNPC in eng.lib.NPCs)
                {
                    if (tmpNPC.position==eng.party.actualRoomID)
                    {
                        // Console.WriteLine("{0}", git.hidden.ToString());
                        if (tmpNPC.alive)
                        {
                            Console.Write("Stoji tu ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(tmpNPC.name);
                            Console.ResetColor();
                            //Console.WriteLine(".");
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

        public ErrorCode RunSpecialAction(string actId)
        {
            ErrorCode ret = ErrorCode.OK;

            Action tmpAct = eng.lib.GetAction(actId);
            if (tmpAct!=null)
            {

                // ACTION - TEST
                if (tmpAct.action == ActionType.TEST)
                {
                    bool sucess = eng.DoTest(tmpAct.attribute, tmpAct.level);
             
                    if (sucess)
                        eng.ExecuteActionList(tmpAct.successActions);
                    else
                        eng.ExecuteActionList(tmpAct.failedActions);
                }

                // ACTION - DEFAULT
                if (tmpAct.action == ActionType.DEFAULT)
                    eng.ExecuteActionList(tmpAct.successActions);

            } else ret = ErrorCode.ACTION_NOT_FOUND;

            return ret;
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
                            if (tmpAct.itemId==tmpGItem.id)
                            {
                                Console.WriteLine("Act "+tmpAct.desc);
                                Console.WriteLine("ActID "+tmpAct.id);
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

            int no;
            do
            {
                string line = Console.ReadLine();
                no = int.Parse(line) - 1;

                if ((no>=0) && (no<actList.Count))
                {
                    RunSpecialAction(actList[no]);
                    no = actList.Count; // Koli ukonceniu tohto menu
                }
            } while (no!=actList.Count);
        }

        public void ShowCharacterStatus()
         {
            // Dennik hraca
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Dennik postavy: {0}", eng.party.members[0].name);
            Console.ResetColor();

            Console.WriteLine("Presnost: {0}", eng.party.members[0].GetAttribute(Attribute.ACCURACY).ToString());
            Console.WriteLine("Komunikativnost: {0}", eng.party.members[0].GetAttribute(Attribute.COMMUNICATION).ToString());
            Console.WriteLine("Zdravie: {0}", eng.party.members[0].GetAttribute(Attribute.CONSTITUTION).ToString());
            Console.WriteLine("Obratnost: {0}", eng.party.members[0].GetAttribute(Attribute.DEXTERITY).ToString());
            Console.WriteLine("Bojovanie: {0}", eng.party.members[0].GetAttribute(Attribute.FIGHTING).ToString());
            Console.WriteLine("Intelekt: {0}", eng.party.members[0].GetAttribute(Attribute.IQ).ToString());
            Console.WriteLine("Vnimanie: {0}", eng.party.members[0].GetAttribute(Attribute.PERCEPTION).ToString());
            Console.WriteLine("Sila: {0}", eng.party.members[0].GetAttribute(Attribute.STRENGTH).ToString());
            Console.WriteLine("Sila vole: {0}", eng.party.members[0].GetAttribute(Attribute.WILL).ToString());

            Console.ReadLine();
            Console.Clear();
         }

        public string SelectItemForEquip(ItemType equipType)
        {
            string res = "";
            Console.Clear();
            List<string> table = new List<string>();

            string desc = "zban";
            if (equipType==ItemType.ARMOR) desc = "zboj/odev/stit";

            // List all wheapons on invertory
            int no = 0;
            foreach (GameItem gi in eng.lib.gameItems)
            {
                if ((gi.position=="player") && (gi.itemType==equipType))
                {
                    no++;
                    string name = eng.lib.GetItem(gi.id).name;
                    Console.WriteLine("{0}. {1}", no.ToString(), name);
                    table.Add(gi.id);
                }
            }

            if (no>0)
            {
                int m = 0;
                do
                {
                    Console.Write("Vyber si aktivnu {0} (1-{1}), alebo {2} pre koniec: ",
                        desc, no.ToString(), (no+1).ToString());
                    string equip = Console.ReadLine();
                    m = int.Parse(equip);

                    if ((m>0) && (m<no+1))
                    {
                        res = table[m-1];
                        m=(no+1);
                    }

                } while (m!=(no+1));
            } else Console.WriteLine("Ziadna {0} nie je k dispozicii.", desc);
            return res;
        }
        public void ShowCharacterInvertory()
         {
            // Batoh hraca
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Batoh postavy: {0}", eng.party.members[0].name);
            Console.ResetColor();

            int no = 0;
            foreach (GameItem gi in eng.lib.gameItems)
            {
                if (gi.position=="player")
                {
                    no++;
                    string name = eng.lib.GetItem(gi.id).name;
                    string outLine = String.Format("{0}. {1}", no.ToString(), name);

                    if (eng.party.members[0].bodySlots[(int)BodySlot.WHEAPON]==gi.id)
                        outLine = outLine + " [Aktivna zbran]";

                    if (eng.party.members[0].bodySlots[(int)BodySlot.ARMOR]==gi.id)
                        outLine = outLine + " [Aktivna zbroj/odev]";

                    if (eng.party.members[0].bodySlots[(int)BodySlot.SHIELD]==gi.id)
                        outLine = outLine + " [Aktivny stit]";

                    Console.WriteLine(outLine);
                }
            }

            do
            {
                Console.WriteLine("1. Zmen vybavenie, 2. Pouzi predmet, 3. Preskumaj, 4. Zahod, 5. Odist ");
                string line = Console.ReadLine();
                no = int.Parse(line);

                if (no==1)
                {
                    int m=0;
                    do
                    {
                        Console.WriteLine("Chces zmenit: 1. Zbran, 2. Zbroj/oblecenie, 3. Stit, 4. Odist");
                        string equip = Console.ReadLine();
                        m = int.Parse(equip);

                        if (m==1)
                        {
                            eng.party.members[0].bodySlots[(int)BodySlot.WHEAPON] = SelectItemForEquip(ItemType.WHEAPON);
                            m=4;
                        }

                        if (m==2)
                        { 
                            eng.party.members[0].bodySlots[(int)BodySlot.ARMOR] = SelectItemForEquip(ItemType.ARMOR);
                            m=4;
                        }

                        if (m==3)
                        { 
                            eng.party.members[0].bodySlots[(int)BodySlot.SHIELD] = SelectItemForEquip(ItemType.SHIELD);
                            m=4;
                        }

                    } while (m!=4);

                }
            } while (no!=5);

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
                    //ShowRoom();
                }

                if (line=="st") 
                {
                    ShowCharacterStatus();
                    ShowRoom();
                }

                if (line=="b") 
                {
                    ShowCharacterInvertory();
                    ShowRoom();
                }

            } while (line!="ko");
        }
    }
}