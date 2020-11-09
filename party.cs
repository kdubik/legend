using System.Collections.Generic;

namespace legend
{
    public struct  InvertoryItem
    {
        public string itemId;
        public string position;    // Kde s anachadza? RoomID, alebo Player
        public int condition;  // 5 - great, 4 - good, 1 - bad, 0 - broken
        public int status; // 0-1 je ako On/Off na mechanizmoch, activated/deactivated...
        public bool equiped;
    }

    public class Party
    {
        public string actualRoomID;
        // Party's invertory
        
    }
}