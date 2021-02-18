using System;

namespace legend
{
    public class GuiMainMenu
    {
        public void Show()
        {
            // Prepare game
            Engine eng = new Engine();
            eng.lib.LoadConfigFiles();  // Most important game data files

            //int ch = 0;
            string ch = "";
            do
            {
                Console.Clear();
                Console.WriteLine(eng.lib.gameInfo.name);
                Console.WriteLine("");

                Print pr = new Print(eng.lib.gameInfo.gameDesc);
                pr.Render();

                Console.WriteLine("\n1. Quick start");
                Console.WriteLine("2. Start");
                Console.WriteLine("3. Quit");

                ch = Console.ReadLine();

                if (ch=="1") 
                {
                    //Prepare new game
                    eng.PrepareNewGame(true);

                    eng.GiveItemToPlayer("cestovatelske_oblecenie", true);
                    eng.GiveItemToPlayer("dyka", true);
                    //eng.GiveItemToPlayer("trojzubec", false);

                    GuiMainWin gameWin = new GuiMainWin(eng);                    
                    gameWin.Show();
                }

                if (ch=="2") 
                {
                    //Prepare new game
                    eng.PrepareNewGame(false);

                    GuiCharacterCreation charGen = new GuiCharacterCreation(eng);
                    charGen.Show();

                    eng.GiveItemToPlayer("cestovatelske_oblecenie", true);
                    eng.GiveItemToPlayer("dyka", true);

                    GuiMainWin gameWin = new GuiMainWin(eng);                    
                    gameWin.Show();
                }
            } while (ch!="3");
        }
    }
}