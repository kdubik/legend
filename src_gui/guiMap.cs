using System;

using LegendEngine;
using LegendLibrary;
using Ascidraw;

namespace Legend
{
    public class GuiMap
    {
        public Engine eng;
        public Adraw ad = new Adraw();
        
        public GuiMap(Engine inEngine)
        {
            eng = inEngine;
            ad.SetLineType(LineType.SINGLE);
        }

        public void DrawRectangle(int x, int y, string msg)
        {
            int width = 20;
            int height = 3;

            ad.DrawWindow(x,y,width,height,"",true);
            Console.SetCursorPosition(x+1,y+1);
            Console.Write(msg);
        }

        public void Show()
        {
            //Console.Clear();
            //Console.WriteLine("Map");
            ad.SetLineType(LineType.DOUBLE);
            ad.DrawWindow(0,0,ad.screenWidth, ad.screenHeight," Map ",true);

            int scrx = Console.BufferWidth;
            int scry = Console.BufferHeight;

            //20 je sirka obdlznika, m8me 3 okna vedla seba
            int empty_x = (scrx / 3) - 20;
            int empty_y = (scry / 3) - 3;

            int half_empty_x = empty_x / 2;
            int half_empty_y = empty_y / 2;

            //Console.WriteLine("empty x . {0}",empty_x.ToString());

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


            if (msg_west!="") DrawRectangle( (0 * (empty_x + 20)) + half_empty_x, py, msg_west);

            // Actual room (highlighted)
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (msg_actual!="")  DrawRectangle( (1 * (empty_x + 20)) + half_empty_x, py, msg_actual);
            Console.ResetColor();
            if (msg_east!="")  DrawRectangle( (2 * (empty_x + 20)) + half_empty_x, py, msg_east);

            if (msg_north!="") DrawRectangle( (1 * (empty_x + 20)) + half_empty_x, py - 6, msg_north);
            if (msg_south!="") DrawRectangle( (1 * (empty_x + 20)) + half_empty_x, py + 6, msg_south);
           
        }
    }
}