using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewCode
{
    /// <summary>
    /// Pattern matching via Knuth Morris Pratt algorithm
    /// </summary>
    class PatternMatchingKnuthMorrisPratt
    {
/// <summary>
/// Efficient LPS calculation function. This is a O(M) algorithm - linear time
/// Calculate LPS function at each offset
/// LPS at any offset is the length of 
///     Longest Proper Prefix of the original string
///     that's also a Suffix of the substring ending at that offset.
/// </summary>
/// <param name="pattern">pattern used in KNP pattern match</param>
/// <returns>LSP values for each offset</returns>
int[] ComputeLPS(string pattern)
{
    int[] lps = new int[pattern.Length];
    lps[0] = 0;
    for( int i=1; i<pattern.Length; i++)
    {
        // If the last character in the suffix is equal to last character of prefix
        // Extend the previous prefix
        if (pattern[i] == pattern[lps[i - 1]])
            lps[i] = lps[i - 1] + 1;
        else
            lps[i] = 0;
    }

    return lps;
}

        public void Test()
        {
            Console.WriteLine(
                ComputeLPS("ababababc")
                    .Aggregate(
                        "", 
                        (output, i) => $"{output} {i}"));
        }
    }
}
