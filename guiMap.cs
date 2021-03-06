using System;

namespace legend
{
    public class GuiMap
    {
        public Engine eng;
        
        public GuiMap(Engine inEngine)
        {
            eng = inEngine;
        }


        public void DrawLine(int x, int y, int sirka, int vyska)
        {
            bool horizontal = true;
            if (sirka ==0) horizontal = false;

            Console.SetCursorPosition(x,y);
            if (horizontal)
            {
                for (int a=0; a<sirka; a++)
                {
                    Console.SetCursorPosition(x+a,y);
                    Console.Write("-");
                }
            }
            else
            {
                for (int a=0; a<vyska; a++)
                {
                    Console.SetCursorPosition(x,y+a);
                    Console.Write("I");
                }
            }
        }

        public void DrawRectangle(int x, int y, string msg)
        {
            int width = 20;
            int height = 3;

            DrawLine(x,y,width,0);
            DrawLine(x,y,0,height);
            DrawLine(x,y+height-1,width,0);
            DrawLine(x+width-1,y,0,height);

            Console.SetCursorPosition(x+1,y+1);
            Console.Write(msg);
        }

        public void Show()
        {
            Console.Clear();
            Console.WriteLine("Map");

            int scrx = Console.BufferWidth;
            int scry = Console.BufferHeight;

            //20 je sirka obdlznika, m8me 3 okna vedla seba
            int empty_x = (scrx / 3) - 20;
            int empty_y = (scry / 3) - 3;

            int half_empty_x = empty_x / 2;
            int half_empty_y = empty_y / 2;

            Console.WriteLine("empty x . {0}",empty_x.ToString());

            int px = 0; 
            string msg = eng.party.actualRoomID;

            // Main line ( west, current room, east)
            int py = (1 * (empty_y + 3)) + half_empty_y;
            DrawRectangle( (0 * (empty_x + 20)) + half_empty_x, py, msg);
            DrawRectangle( (1 * (empty_x + 20)) + half_empty_x, py, msg);
            DrawRectangle( (2 * (empty_x + 20)) + half_empty_x, py, msg);

            /*
            DrawRectangle( (0 * (empty_x + 20)) + half_empty_x,py, eng.party.actualRoomID);
            DrawRectangle( (1 * (empty_x + 20)) + half_empty_x,py, eng.party.actualRoomID);
            DrawRectangle( (2 * (empty_x + 20)) + half_empty_x,py, eng.party.actualRoomID);
            */
            
        }
    }
}