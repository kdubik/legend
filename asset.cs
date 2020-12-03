
// This is basic game object, which is part of "map"
// and should not be bearable. It is static (decoration) type
// of object, that is fixed part of some room

namespace legend
{
    public enum AssetType { DECORATION, TREASURE, FLOOR, TELEPORT, CONTAINER };
    public class Asset
    {
        public AssetType type = AssetType.DECORATION;
        public string id, name;
        public bool invisible;      // Je tato vec viditelna, alebo skovana?
        public bool noticeLevel;    // Ak je schovana, aka je sanca spozorovat tuto vec?

        public string parameter;
        /*
            treasure: how many common/uncommon/rare items is there? Which level? [ako parameter?]
            floor: bonus for fight/defence/running? [vlastne nemusi existovat, je to objekt s akciou, ze "ako ju prekonat"]
            teleport: where? (map or target room) [teleport vlastne nemusi existovat, je to objekt s akciou]
            container: aky konkretny predmet ukryva? [ako parameter?]
        */

        public Asset(string id, AssetType type)
        {
            this.id = id;
            name = id;
            this.type = type;
        }
    }
}