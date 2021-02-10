namespace legend
{
    public static class Tools
    {
        /// <summary>
        /// This function returns sinegle string composed of input strings,
        /// starting with defined string index.
        /// </summary>
        /// <param name="inStrings">Input list of strings.</param>
        /// <param name="startNo">Defines starting string. Rest of strings are merged and returned.</param>
        /// <returns></returns>
        public static string MergeString(string[] inStrings, int startNo)
        {
            string ret = "";

            ret = inStrings[startNo];

            for (int a=startNo+1;a<inStrings.Length;a++)
            {
                ret = ret + " " + inStrings[a];
            }

            return ret;
        }
    
        public static string RemoveQuotes(string msg)
        {
            // Remove quotes (first and last character of the sentence)
            msg = msg.Substring(1);
            msg = msg.Remove(msg.Length - 1);
            return msg;
        }

        public static bool CheckForQuotes(string msg)
        {
            bool res = false;
            char quota = '"';
            if ((msg[0]==quota) && (msg[msg.Length - 1]==quota)) res=true;
            return res; 
        }
    }
}