using System;
using System.Collections.Generic;

namespace LegendLibrary
{
    public class focus_data
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
            
            /*
            focus_data fd;
            fd.attr = at;
            fd.id = focusId;
            fd.desc = desc;
            fd.name = focusName;
            */
            data.Add(fd);
        }

        public focus_data GetByIndex(int index) => data[index];
        
        public focus_data GetById (string id)
        {
            focus_data ret = null;
            foreach (var fd in data)
            {
                if (fd.id == id)
                {
                    ret = fd;
                    break;
                }
            }
            return ret;
        }
        public int Count() => data.Count;

        public List<focus_data> GetListOfPrimaryAbilities(List<CharAttr> primaryAbilities)
        {
            List<focus_data> listdata = new List<focus_data>();

            foreach (var lf in data)
            {
                if (primaryAbilities.Contains(lf.attr)) listdata.Add(lf);
            }

            return listdata;
        }
        public List<focus_data> GetListOfNonPrimaryAbilities(List<CharAttr> primaryAbilities)
        {
            List<focus_data> listdata = new List<focus_data>();

            foreach (var lf in data)
            {
                if (!primaryAbilities.Contains(lf.attr)) listdata.Add(lf);
            }

            return listdata;
        }
        
    }
}