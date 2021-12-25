namespace LegendLibrary
{
    public class AdventureInfo
    {
        public string id, mapName;      // Name and Id for this map
        public string startLocation;    // Entrance to the location
        public string targetLocation;   // When player comes here, it gains a reward
        
        public bool exterior;
    }
}