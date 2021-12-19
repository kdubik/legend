using System;

namespace Legend
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Legends of Aldea RPG, by Kamil Dubik (c) 2021");
            // Console.ReadLine();
            GuiMainMenu mainWin = new GuiMainMenu();
            mainWin.Show();
        }
    }
}
