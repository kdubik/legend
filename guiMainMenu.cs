using System;

namespace legend
{
    public class GuiMainMenu
    {
        public void Show()
        {
            //int ch = 0;
            string ch = "";
            do
            {
                Console.Clear();
                Console.WriteLine("1. Quick start");
                Console.WriteLine("2. Start");
                Console.WriteLine("3. Quit");

                ch = Console.ReadLine();

                if (ch=="1") 
                {
                    Engine eng = new Engine();

                    // Prepare default character
                    eng.party.Clean();
                    Character hero = new Character(true);
                    eng.party.members.Add(hero);

                    eng.GiveItemToPlayer("cestovatelske_oblecenie", true);
                    eng.GiveItemToPlayer("mec", true);
                    eng.GiveItemToPlayer("trojzubec", false);

                    GuiMainWin gameWin = new GuiMainWin(eng);                    
                    gameWin.Show();
                }

                if (ch=="2") 
                {
                    Engine eng = new Engine();

                    eng.party.Clean();
                    GuiCharacterCreation charGen = new GuiCharacterCreation(eng);
                    charGen.Show();

                    eng.GiveItemToPlayer("cestovatelske_oblecenie", true);
                    eng.GiveItemToPlayer("mec", true);

                    GuiMainWin gameWin = new GuiMainWin(eng);                    
                    gameWin.Show();
                }
            } while (ch!="3");
        }
    }
}