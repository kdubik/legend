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
                Console.WriteLine("1. Start");
                Console.WriteLine("2. Quit");

                ch = Console.ReadLine();
                if (ch=="1") 
                {
                    Engine eng = new Engine();

                    GuiCharacterCreation charGen = new GuiCharacterCreation(eng);
                    charGen.Show();

                    GuiMainWin gameWin = new GuiMainWin(eng);                    
                    gameWin.Show();
                }
            } while (ch!="2");
        }
    }
}