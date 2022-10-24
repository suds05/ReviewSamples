/*
**  Find smallest window within a text that contains all the specified characters
*/
namespace WindowMatcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A range of the text, defined by its left position and right position
    /// </summary>
    struct Window
    {
        public int Left;
        public int Right;
        public static Window Sentinel = new Window(){ Left = int.MinValue/2, Right = int.MaxValue/2 };

        public int Compare(Window other)
        {
            return ((this.Right - this.Left) - (other.Right - other.Left));
        }

        public override string ToString()
        {
            return $"[{Left}, {Right}]";
        }
    }

    /// <summary>
    /// A class that finds the smallest window within a text that contains all the specified characters
    /// </summary>
    class WindowMatcher<T>
    {
        List<T> sequence;
        
        ISet<T> interestingTokens;
        
        Dictionary<T,List<int>> positions;

        /// <summary>
        /// The minimum window
        /// </summary>
        Window mw = Window.Sentinel;

        /// <summary>
        /// The current window
        /// </summary>
        Window cw = new Window() { Left = 0, Right = 0 };

        /// <summary>
        /// Count of distinct interesting tokens within the current window
        /// Note that this value will monotically increase until we cover all interesting tokens
        /// </summary>
        int distinctTokenCountInCW = 0;

        public WindowMatcher(List<T> sequence, ISet<T> tokens)
        {
            this.sequence = sequence;
            this.positions = new Dictionary<T, List<int>>();
            this.interestingTokens = tokens;
        }

        public Window Scan()
        {
            while(true)
            {
                bool rightProgress = AdvanceRight2();

                bool leftProgress = AdvanceLeft2();

                if (!rightProgress && !leftProgress)
                {
                    break;
                }
                else
                {
                    CheckAndUpdateMinWindow();
                }
            }

            return this.mw;
        }

        /// <summary>
        /// Advance right end fo window to next valid token.
        /// If not possible to advance (or if end of sequence), return false
        /// </summary>
        private bool AdvanceRight2()
        {
            int i = cw.Right;
            while(i < sequence.Count)
            {
                T t = sequence[i];

                // Skip uninteresting tokens
                if (!interestingTokens.Contains(t))
                {
                    i++;
                    continue;
                }

                // If this is a token seen for first time
                if(!positions.ContainsKey(t))
                {
                    positions[t] = new List<int>() { i };
                    distinctTokenCountInCW++;
                }
                else
                {
                    // This is a valid token seen for second or later time. 
                    List<int> thisTokPos = this.positions[t];

                    // If we were at same position last time, advance
                    if(thisTokPos.Last() == i)
                    {
                        i++;
                        continue;
                    }

                    // Else Record its position
                    thisTokPos.Add(i);
                }

                if(this.distinctTokenCountInCW < interestingTokens.Count)
                {
                    // If we do not have all the tokens within the window, keep advancing
                    i++;
                    continue;
                }

                break;
            }

            if( i == sequence.Count || i == cw.Right)
            {
                // If we could advance right (or we are past the end), return false
                return false;
            }
            else
            {
                // Update the current window's right to this position, and signal progress
                cw.Right = i;
                return true;
            }
        }

        /// <summary>
        /// Keep advancing left to pass over invalid tokens, or over valid tokens present again in the window
        /// </summary>
        bool AdvanceLeft2()
        {
            int i = cw.Left;
            while(i < cw.Right)
            {
                T t = sequence[i];

                if(!this.interestingTokens.Contains(t))
                {
                    // Uninteresting token, we can skip
                    i++;
                    continue;
                }

                if(positions[t].Count > 1)
                {
                    // The current token is present again within the window. So we can skip this one.
                    positions[t].RemoveAt(0);
                    i++;
                    continue;
                }

                // This token is interesting, and not present later. We cannot advance
                break;
            }

            if(i == cw.Right || i == cw.Left)
            {
                // If we have reached the end, or unable to progress, signal false
                return false;
            }
            else
            {
                // Update the current window's left to this position, and signal progress
                cw.Left = i;
                return true;
            }
        }

        // If the current window is smaller than min window 
        // and we have covered all interesting tokens, update min window
        private void CheckAndUpdateMinWindow()
        {
            // if our current window is smaller than minimum window, update the minimum
            if (cw.Compare(mw) < 0 && this.distinctTokenCountInCW == this.interestingTokens.Count)
            {
                this.mw = this.cw;
            }
        }
   }
}