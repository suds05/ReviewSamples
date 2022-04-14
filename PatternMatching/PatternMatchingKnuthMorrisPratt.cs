using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternMatching
{
    /// <summary>
    /// Pattern matching via Knuth Morris Pratt algorithm
    /// </summary>
    class PatternMatchingKnuthMorrisPratt
    {
        /// <summary>
        /// Efficient LPS calculation function. 
        /// LPS is the "Go Back To" position in a pattern after a mismatch has been found at specified position
        /// It is the length of (or position just after) the 'Longest proper Prefix that's also a Suffix' at specified position
        /// This is a O(M) algorithm - linear time
        /// Calculate LPS function at each offset
        /// LPS at any offset is the position just after the
        ///     Longest Proper Prefix of the pattern
        ///     that's also a Suffix ending at that offset.
        /// </summary>
        /// <param name="pattern">pattern used in KNP pattern match</param>
        /// <returns>LSP values for each offset</returns>
        public int[] ComputeLPS(string pattern)
        {
            int[] lps = new int[pattern.Length + 1];
            lps[0] = 0;
            for (int i = 1; i <= pattern.Length; i++)
            {
                if (i == pattern.Length)
                {
                    // If pattern has ended, use LPS of previous character (i.e go back to wherever you would if prev character hadn't matched)
                    lps[i] = lps[i - 1];
                }
                else if (pattern[i] == pattern[lps[i - 1]])
                {
                    // If the last character in the suffix is equal to character after the previous charater's LPS
                    // Extend the previous prefix
                    lps[i] = lps[i - 1] + 1;
                }
                else
                {
                    lps[i] = 0;
                }
            }

            return lps;
        }

        /// <summary>
        /// Find this pattern within the text
        /// </summary>
        /// <param name="pattern">pattern to search for</param>
        /// <param name="text">text to search in</param>
        /// <returns>positions in the text that has this pattern</returns>
        public int[] Find(string text, string pattern, int[] lps)
        {
            List<int> result = new List<int>();

            // Position within the pattern
            int iP = 0;

            // As per KMP algorithm, we move straight thru the text. We never need to go back
            for(int iT = 0; iT < text.Length; iT++)
            {
                // char in text and pattern match. Advance both.
                if (pattern[iP] == text[iT])
                {
                    iP++;
                    if (iP == pattern.Length)
                    {
                        // A match has been found. Add it to result
                        result.Add(iT - iP + 1);

                        // Reset the pattern index
                        iP = lps[iP];
                    }
                }
                else
                {
                    // Reset the pattern index by goint back to the LPS position for that offset
                    iP = lps[iP];
                }
            }

            return result.ToArray();
        }
    }
}
