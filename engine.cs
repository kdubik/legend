using System;
using System.Collections.Generic;

namespace legend
{
    public enum ErrorCode { OK, ROOM_NOT_FOUND, EMPTY_PATH };

    public class Engine
    {
        public Library lib = new Library();
        public Party party = new Party();
        public List<InvertoryItem> gameItems = new List<InvertoryItem>();
        public Engine()
        {
            lib.LoadData();
            party.actualRoomID = "1";
        }

        public ErrorCode Go(EPath path)
        {
            ErrorCode ec = ErrorCode.OK;

                Room lroom = lib.GetRoom(party.actualRoomID);
                if (lroom!=null)
                {
                    string tmp = lroom.GetRoadTarget(path);
                    if (tmp!="np")
                    {
                         party.actualRoomID = tmp;
                    } else ec=ErrorCode.EMPTY_PATH;

                } else ec=ErrorCode.ROOM_NOT_FOUND;

            return ec;
        }
    }
}