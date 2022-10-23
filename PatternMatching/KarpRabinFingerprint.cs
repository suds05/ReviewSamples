using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternMatching
{
    /// <summary>
    /// Karp Rabin Fingerprint.
    /// Aka a Rolling Hash
    /// </summary>
    class KarpRabinFingerprint
    {
        readonly long primeBase;
        readonly int windowWidth;
        long fingerprint;

        public KarpRabinFingerprint(int windowWidth)
        {
            this.primeBase = (1L << 62) - 1;
            this.windowWidth = windowWidth;
        }

        public KarpRabinFingerprint(string pattern)
            : this(pattern.Length)
        {
            Initialize(pattern);
        }

        public void Initialize(string s)
        {
            for(int i=0; i<s.Length; i++)
            {
                this.Update('\0', s[i]);
            }
        }

        /// <summary>
        /// This is the main method - where the 'rolling' nature is in display
        /// The existing hash is rolled over with the incoming character, and outgoing character
        /// </summary>
        /// <param name="outGoing">character incoming into hash window</param>
        /// <param name="incoming">character outgoing into the hash window</param>
        public void Update(char outGoing, char incoming)
        {
            fingerprint = (fingerprint << 1) 
                + (long)incoming 
                - (long)outGoing * (1L << windowWidth);
        }

        public bool Equals(KarpRabinFingerprint other)
        {
            return this.fingerprint == other.fingerprint;
        }
    }

    class KRFingerprintMatcher
    {
        public List<int> GetMatches(string text, string pattern)
        {
            KarpRabinFingerprint patHash = new KarpRabinFingerprint(pattern);
            KarpRabinFingerprint textHash = new KarpRabinFingerprint(text.Substring(0, pattern.Length));
            List<int> matches = new List<int>();

            int i = pattern.Length;
            while (i < text.Length)
            {
                if (patHash.Equals(textHash))
                {
                    matches.Add(i - pattern.Length);
                }

                textHash.Update(text[i - pattern.Length], text[i]);

                i++;
            }

            if (patHash.Equals(textHash))
            {
                matches.Add(i - pattern.Length);
            }

            return matches;
        }
    }
}
