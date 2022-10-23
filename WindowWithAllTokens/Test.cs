namespace WindowMatcher
{
    using System.Collections.Generic;
    static class Test
    {
        public static void RunTest()
        {
            var wm = new WindowMatcher<char>(
                new List<char>() { 'a', 'x', 'x', 'b', 'x', 'a', 'x', 'x', 'a', 'b', 'x', 'c', 'x', 'x', 'a', 'b', 'c' },
                new HashSet<char>() { 'a', 'b', 'c' });

            wm.Scan();
        }
    }
}