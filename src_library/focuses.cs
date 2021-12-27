using System;
using System.Collections.Generic;

namespace LegendLibrary
{
    public struct focus_data
    {
        public CharAttr attr;
        public string id,name,desc;

        public focus_data(CharAttr attrName, string focusId, string focusName, string desc)
        {
            attr = attrName;
            id = focusId;
            name = focusName;
            this.desc = desc;
        }
    }

    public class Focuses
    {
        List<focus_data> data = new List<focus_data>();

        public void Add(string attrName, string focusId, string focusName, string desc)
        {
            CharAttr at = Character.GetAttributeFromString(attrName);
            focus_data fd = new focus_data(at, focusId, focusName,desc);
            data.Add(fd);
        }

        public focus_data GetByIndex(int index) => data[index];
        
        public int Count() => data.Count;

        public List<focus_data> GetListOfPrimaryAbilities(CharAttr[] primaryAbilities)
        {
            List<focus_data> listdata = new List<focus_data>();

            return listdata;
        }
        public List<focus_data> GetListOfNonPrimaryAbilities(CharAttr[] primaryAbilities)
        {
            List<focus_data> listdata = new List<focus_data>();

            return listdata;
        }
        
    }
}