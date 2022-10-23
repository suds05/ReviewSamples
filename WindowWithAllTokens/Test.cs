namespace WindowMatcher
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    static class Test
    {
        public static void RunTest1()
        {
            var w1 = new Window() { Left = 0, Right = 1 };
            var w2 = new Window() { Left = 0, Right = 2 };
            Trace.Assert(w1.Compare(w2) < 0, "Window of 1 should be lesser than window of 2");
            Trace.Assert(w2.Compare(w1) > 0, "Window of 2 should be greater than window of 1");
            Trace.Assert(w1.Compare(w1) == 0, "Window of 1 should be equal to window of 1");
            Trace.Assert(w2.Compare(Window.Sentinel) < 0, "Window of 2 should be lesser than sentinel");
            Trace.Assert(Window.Sentinel.Compare(w2) > 0, "Sentinel should be bigger");
        }

        public static void RunTest2()
        {
            var wm = new WindowMatcher<char>(
                new List<char>() { 'a', 'x', 'x', 'b', 'x', 'a', 'x', 'x', 'a', 'b', 'x', 'c', 'x', 'x', 'a', 'b', 'c' },
                new HashSet<char>() { 'a', 'b', 'c' });

            var w = wm.Scan();

            Trace.Assert(w.Left == 14 && w.Right == 16, "Window should be [14, 16]");
        }
    }
}