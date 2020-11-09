using System;

namespace legend
{
    public class GuiMainWin
    {
        public Engine eng;
        public GuiMainWin(Engine inEngine)
        {
            eng = inEngine;
        }
        
        public void DescribeRoom(string roomId)
        {
            Room lRoom = eng.lib.GetRoom(roomId);
            if (lRoom!=null)
            {
                Console.WriteLine(lRoom.desc);
                if (lRoom.container!=null)
                {
                    Console.WriteLine("Vidis tu {0}", lRoom.container.name);
                }
            }
            else Console.WriteLine("Engine error: Unable to find room '{0}'", roomId);
        }

        public void ShowRoom()
        {
            DescribeRoom(eng.party.actualRoomID);
            // Show Container
        }

        

        public void Show()
        {
            ShowRoom();

            string line = "";
            do
            {
                line = Console.ReadLine();

                if (line=="n") 
                {
                    var ec = eng.Go(EPath.NORTH);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else 
                    {
                        Console.WriteLine("Siel si na sever.");
                        ShowRoom();
                    }
                }

                if (line=="s") 
                {
                    var ec = eng.Go(EPath.SOUTH);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else
                    {
                        Console.WriteLine("Siel si na juh.");
                        ShowRoom();
                    }
                }

                if (line=="w") 
                {
                    var ec = eng.Go(EPath.WEST);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else
                    {
                        Console.WriteLine("Siel si na zapad.");
                        ShowRoom();
                    }
                }

                if (line=="e") 
                {
                    var ec = eng.Go(EPath.EAST);
                    if (ec==ErrorCode.EMPTY_PATH)
                    {
                        Console.WriteLine("Unable to go that way.");
                    }
                    else
                    {
                        Console.WriteLine("Siel si na vychod.");
                        ShowRoom();
                    }
                }
                if (line=="otvor") 
                {
                    GuiContainer gc = new GuiContainer(eng);
                    gc.OpenContainer();
                }


            } while (line!="ko");
        }
    }
}