using System.Collections.Generic;

namespace LegendLibrary
{
    public enum Background { ALDIN, FOREST_FOLK, JARZONI, KERNISH, LARTYAN, OUTCAST, ROAMER };
    public enum Race { HUMAN, NIGHT, SEA, DARK_VATA, VATA };
    public enum BRClass { ADEPT, WARRIOR, EXPERT };
    public enum Specialization { NONE, BERSERKER, BEAST_FRIEND, BARD, DIPLOMAT, DUELIST, HEALER, HUNTER, MARTIAL_ARTIST, SHAPER, SLAYER, SPIRIT_DANCER, SPY };
    public enum CharAttr
    { 
        ACCURACY, COMMUNICATION, CONSTITUTION, DEXTERITY, FIGHTING, IQ, PERCEPTION, STRENGTH, WILL 
    }

    public enum BodySlot { WHEAPON, SHIELD, ARMOR };

    public class Character
    {
        public string name = "Hero";
        public BRClass brclass = BRClass.WARRIOR;
        public Race race = Race.HUMAN;
        public bool female = true;
        public int level = 1;
        public Specialization specialization = Specialization.NONE;

        // Mensie info
        public int max_health, health;
        public int speed, defense, armor;


        // Character attributes
        public int[] attr = new int[9];

        public int[] abilityAdvancement = new int[9];   // When leveling UP, how many points are reguired for increase desired ability

        // Character body equipment
        // Obsahuje ID equipnuteho predmetu, alebo nic
        public string[] bodySlots = new string[3];

        public List<string> focuses = new List<string>();   // Focuses, that hero learns through the game
        public List<string> wheaponsAndArmorGroups = new List<string>();   // What kind of wheapons are allowed?

        public void SetAttribute(CharAttr inAttr, int value) => attr[(int)inAttr] = value;

        public int GetAttribute(CharAttr inAttr) => attr[(int)inAttr];

        /// <summary>
        /// Returns ID of item (wheapon), that character has equipped.false
        /// If there is no wheapon, "empty_hands" is returned.
        /// </summary>
        /// <returns>ID of item (wheapon), that character has equipped</returns>
        public string GetActualWheaponId() => bodySlots[(int)BodySlot.WHEAPON];

        public string GetActualArmorId() => bodySlots[(int)BodySlot.ARMOR];


        /// <summary>
        /// Constructor - prepare default character
        /// </summary>
        /// <param name="female">Is character female? (Man by default)</param>
        public Character(bool female)
        {
            // Defaults
            // Race = HUMAN
            // Class = WARRIOR
            level = 1;
            brclass = BRClass.EXPERT;
            race = Race.HUMAN;

            //wheaponsAndArmorGroups.Add("light");
            wheaponsAndArmorGroups.Add("light_blades");
            wheaponsAndArmorGroups.Add("light_armors");

            if (female)
            {
                name = "Sienna";
                female = true; // Default female
                max_health = 25;
                health = max_health;
                defense = 12;
                speed = 11;
                armor = 0;
                SetAttribute(CharAttr.ACCURACY,2);
                SetAttribute(CharAttr.COMMUNICATION,2);
                SetAttribute(CharAttr.CONSTITUTION,3);  // +1, because HUMAN race
                SetAttribute(CharAttr.DEXTERITY,3);
                SetAttribute(CharAttr.FIGHTING,4);
                SetAttribute(CharAttr.IQ,0);
                SetAttribute(CharAttr.PERCEPTION,3);
                SetAttribute(CharAttr.STRENGTH,3);
                SetAttribute(CharAttr.WILL,1);
                focuses.Add("SEARCHING");
                //focuses.Add("LIGHT_BLADES");
            }
            else
            {
                name = "Scamm";
                female = false; // Default male
                max_health = 25;
                health = max_health;
                defense = 12;
                speed = 11;
                armor = 0;
                SetAttribute(CharAttr.ACCURACY,2);
                SetAttribute(CharAttr.COMMUNICATION,2);
                SetAttribute(CharAttr.CONSTITUTION,3);  // +1, because HUMAN race
                SetAttribute(CharAttr.DEXTERITY,3);
                SetAttribute(CharAttr.FIGHTING,4);
                SetAttribute(CharAttr.IQ,0);
                SetAttribute(CharAttr.PERCEPTION,3);
                SetAttribute(CharAttr.STRENGTH,3);
                SetAttribute(CharAttr.WILL,1);
                focuses.Add("STAMINA");
            }

            for (int a=0;a<9;a++) abilityAdvancement[a] = 0;
        }

        public static CharAttr GetAttributeFromString(string attrString)
        {
            CharAttr res = CharAttr.ACCURACY;
            string li = attrString.ToLower();

            if (li=="accuracy") res = CharAttr.ACCURACY;
            if (li=="communication") res = CharAttr.COMMUNICATION;
            if (li=="constitution") res = CharAttr.CONSTITUTION;
            if (li=="dexterity") res = CharAttr.DEXTERITY;
            if (li=="fighting") res = CharAttr.FIGHTING;
            if (li=="iq") res = CharAttr.IQ;
            if (li=="perception") res = CharAttr.PERCEPTION;
            if (li=="strength") res = CharAttr.STRENGTH;
            if (li=="will") res = CharAttr.WILL;

            return res;
        } 

        public static List<CharAttr> GetPrimaryAbilities(BRClass brclass)
        {
            List<CharAttr> primaryAbilites = new List<CharAttr>();

            switch (brclass)
            {
                case BRClass.ADEPT:
                    primaryAbilites.Add(CharAttr.ACCURACY);
                    primaryAbilites.Add(CharAttr.IQ);
                    primaryAbilites.Add(CharAttr.PERCEPTION);
                    primaryAbilites.Add(CharAttr.WILL);
                    break;
                case BRClass.WARRIOR:
                    primaryAbilites.Add(CharAttr.CONSTITUTION);
                    primaryAbilites.Add(CharAttr.DEXTERITY);
                    primaryAbilites.Add(CharAttr.FIGHTING);
                    primaryAbilites.Add(CharAttr.STRENGTH);
                    break;
                default:
                    // Expert
                    primaryAbilites.Add(CharAttr.ACCURACY);
                    primaryAbilites.Add(CharAttr.COMMUNICATION);
                    primaryAbilites.Add(CharAttr.PERCEPTION);
                    primaryAbilites.Add(CharAttr.DEXTERITY);
                    break;
            }

            return primaryAbilites;
        }

        public void AddFocus(string focusId) => focuses.Add(focusId);

        public bool CheckFocus(string focusId) => focuses.Contains(focusId);
        
        public void ClearFocuses() => focuses.Clear();

        /// <summary>
        /// Check, whether actual wheapon, is of type, that character has focus for.
        /// </summary>
        /// <param name="wheaponType">Type of wheapon</param>
        /// <returns>True, if character has focus for this wheapon type</returns>
        public bool IsUsedWheaponFocused(string wheaponType)
        {
            return focuses.Contains((wheaponType+"s").ToUpper());
        }

        public void AddWheaponGroup(string groupId) => wheaponsAndArmorGroups.Add(groupId);
        public void AddArmorGroup(string groupId) => wheaponsAndArmorGroups.Add(groupId);
        public bool CheckWheaponGroup(string groupId) => wheaponsAndArmorGroups.Contains(groupId+"s");
        public bool CheckArmorGroup(string groupId) => wheaponsAndArmorGroups.Contains(groupId+"s");

        public string[] GetFocusList()
        {
            return focuses.ToArray();
        }
    }
}