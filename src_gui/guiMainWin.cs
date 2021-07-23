using System;
using System.Collections.Generic;

using LegendTools;
using LegendEngine;
using LegendLibrary;

namespace Legend
{
    public class GuiMainWin
    {
        
        public Engine eng;
        //GameStatus status = GameStatus.PLAYING;

        public GuiMainWin(Engine inEngine)
        {
            eng = inEngine;
        }
           
        /// <summary>
        /// Describe actual room and check for battle.
        /// </summary>
        public void ShowRoom()
        {
            eng.Check_combat();       

            // If there is not a GAME OVER situation, we can describe a room
            if (eng.actualGameStatus!=GameStatus.LOOSE) eng.DescribeRoom();
        }
        
        public ErrorCode RunSpecialAction(string actId)
        {
            ErrorCode ret = ErrorCode.OK;

            SpecialAction tmpAct = eng.lib.GetAction(actId);
            if (tmpAct!=null)
            {
                // ACTION - ITEM (do we have some Item?)
                if (tmpAct.action == ActionType.ATTRIBUTE)
                {
                    bool sucess = false;
                    foreach (GameItem gi in eng.lib.gameItems)
                    {
                        // Need to implement
                        // if (gi.id==tmpAct.param) sucess=true;
                    }
                    int attrValue = eng.party.members[0].GetAttribute(tmpAct.attribute);

                    if (attrValue >= tmpAct.level) sucess = true;
             
                    if (sucess)
                        eng.ExecuteActionList(tmpAct.successActions);
                    else
                        eng.ExecuteActionList(tmpAct.failedActions);
                }

                // ACTION - ATRIBUTE (do we have some atribut with specific minimal value?)
                if (tmpAct.action == ActionType.ATTRIBUTE)
                {
                    int attrValue = eng.party.members[0].GetAttribute(tmpAct.attribute);

                    bool sucess = false;
                    if (attrValue >= tmpAct.level) sucess = true;
             
                    if (sucess)
                        eng.ExecuteActionList(tmpAct.successActions);
                    else
                        eng.ExecuteActionList(tmpAct.failedActions);
                }

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

            // ITEM list
            foreach(GameItem tmpGItem in eng.lib.gameItems)
            {
                if (tmpGItem.position==eng.party.actualRoomID)
                {
                    if (!tmpGItem.hidden)
                    {
                        foreach(SpecialAction tmpAct in eng.lib.actions)
                        {
                            if (tmpAct.enabled)
                            {
                                if (tmpAct.itemId==tmpGItem.id)
                                {
                                    //Console.WriteLine("Act "+tmpAct.desc);
                                    //Console.WriteLine("ActID "+tmpAct.id);
                                    actList.Add(tmpAct.id);
                                }
                            }
                        }
                    }
                }
            }

            // NPC list
            foreach(NPC lNPC in eng.lib.NPCs)
            {
                if (lNPC.position==eng.party.actualRoomID)
                {
                    if (lNPC.alive)
                    {
                        foreach(SpecialAction tmpAct in eng.lib.actions)
                        {
                            if (tmpAct.enabled)
                            {
                                if (tmpAct.itemId==lNPC.id)
                                {
                                    //Console.WriteLine("Act "+tmpAct.desc);
                                    //Console.WriteLine("ActID "+tmpAct.id);
                                    actList.Add(tmpAct.id);
                                }
                            }
                        }
                    }
                }
            }

            // Enemmy group list
            Room actRoom = eng.lib.GetRoom(eng.party.actualRoomID);
            if (actRoom.enemyGroup!="")
            {
                foreach(SpecialAction tmpAct in eng.lib.actions)
                {
                    if (tmpAct.enabled)
                    {
                        if (tmpAct.itemId=="enemy_group")
                        {
                            //Console.WriteLine("Act "+tmpAct.desc);
                            //Console.WriteLine("ActID "+tmpAct.id);
                            actList.Add(tmpAct.id);
                            break;
                        }
                    }
                }
            }
       
            for(int a=0;a<actList.Count;a++)
            {
                SpecialAction tmpAct = eng.lib.GetAction(actList[a]);
                string desc = eng.lib.GetTextBlock(tmpAct.desc);
                Console.WriteLine("{0} {1}", (a+1).ToString(), desc);
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

            Console.WriteLine("Presnost: {0}", eng.party.members[0].GetAttribute(CharAttr.ACCURACY).ToString());
            Console.WriteLine("Komunikativnost: {0}", eng.party.members[0].GetAttribute(CharAttr.COMMUNICATION).ToString());
            Console.WriteLine("Zdravie: {0}", eng.party.members[0].GetAttribute(CharAttr.CONSTITUTION).ToString());
            Console.WriteLine("Obratnost: {0}", eng.party.members[0].GetAttribute(CharAttr.DEXTERITY).ToString());
            Console.WriteLine("Bojovanie: {0}", eng.party.members[0].GetAttribute(CharAttr.FIGHTING).ToString());
            Console.WriteLine("Intelekt: {0}", eng.party.members[0].GetAttribute(CharAttr.IQ).ToString());
            Console.WriteLine("Vnimanie: {0}", eng.party.members[0].GetAttribute(CharAttr.PERCEPTION).ToString());
            Console.WriteLine("Sila: {0}", eng.party.members[0].GetAttribute(CharAttr.STRENGTH).ToString());
            Console.WriteLine("Sila vole: {0}", eng.party.members[0].GetAttribute(CharAttr.WILL).ToString());

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
                    name = eng.lib.GetTextBlock(name);
                    Console.WriteLine("{0}. {1}", no.ToString(), name);
                    table.Add(gi.id);
                }
            }

            if (no>0)
            {
                int m = 0;
                do
                {
                    Console.Write("Vyber si aktivnu {0} (1-{1}), {2} pre odlozenie vybavenia, alebo {3} pre koniec: ",
                        desc, no.ToString(), (no+1).ToString(), (no+2).ToString());
                    string equip = Console.ReadLine();
                    m = int.Parse(equip);

                    if ((m>0) && (m<no+1))
                    {
                        res = table[m-1];
                        m=no+2;
                    }

                    if (m==(no+1))
                    {
                        res = "";   // Unequip object
                        m=no+2;
                    }

                } while (m!=(no+2));
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
                    Item fff =  eng.lib.GetItem(gi.id);
                    string ttt = fff.name;
                    string name = eng.lib.GetTextBlock(ttt);
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
                Console.WriteLine("\n1. Zmen vybavenie, 2. Pouzi predmet, 3. Preskumaj, 4. Zahod, 5. Odist ");
                //string line = Console.ReadLine();
                //no = int.Parse(line);
                no = Textutils.GetNumberRange(1,5);

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

        public void ShowIntro()
        {
            Console.Clear();
            Console.WriteLine("Vitaj {0}, vitaj v krajine zvanej Aldea!", eng.party.members[0].name);
            //Console.WriteLine("Tvoja kralovna ta potrebuje, dobrodruzstvo caka!");

            Console.WriteLine("");

            Print pr = new Print(eng.lib.gameInfo.gameIntro);
            pr.Render();

            Console.WriteLine("\nAk sa na to citis, stlac ENTER...");
            //Console.ReadLine();
            Textutils.WaitKey();
        }

        public void ShowHelp()
        {
            Console.Clear();
            Print pr = new Print( eng.lib.GetTextBlock("BThelp") );
            pr.Render();
            Console.WriteLine("\nStlac ENTER pre pokracovanie...");
            Console.ReadLine();
        }

        public void Show()
        {
            ShowIntro();

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

                if (line=="d")
                {
                    var ec = eng.Go(Path.DOWN);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else 
                    {
                        Console.WriteLine("Siel si nadol.\n");
                        ShowRoom();
                    }
                }

                if (line=="h")
                {
                    var ec = eng.Go(Path.UP);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else 
                    {
                        Console.WriteLine("Siel si nahor.\n");
                        ShowRoom();
                    }
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

                if (line=="k") 
                {
                    ShowRoom();
                }

                if (line=="ver") 
                {
                    Console.WriteLine("Current game version: {0}",eng.lib.gameInfo.gameVersion);
                    Console.WriteLine("Current engine version: {0}",eng.lib.gameInfo.engineVersion);
                }

                if (line=="p") 
                {
                    ShowHelp();
                    ShowRoom();
                }

                if (line=="map") 
                {
                    //Console.WriteLine("dddd");
                    GuiMap map = new GuiMap(eng);
                    map.Show();
                    Console.ReadLine();
                    ShowRoom();            
                }

                if (line=="cheat") 
                {
                    List<string> cmds = new List<string>();
                    Console.Write("Cheat > ");
                    string cht = Console.ReadLine();
                    string[] w = cht.Split(" ");
                    if (w[0]=="teleport")
                    {
                        cmds.Add("teleport" + " " + w[1]);
                        eng.ExecuteActionList(cmds);
                    }
                }

                // Quit game correctly
                if (line=="ko") eng.actualGameStatus = GameStatus.QUIT;
                if (line=="ks") eng.actualGameStatus = GameStatus.QUIT_SAVE;

            } while (eng.actualGameStatus == GameStatus.PLAYING);

            if (eng.actualGameStatus == GameStatus.WIN)
            {
                // Show win message + statistics              
            }

            if (eng.actualGameStatus == GameStatus.LOOSE)
            {
                // Show loose message + statistics 
                Console.Clear();
                Console.WriteLine("Prehral si...");
                Console.ReadLine();              
            }

            if (eng.actualGameStatus == GameStatus.QUIT)
            {
                // Save game + message           
            }
        }
    }
}