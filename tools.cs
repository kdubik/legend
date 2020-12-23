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
    }
}