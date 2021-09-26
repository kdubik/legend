using System;

using LegendEngine;
using LegendLibrary;

namespace Legend
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
           

            // Main line ( west, current room, east)
            int py = (1 * (empty_y + 3)) + half_empty_y;
            //Room actRoom = eng.lib.GetRoom(eng.party.actualRoomID);          

            string dir = "";
            string msg_north = "";
            string msg_south = "";
            string msg_west = "";
            string msg_east = "";
            string msg_actual = eng.lib.GetRoomName(eng.party.actualRoomID);
            foreach (Road rd in eng.lib.roads)
            {
                string roomId = eng.GetConnectedRoom(rd,out dir);
                if (roomId!="none") 
                {
                    if (dir=="sever") msg_north = eng.lib.GetRoomName(roomId);
                    if (dir=="juh") msg_south = eng.lib.GetRoomName(roomId);
                    if (dir=="vychod") msg_east = eng.lib.GetRoomName(roomId);
                    if (dir=="zapad") msg_west = eng.lib.GetRoomName(roomId);
                }
            }

            // string msg = eng.party.actualRoomID;
            DrawRectangle( (0 * (empty_x + 20)) + half_empty_x, py, msg_west);
            DrawRectangle( (1 * (empty_x + 20)) + half_empty_x, py, msg_actual);
            DrawRectangle( (2 * (empty_x + 20)) + half_empty_x, py, msg_east);

            /*
            DrawRectangle( (0 * (empty_x + 20)) + half_empty_x,py, eng.party.actualRoomID);
            DrawRectangle( (1 * (empty_x + 20)) + half_empty_x,py, eng.party.actualRoomID);
            DrawRectangle( (2 * (empty_x + 20)) + half_empty_x,py, eng.party.actualRoomID);
            */
            
        }
    }
}