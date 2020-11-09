using System;

namespace legend
{
    public class GuiContainer
    {
        public Engine eng;
        public GuiContainer(Engine inEngine)
        {
            eng = inEngine;
        }
        
        private void ShowContainerContent()
        {
            int itemsTaken = 0; // Kolko predmetov sme vzali
            int itemsLeaved = 0;// Kolko predmetov sme nechali na zemi
            for (int a=0;a<eng.gameItems.Count;a++)
            {
                InvertoryItem inItem = eng.gameItems[a];
                 
                if (eng.gameItems[a].position==eng.party.actualRoomID)
                {
                    var tmpGameItem = eng.gameItems[a];
                    var tmpItem = eng.lib.GetItem(tmpGameItem.itemId);               
                    Console.WriteLine("Vidis tu: {0}", tmpItem.name);
                    Console.WriteLine("Chces tnto predmet vziat? (a/n)");

                    string answer = Console.ReadLine();
                    if (answer=="a")
                    {
                        tmpGameItem.position = "player";
                        itemsTaken++;
                    } else itemsLeaved++;
                }
                Console.WriteLine("Vzal si {0} predmetov. Zanechal si tu {1} predmetov.", 
                    itemsTaken.ToString(), itemsLeaved.ToString());
            }
        }
        public void OpenContainer()
        {
            var aRoom = eng.lib.GetRoom(eng.party.actualRoomID);
            if (aRoom.container!=null)
            {
                Console.WriteLine("Otvaras {0}...", aRoom.container.name);
                
                // Ak je uspesne otvorenie, tak:
                if (aRoom.container.spawner && aRoom.container.used)
                {
                    //GenerujObsah();
                }

                // No a napokon, ak je vsetko ok, tak:
                ShowContainerContent();
                
            }
            else Console.WriteLine("Nie je tu co otvorit!");
        }
    }
}