namespace legend
{
    public static class Tools
    {
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