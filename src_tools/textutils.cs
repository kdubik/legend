using System;

namespace LegendTools

{
    public class Textutils
    {
		public static bool GetBool(string inputText)
		{
			bool res = false;
			if (inputText == "yes") res = true;
			return res;
		}

        public static int GetNumber()
		{
			string tmpStr;
			int tmpNumber=-2;
			bool converted=false;

			Console.CursorVisible = false;
			do {
				tmpStr = Console.ReadLine();
				//Console.WriteLine ("trying");
				converted = Int32.TryParse(tmpStr, out tmpNumber);							
			} while (!converted);

			Console.CursorVisible = true;

			return tmpNumber;
		}

		public static int GetNumberRange(int min, int max)
		{
			bool inRange=false;
			int data;

			do {
				data=GetNumber();
				if ((data>=min) && (data<=max)) inRange=true;
			} while (!inRange);

			return data;
		}

		public static string[] GetWordsFromLine(string line)
		{
			string[] tmp;
			string[] newTmp;
			tmp = line.Split(' ');
			int a = tmp.Length;

			int count = 0;	// Spociame vsetky medzery co su navyse
			foreach (string tmpStr in tmp)
			{
				// Console.WriteLine("[{0}]",tmpStr);
				if (tmpStr!="") count++;
			}
			// Console.WriteLine("Usable words count: {0}", count);

			newTmp = new string[count];
			count = 0;
			foreach (string tmpStr in tmp)
			{				
				if (tmpStr!="") {
					newTmp[count]=tmpStr;
					count++;
				}
			}
			// Console.WriteLine("TOTAL words count: {0}", count);
			return newTmp;
		}
		
		/*
			Compare strings, with also shortcuts
		 */
		public static bool Compare(string word, string secondWord, string shortcutWord)
		{
			bool res = false;

			bool cond1 = false;
			bool cond2 = false;

			cond1 = word.Equals(secondWord,StringComparison.Ordinal);
			cond2 = word.Equals(shortcutWord,StringComparison.Ordinal);
			res = cond1 || cond2;

			return res;
		}

		/*
		Compare strings, no shortcuts
		 */
		public bool CmpStr(string word, string secondWord)
		{
			bool res = false;
			res = word.Equals(secondWord,StringComparison.Ordinal);			
			return res;
		}

        public static string GetRest(string[] tmp, int startIndex)
        {
            string res = "_error";
            if ((startIndex > 0) && (startIndex < tmp.Length))
            {
                res = "";
                for (int x = startIndex; x < tmp.Length; x++)
                {                                
                    res = res + tmp[x];
                    if ((x + 1) != tmp.Length) res = res + " ";
                }
            }
            return res;
        }

        public static string RemoveLastCharacter(string line)
        {
            string res = "_error";

            if (line.Length > 0)
            {
                res = "";
                for (int a = 0; a < line.Length; a++)
                {
                    if (a + 1 < line.Length) res = res + line[a];
                }
            }

            return res;
        }
        
        public static void WriteColor(ConsoleColor col, string line)
        {
			//var settings = 
            Console.ForegroundColor = col;
            Console.Write(line);
            Console.ResetColor();

			//Console.ForegroundColor = ConsoleColor.Black;
            //Console.BackgroundColor = ConsoleColor.Gray;
        }

        public static void WriteLineColor(ConsoleColor col, string line)
        {
            Console.ForegroundColor = col;
            Console.WriteLine(line);
            Console.ResetColor();
            
			//Console.ForegroundColor = ConsoleColor.Black;
            //Console.BackgroundColor = ConsoleColor.Gray;
        }

		// As input, some commands have to be entered
		// example: "File New Help"
		// Function returns number of chosen option
		public static int LineMenu(string commandsLine)
		{
			int res = -1;
			string[] commands = GetWordsFromLine(commandsLine);
			int commandCount = commands.Length;

			int startPx = Console.CursorLeft;
			int startPy = Console.CursorTop;

			foreach (string cmd in commands)
			{
				Console.Write(cmd);
				Console.SetCursorPosition(startPx, startPy);
				WriteColor(ConsoleColor.Yellow, cmd[0].ToString());
				startPx = startPx + cmd.Length + 1;
				Console.SetCursorPosition(startPx, startPy);
			}

			int x;
			Console.CursorVisible = false;
			
			do
			{
				ConsoleKeyInfo cki = Console.ReadKey(true);				

				// Ktory klaves bol stlaceny?
				x = 0;
				foreach (string cmd in commands)
				{
					x++;
					string tmp = cmd[0].ToString().ToLower();

					if (cki.KeyChar == tmp[0]) 
					{
						res = x;						
						break;
					}
				}			
			} while (res==-1);
			Console.Write("\n");
			Console.CursorVisible = true;

			return res;
		}

		public static void WaitKey()
		{
			Console.CursorVisible = false;
			Console.ReadKey(true);
			Console.CursorVisible = true;
		}

		public static ConsoleKeyInfo GetPressedKey()
		{
			Console.CursorVisible = false;
			ConsoleKeyInfo key = Console.ReadKey(true);		
			Console.CursorVisible = true;
			return key;
		}
    }
}