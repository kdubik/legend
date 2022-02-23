using System;

using LegendTools;
using LegendEngine;
using Ascidraw;

namespace Legend
{
    public class GuiMainMenu
    {
        Engine eng = new Engine();
        Adraw ad = new Adraw();

        public void ShowGUITexts()
        {
            ad.DrawWindow(0,0,ad.screenWidth, ad.screenHeight,"",true);

            Console.SetCursorPosition(1,1);
            Console.WriteLine(eng.lib.gameInfo.name);
            //Console.WriteLine("");

            Console.SetCursorPosition(1,3);
            Print pr = new Print(eng.lib.gameInfo.gameDesc);
            pr.Render(1);

            Console.SetCursorPosition(1,8);
            Console.Write("1. Quick start\n");
            Console.SetCursorPosition(1,9);
            Console.WriteLine("2. Start");
            Console.SetCursorPosition(1,10);
            Console.WriteLine("3. Quit");
        }
        public void Show()
        {
            // Prepare GUI
            ad.SetLineType(LineType.DOUBLE);

            // Prepare game
            //Engine eng = new Engine();
            eng.lib.LoadConfigFiles();  // Most important game data files

            ShowGUITexts();

            //int ch = 0;
            //string ch = "";
            ConsoleKeyInfo ch;
            do
            {
                /*
                //Console.Clear();
                ad.DrawWindow(0,0,ad.screenWidth, ad.screenHeight,"",true);

                Console.SetCursorPosition(1,1);
                Console.WriteLine(eng.lib.gameInfo.name);
                //Console.WriteLine("");

                Console.SetCursorPosition(1,3);
                Print pr = new Print(eng.lib.gameInfo.gameDesc);
                pr.Render(1);

                Console.SetCursorPosition(1,8);
                Console.Write("1. Quick start\n");
                Console.SetCursorPosition(1,9);
                Console.WriteLine("2. Start");
                Console.SetCursorPosition(1,10);
                Console.WriteLine("3. Quit");
                */

                //ch = Console.ReadLine();
                //ch = Textutils.GetNumberRange(1,3);
                ch = Textutils.GetPressedKey();

                if (ch.KeyChar=='1') 
                {
                    //Prepare new game
                    eng.PrepareNewGame(true);

                    GuiCampaignWindow campaignWin = new GuiCampaignWindow(eng);
                    campaignWin.Show();
                    //GuiMainWin gameWin = new GuiMainWin(eng);                    
                    //gameWin.Show();
                    ShowGUITexts();
                }

                if (ch.KeyChar=='2') 
                {
                    //Prepare new game
                    eng.PrepareNewGame(false);

                    GuiCharacterCreation charGen = new GuiCharacterCreation(eng);
                    charGen.Show();

                    GuiCampaignWindow campaignWin = new GuiCampaignWindow(eng);
                    campaignWin.Show();

                    //GuiMainWin gameWin = new GuiMainWin(eng);                    
                    //gameWin.Show();

                    ShowGUITexts();
                }
            } while (ch.KeyChar!='3');
        }
    }
}