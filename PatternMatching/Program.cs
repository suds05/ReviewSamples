namespace PatternMatching
{
    using System;
    using System.Linq;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            RunKrfAndValidate("aba", "ababababc", new int[] { 0, 2, 4 });
        }

        public static void RunKrfAndValidate(string pattern, string text, int[] matchesExpected)
        {
            var krf = new KRFingerprintMatcher();
            var matches = krf.GetMatches(text, pattern);

            for (int i = 0; i < matchesExpected.Length; i++)
            {
                Debug.Assert(matches[i] == matchesExpected[i], $"Match at position {i} does not match! Expected {matchesExpected[i]}, found {matches[i]}");
            }

            Console.WriteLine(
                    matches.Aggregate("Text matches pattern at positions: ", (output, i) => $"{output} {i}"));
        }

        public void TestKmp()
        {
            RunKmpAndValidate("aba", "ababababc", new int[] { 0, 0, 1, 1 }, new int[] { 0, 2, 4 });
            RunKmpAndValidate("aaa", "aaaaac", new int[] { 0, 1, 2, 2 }, new int[] { 0, 1, 2 });
            RunKmpAndValidate("ababababc", "ababababc", new int[] { 0, 0, 1, 2, 3, 4, 5, 6, 0, 0 }, new int[] { 0 });
        }

        public static void RunKmpAndValidate(string pattern, string text, int[] kmpExpected, int[] matchesExpected)
        {
            var kmp = new PatternMatchingKnuthMorrisPratt();

            var lps = kmp.ComputeLPS(pattern);

            for (int i = 0; i < kmpExpected.Length; i++)
            {
                Debug.Assert(lps[i] == kmpExpected[i], $"LPS at position {i} does not match! Expected {kmpExpected[i]}, found {lps[i]}");
            }

            Console.WriteLine( 
                lps.Aggregate("LPS positions: ", (output, i) => $"{output} {i}"));

            var matches = kmp.Find(text, pattern, lps);

            for (int i = 0; i < matchesExpected.Length; i++)
            {
                Debug.Assert(matches[i] == matchesExpected[i], $"Match at position {i} does not match! Expected {matchesExpected[i]}, found {matches[i]}");
            }

            Console.WriteLine(
                    matches.Aggregate("Text matches pattern at positions: ", (output, i) => $"{output} {i}"));
        }
    }
}
