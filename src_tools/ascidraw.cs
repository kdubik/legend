using System;

namespace Ascidraw
{
    public enum LineType { SINGLE, DOUBLE };
    public class Adraw
    {
        public int screenWidth;
        public int screenHeight;
        public LineType lineType;

        #region  Graphics characters variables
        // Graph characters    
        char cline;     // line (horizontal line) 
        char ccol;      // column (vertical line)

        // Corners of window
        char cornerA;
        char cornerB;
        char cornerC;
        char cornerD;

        #endregion

        public Adraw()
        {            
            screenHeight = Console.WindowHeight;
            screenWidth = Console.WindowWidth;
            lineType = LineType.DOUBLE; // Default
        }

        public void SetLineType(LineType ln)
        {
            lineType = ln;

            // Default = DOUBLE lines
            cline = '\u2550';
            ccol = '\u2551';
            cornerA = '\u2554';
            cornerB = '\u255a';
            cornerC = '\u2557';
            cornerD = '\u255d';

            if (ln==LineType.SINGLE)
            {
                cline = '\u2500';
                ccol = '\u2502';
                cornerA = '\u250c';
                cornerB = '\u2514';
                cornerC = '\u2510';
                cornerD = '\u2518';
            }
        }

        /// <summary>
        /// Clean window with current "background color"
        /// </summary>
        /// <param name="x">Starting character offset on the screen X</param>
        /// <param name="y">Starting character offset on the screen Y</param>
        /// <param name="width">Width of area in characters</param>
        /// <param name="height">Height of area in characters</param>
        public static void CleanWindow(int x, int y, int width, int height)
        {
            for (int ly=0;ly<height;ly++)
            {
                Console.SetCursorPosition(x,y+ly);
                for (int lx=0;lx<width;lx++)
                {
                    Console.Write(" ");
                }
            }
        }
        
        /// <summary>
        /// Draw a window on the terminal
        /// </summary>
        /// <param name="x">Starting character offset on the screen X</param>
        /// <param name="y">Starting character offset on the screen Y</param>
        /// <param name="width">Width of area in characters</param>
        /// <param name="height">Height of area in characters</param>
        public void DrawWindow(int x, int y, int width, int height)
        {
            

            // Lines      
            for (int lx=0;lx<width;lx++)
            {
                Console.SetCursorPosition(x+lx,y);
                Console.Write(cline);
                Console.SetCursorPosition(x+lx,y+height);
                Console.Write(cline);
            }

            // Columns
            for (int ly=0;ly<height;ly++)
            {
                Console.SetCursorPosition(x,y+ly);
                Console.Write(ccol);
                Console.SetCursorPosition(x+width,y+ly);
                Console.Write(ccol);
            }

            // Corners
            Console.SetCursorPosition(x,y);
            Console.Write(cornerA);
            Console.SetCursorPosition(x,y+height);
            Console.Write(cornerB);
            Console.SetCursorPosition(x+width,y);
            Console.Write(cornerC);
            Console.SetCursorPosition(x+width,y+height);
            Console.Write(cornerD);
        }

        /// <summary>
        /// Draw a window on the terminal including its name
        /// </summary>
        /// <param name="x">Starting character offset on the screen X</param>
        /// <param name="y">Starting character offset on the screen Y</param>
        /// <param name="width">Width of area in characters</param>
        /// <param name="height">Height of area in characters</param>
        /// <param name="header">Title on the border (window name)</param>
        public void DrawWindow(int x, int y, int width, int height, string header, bool clean)
        {
            // If cleaning is required
            if (clean) CleanWindow(x, y, width, height);
            // Draw window
            DrawWindow(x, y, width, height);

            // Header
            if (header!="")
            {
                int hs = header.Length;
                int bs = width + 1;
                int cx = (bs - hs)/2;
                Console.SetCursorPosition(x+cx,y);
                Console.Write(header);
            }
        }


    }
}