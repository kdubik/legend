using System;

namespace LegendTools
{
    public class Print
    {
        string[] words = null;

        public Print(string msg)
        {
            words = msg.Split(" ");
            //Console.WriteLine("Words count: {0}",words.Length.ToString());
        }

        public void SetMessage(string msg)
        {
            //words = null;
            words = msg.Split(" ");
        }

        /*
        public string PrepareLine(int startPoint, out int wordCount)
        {
            string tmp = "";
            string oldTmp = "";
            bool isEnd = false;
            int actualWord = startPoint;
            int usedWordCounter = 0;

            do
            {
                oldTmp = tmp;

                // 1. Vezmi slovo
                // 2. Pridaj ho k vete
                if (tmp!="") tmp = tmp + " ";
                tmp = tmp + words[actualWord];
                usedWordCounter++;                  // How many words did we used
                actualWord++;

                // 3. Je vacsie ako 80?
                // Ak ano, tak je koniec a vrat vetu este spred pridanim slova.
                if (tmp.Length>80)
                {
                    isEnd = true;
                    tmp = oldTmp;
                }

                // 3b. Je posledne slovo <br> ?
                // Ak ano, tak je koniec a vrat vetu este spred pridanim slova.
                if (words[actualWord-1]=="<br>")
                {
                    isEnd = true;
                    tmp = oldTmp;
                    usedWordCounter++;
                }
                
                // 4. Opakuj, ak este mame slova a nie je koniec
                if (actualWord==words.Length) isEnd=true;

            } while (!isEnd);

            wordCount = usedWordCounter;
            return tmp;
        }
        */
        public string PrepareLine(int startPoint, out int wordCount, int maxLineLength)
        {
            string tmp = "";
            string oldTmp = "";
            bool isEnd = false;
            int actualWord = startPoint;
            int usedWordCounter = 0;

            do
            {
                oldTmp = tmp;

                // 1. Vezmi slovo
                // 2. Pridaj ho k vete
                if (tmp!="") tmp = tmp + " ";
                tmp = tmp + words[actualWord];
                usedWordCounter++;                  // How many words did we used
                actualWord++;

                // 3. Je vacsie ako maxLineLength (80 znakov|?
                // Ak ano, tak je koniec a vrat vetu este spred pridanim slova.
                if (tmp.Length>maxLineLength)
                {
                    isEnd = true;
                    tmp = oldTmp;
                }

                // 3b. Je posledne slovo <br> ?
                // Ak ano, tak je koniec a vrat vetu este spred pridanim slova.
                if (words[actualWord-1]=="<br>")
                {
                    isEnd = true;
                    tmp = oldTmp;
                    usedWordCounter++;
                }
                
                // 4. Opakuj, ak este mame slova a nie je koniec
                if (actualWord==words.Length) isEnd=true;

            } while (!isEnd);

            wordCount = usedWordCounter;
            return tmp;
        }

        public string PrepareLine(int startPoint, out int wordCount)
        {
            return PrepareLine(startPoint,out wordCount, 80);
        }

        /*
        private void RenderLine(string line)
        {
            foreach (char ch in line)
            {
                Console.Write(ch);
                //System.Threading.Thread.Sleep(2);
            }
            Console.WriteLine("");
        }
        */

        private void RenderLine(string line)
        {
            RenderLine(line,0);
        }

        private void RenderLine(string line, int offsetX)
        {
            int cx,cy;
            (cx,cy) = Console.GetCursorPosition();
            Console.SetCursorPosition(offsetX, cy);

            foreach (char ch in line)
            {
                Console.Write(ch);
                //System.Threading.Thread.Sleep(2);
            }
            Console.WriteLine("");
        }

        public void Render()
        {
            int usedWordCount = 0;
            int actualWord = 0;
            int wordsAvaiable = words.Length;

            //Console.WriteLine("Total words Avaiable: {0}", wordsAvaiable);
            do
            {
                //1. Prepare line
                string line = PrepareLine(actualWord, out usedWordCount);
                wordsAvaiable -=  usedWordCount-1;
                actualWord += usedWordCount -1;

                //2. Print line
                //Console.WriteLine(line);
                RenderLine(line);
                System.Threading.Thread.Sleep(7);

                //3. If there are still some words, repeat.
                //Console.WriteLine("wordsAvaiable: {0}", wordsAvaiable);
            } while (wordsAvaiable>1);
        }
    
        public void Render(int offsetX)
        {
            int usedWordCount = 0;
            int actualWord = 0;
            int wordsAvaiable = words.Length;
            int lineLength = 80 - offsetX;

            //Console.WriteLine("Total words Avaiable: {0}", wordsAvaiable);
            do
            {
                //1. Prepare line
                string line = PrepareLine(actualWord, out usedWordCount);
                wordsAvaiable -=  usedWordCount-1;
                actualWord += usedWordCount -1;

                //2. Print line
                //Console.WriteLine(line);
                RenderLine(line, offsetX);
                System.Threading.Thread.Sleep(7);

                //3. If there are still some words, repeat.
                //Console.WriteLine("wordsAvaiable: {0}", wordsAvaiable);
            } while (wordsAvaiable>1);
        }
    }
}