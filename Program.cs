using System;

namespace legend
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Speed RPG");
            // Console.ReadLine();
            GuiMainMenu mainWin = new GuiMainMenu();
            mainWin.Show();
        }
    }
}
