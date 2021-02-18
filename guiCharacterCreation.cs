using System;

namespace legend
{
    public class GuiCharacterCreation
    {
        public Engine eng;
        public GuiCharacterCreation(Engine inEngine)
        {
            eng = inEngine;
        }

        public bool Show()
        {
            bool res = true;
            string line = "";
            Character hero = new Character(true);

            Console.Clear();
            Console.WriteLine("Vytvorenie postavy: Vyber si rasu\n\n");
            Console.WriteLine("1. Clovek");
            Console.WriteLine("2. Clovek noci (night-people)");
            Console.WriteLine("3. Sea-folk");
            Console.WriteLine("4. Vata");
            Console.WriteLine("5. Tmavy Vata");
            Console.WriteLine("\n6. ukoncenie...");
            do
            {
                line = Console.ReadLine();
                if (line=="1") hero.race = Race.HUMAN;
                if (line=="2") hero.race = Race.NIGHT;
                if (line=="3") hero.race = Race.SEA;
                if (line=="4") hero.race = Race.VATA;
                if (line=="5") hero.race = Race.DARK_VATA;

            } while (line=="");

            Console.Clear();
            Console.WriteLine("Vytvorenie postavy: Vyber si triedu");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{0}\n", hero.race.ToString());
            Console.ResetColor();

            Console.WriteLine("1. Adept (dominuju carovacie schopnosti)");
            Console.WriteLine("2. Expert (dominuju schopnosti ako obratnost a pohyblivost");
            Console.WriteLine("3. Bojovnik (dominuju bojove schopnosti)");
            Console.WriteLine("\n4. ukoncenie...");
            do
            {
                line = Console.ReadLine();
                if (line=="1") hero.brclass = BRClass.ADEPT;
                if (line=="2") hero.brclass = BRClass.EXPERT;
                if (line=="3") hero.brclass = BRClass.WARRIOR;

            } while (line=="");

            Console.Clear();
            Console.WriteLine("Vytvorenie postavy: Vyber si pohlavie a meno");
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{0} {1}\n", hero.race.ToString(), hero.brclass.ToString());
            Console.ResetColor();

            Console.Write("Zenska postava (a/n)? ");
            line = Console.ReadLine();
            if (line=="a") hero.female = true; else hero.female = false;

            Console.Write("Meno postavy: ");
            line = Console.ReadLine();
            hero.name = line;

            // Ulozime postavu
            eng.party.members.Add(hero);    // Pridame hrdinu do partie

            return res;
        }
    }
}