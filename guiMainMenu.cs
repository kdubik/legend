using System;

namespace legend
{
    public class GuiMainMenu
    {
        public void Show()
        {
            int ch = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Start");
                Console.WriteLine("2. Quit");

                ch = Console.Read();
                if (ch=='1') 
                {
                    Engine eng = new Engine();
                    GuiMainWin gameWin = new GuiMainWin(eng);
                    gameWin.Show();
                }
            } while (ch!='2');
        }
    }
}