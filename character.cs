namespace legend
{
    public enum Background { ALDIN, FOREST_FOLK, JARZONI, KERNISH, LARTYAN, OUTCAST, ROAMER };
    public enum Race { HUMAN, NIGHT, SEA, DARK_VATA, VATA };
    public enum BRClass { ADEPT, WARRIOR, EXPERT };
    public enum Specialization { NONE, BERSERKER, BEAST_FRIEND, BARD, DIPLOMAT, DUELIST, HEALER, HUNTER, MARTIAL_ARTIST, SHAPER, SLAYER, SPIRIT_DANCER, SPY };
    public class Character
    {
        public string name = "Hero";
        public BRClass brclass = BRClass.WARRIOR;
        public Race race = Race.HUMAN;
        public bool female = true;
        public int level = 1;
        public Specialization specialization = Specialization.NONE;

    }
}