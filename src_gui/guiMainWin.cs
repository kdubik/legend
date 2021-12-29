using System;
using System.Collections.Generic;

using Ascidraw;

using LegendTools;
using LegendEngine;
using LegendLibrary;

namespace Legend
{
    public class GuiMainWin
    {
        
        public Engine eng;
        //GameStatus status = GameStatus.PLAYING;
        public Adraw ad = new Adraw();

        public Dices dices = new Dices();

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
            // Show GUI
            ad.SetLineType(LineType.DOUBLE);
            ad.DrawWindow(0,0,ad.screenWidth, ad.screenHeight," Akcie ",true);

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
                Console.SetCursorPosition(2,a+2);
                Console.Write("{0} {1}", (a+1).ToString(), desc);
            }
            Console.SetCursorPosition(2,actList.Count+2);
            Console.Write("{0} Konec", (actList.Count+1).ToString());


            int no;
            ConsoleKeyInfo ch;

            do
            {
                //string line = Console.ReadLine();
                //no = int.Parse(line) - 1;
                
                //Console.SetCursorPosition(2,actList.Count+3);
                //no = Textutils.GetNumberRange(1, actList.Count) - 1;

                bool was = false;
                do 
                {
                    ch = Textutils.GetPressedKey();           
                    Console.WriteLine(ch.KeyChar.ToString());
                    was = int.TryParse(ch.KeyChar.ToString(),out no);
                } while (!was);
                no--;           

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

        private bool isNumberEven(int number)
        {
            bool res = false;
            if(number%2 == 0) res = true;
            return res;
        }

        public void UpdatePrimaryAbilities(List<CharAttr> primaryAbilites, int characterLevel)
        {
            int x = 0;
            int optionsCount = 4;
            int[] optionTable = new int[5];
            bool evenLevel = isNumberEven(characterLevel);

            int reguriredPoints = 2;
            if (characterLevel<6) reguriredPoints = 1;
            if (characterLevel>8) reguriredPoints = 3;

            Console.WriteLine("  (Required points for upgrade: {0})\n", reguriredPoints);

            if (evenLevel)
            {
                foreach (var lat in primaryAbilites)
                {
                    x++;
                    if (lat == CharAttr.ACCURACY) Console.WriteLine("{0}. Accuracy (prenost) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[0].ToString());
                    if (lat == CharAttr.COMMUNICATION) Console.WriteLine("{0}. Communication (komunikacia) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[1].ToString());
                    if (lat == CharAttr.CONSTITUTION) Console.WriteLine("{0}. Constitution (odolnost) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[2].ToString());
                    if (lat == CharAttr.DEXTERITY) Console.WriteLine("{0}. Dexterity (obratnost) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[3].ToString());
                    if (lat == CharAttr.FIGHTING) Console.WriteLine("{0}. Fighting (boj) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[4].ToString());
                    if (lat == CharAttr.IQ) Console.WriteLine("{0}. Intelect (inteligencia) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[5].ToString());
                    if (lat == CharAttr.PERCEPTION) Console.WriteLine("{0}. Perception (vnimanie) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[6].ToString());
                    if (lat == CharAttr.STRENGTH) Console.WriteLine("{0}. Strength (sila) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[7].ToString());
                    if (lat == CharAttr.WILL) Console.WriteLine("{0}. Will (sila vole) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[8].ToString());
                }
            }
            else
            {       
                optionsCount = 5;        
                x=1;
                if (!primaryAbilites.Contains(CharAttr.ACCURACY)) 
                {
                    Console.WriteLine("{0}. Accuracy (prenost) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[0].ToString());
                    optionTable[x-1]=0;
                    x++;                    
                }
                if (!primaryAbilites.Contains(CharAttr.COMMUNICATION))
                {
                    Console.WriteLine("{0}. Communication (komunikacia) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[1].ToString());
                    optionTable[x-1]=1;
                    x++;
                }
                if (!primaryAbilites.Contains(CharAttr.CONSTITUTION))
                {
                    Console.WriteLine("{0}. Constitution (odolnost) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[2].ToString());
                    optionTable[x-1]=2;
                    x++;
                }

                if (!primaryAbilites.Contains(CharAttr.DEXTERITY)) 
                {
                    Console.WriteLine("{0}. Dexterity (obratnost) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[3].ToString());
                    optionTable[x-1]=3;
                    x++;
                }
                if (!primaryAbilites.Contains(CharAttr.FIGHTING)) 
                {
                    Console.WriteLine("{0}. Fighting (boj) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[4].ToString());
                    optionTable[x-1]=4;
                    x++;
                }
                if (!primaryAbilites.Contains(CharAttr.IQ))
                {
                    Console.WriteLine("{0}. Intelect (inteligencia) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[5].ToString());
                    optionTable[x-1]=5;
                    x++;
                }
                if (!primaryAbilites.Contains(CharAttr.PERCEPTION)) 
                {
                    Console.WriteLine("{0}. Perception (vnimanie) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[6].ToString());
                    optionTable[x-1]=6;
                    x++;
                }
                if (!primaryAbilites.Contains(CharAttr.STRENGTH))
                {
                    Console.WriteLine("{0}. Strength (sila) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[7].ToString());
                    optionTable[x-1]=7;                 
                    x++;
                }
                if (!primaryAbilites.Contains(CharAttr.WILL)) 
                {
                    Console.WriteLine("{0}. Will (sila vole) [body: {1}]", x.ToString(), eng.party.members[0].abilityAdvancement[8].ToString());            
                    optionTable[x-1]=8;
                    x++;
                }
            }
        
            int a;
            a = Textutils.GetNumberRange(1,optionsCount);

            /*
            if (evenLevel)
            {
                int an = (int)primaryAbilites[a-1];
                eng.party.members[0].abilityAdvancement[an]++;
                //eng.party.members[0].attr[an]++;
            }
            else
            {
                int an = optionTable[a-1];
                eng.party.members[0].abilityAdvancement[an]++;
            }
            */

            int an = optionTable[a-1];
            if (evenLevel) an = (int)primaryAbilites[a-1];
            eng.party.members[0].abilityAdvancement[an]++;

            // Check abilities
            for (int b=0; b<9;b++)
            {
                if (eng.party.members[0].abilityAdvancement[b]>=reguriredPoints)
                {
                    eng.party.members[0].abilityAdvancement[b] = 0;
                    eng.party.members[0].attr[b]++;
                    Console.WriteLine("Schopnost postavy {0} sa zvysila (na {1} bodov)!", ((CharAttr)b).ToString(), eng.party.members[0].attr[b].ToString());
                }
            }        
        }

        public void ChooseNewFocus(List<CharAttr> primaryAbilites, int characterLevel)
        {
            bool evenLevel = isNumberEven(characterLevel);

            // 1. zistit, list focusov (podla evenLevelu)
            List<focus_data> fd;
            if (evenLevel) fd = eng.lib.focuses.GetListOfPrimaryAbilities(primaryAbilites);
            else fd = eng.lib.focuses.GetListOfNonPrimaryAbilities(primaryAbilites);

            // 2. vyhodit focusy, ktore uz mames
            List<focus_data> delList = new List<focus_data>();
            foreach (var lf in fd)
            {
                if (eng.party.members[0].focuses.Contains(lf.id)) delList.Add(lf);
            }

            foreach (var erase in delList)
                fd.Remove(erase);

            // 3. zobrazit focusy + nesmieme zobrazit tie, ktore uz mame
            int a = 0;
            foreach (var lf in fd)
            {
                a++;
                Console.WriteLine("{0}. {1} ({2})", a.ToString(), lf.name, lf.id);
            }

            // 4. vypytat si, ktory focus chceme pridat do zoznamu
            int answer = Textutils.GetNumberRange(1,a) - 1;

            eng.party.members[0].focuses.Add(fd[answer].id);
            Console.WriteLine("Pridavam focus \"{0}\"",fd[answer].name);
        }

        private void DoLevelUp()
        {
            List<CharAttr> primaryAbilites =  Character.GetPrimaryAbilities(eng.party.members[0].brclass);

            eng.party.members[0].level++;
            eng.party.actualDungeonWin = true;
            
            Console.WriteLine("Dobrodruzstvo bolo uspesne splnene!");
            Console.WriteLine("Postava dosiahla dalsiu uroven - {0}!\n", eng.party.members[0].level.ToString());
            Console.ReadLine();

            // 1. Ve increase health for our hero
            int newLives = eng.party.members[0].GetAttribute(CharAttr.CONSTITUTION);
            if (eng.party.members[0].level<11) newLives+= dices.ThrowDice();
            Console.Write("Zdravie {0} bolo zvysene o {1} bodov!", eng.party.members[0].name, newLives.ToString());
            int oldHealth = eng.party.members[0].max_health;
            eng.party.members[0].max_health += newLives;
            Console.WriteLine(" ({0} -> {1})", oldHealth.ToString(),eng.party.members[0].max_health.ToString());
            Console.Write("Press any key");
            Console.ReadLine();

            // 2. Ability advancement
            Console.WriteLine("Zlepsenie schopnosti postavy!");
            Console.WriteLine("Ziskavas 1 bod (advancement)! Zvol si schopnost, ktora sa vylepsi:");
            UpdatePrimaryAbilities(primaryAbilites, eng.party.members[0].level);
            Console.Write("Press any key");
            Console.ReadLine();

            // 3. Class powers

            // 4. Ability focus
            Console.WriteLine("Zlepsenie schopnosti postavy!");
            Console.WriteLine("Ziskavas 1 dalsi focus! Zvol si novy focus:");
            ChooseNewFocus(primaryAbilites, eng.party.members[0].level);

            Console.WriteLine("\nChces este pokracovat v prieskume lokality? (a/n)");
            
            if (!Textutils.GetYesNo())
            {
                // Ukoncime dungeon crawling
            }
        }

        public void ShowHelp()
        {
            Console.Clear();
            Print pr = new Print( eng.lib.GetTextBlock("BThelp") );
            pr.Render();
            Console.WriteLine("\nStlac ENTER pre pokracovanie...");
            Console.ReadLine();
        }

        private void ShowFocuses()
        {
            int fc = eng.lib.focuses.Count();
            for (int a=0;a<fc;a++)
            {
                focus_data fd = eng.lib.focuses.GetByIndex(a);
                Console.WriteLine("Focus: {0} ({1})", fd.name, fd.id);
            }
        }

        public void Show()
        {
            ShowIntro();

            Console.Clear();
            ShowRoom();

            string line = "";
            do
            {
                bool levelUp = false;
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

                    if (ec==ErrorCode.TARGET_LOCATION_REACHED) levelUp = true;

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
                    if (ec==ErrorCode.TARGET_LOCATION_REACHED) levelUp = true;
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
                    if (ec==ErrorCode.TARGET_LOCATION_REACHED) levelUp = true;
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
                    if (ec==ErrorCode.TARGET_LOCATION_REACHED) levelUp = true;
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
                    if (ec==ErrorCode.TARGET_LOCATION_REACHED) levelUp = true;
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
                    if (ec==ErrorCode.TARGET_LOCATION_REACHED) levelUp = true;
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
                    GuiMap map = new GuiMap(eng);
                    map.Show();
                    //Console.ReadLine();
                    var ch = Textutils.GetPressedKey();
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
                    if (w[0]=="levelup")
                    {
                        DoLevelUp();
                    }
                    if (w[0]=="focuses")
                    {
                        ShowFocuses();
                    }
                }

                // Other events
                if (levelUp) DoLevelUp();

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