/*
**  Find smallest window within a text that contains all the specified characters
*/
namespace WindowMatcher
{
    using System.Collections.Generic;

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

    class WindowMatcher<T>
    {
        List<T> sequence;
        
        HashSet<T> interestingTokens;
        
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

        public WindowMatcher(List<T> sequence, HashSet<T> tokens)
        {
            this.sequence = sequence;
            this.interestingTokens = tokens;
            this.positions = new Dictionary<T, List<int>>();
        }

        public Window Scan()
        {
            while(true)
            {
                bool rightProgress = AdvanceRight();

                bool leftProgress = AdvanceLeft();

                // If we cannot advance either left or right, we are done
                if( !rightProgress && !leftProgress )
                {
                    if(this.distinctTokenCountInCW == this.interestingTokens.Count)
                    {
                        // If we have covered all interesting tokens, we have a valid window
                        return this.mw;
                    }
                    else
                    {
                        // Otherwise, we have no valid window
                        return Window.Sentinel;
                    }
                }
            }
        }

        // Advance right end of the cursor
        private bool AdvanceRight()
        {
            bool progress = false;
            while(true)
            {
                // On right end, break;
                if (this.cw.Right == sequence.Count - 1)
                    break;
                
                var thisToken = this.sequence[this.cw.Right];

                // If current token is interesting
                if (interestingTokens.Contains(thisToken))
                {
                    if (!this.positions.ContainsKey(thisToken))
                    {
                        // If seeing it first time
                        this.distinctTokenCountInCW++;
                        this.positions[thisToken] = new List<int>() { this.cw.Right };
                    }
                    else
                    {
                        // If we have seen it before
                        this.positions[thisToken].Add(this.cw.Right);
                    }

                    CheckAndUpdateMinWindow();
                }

                this.cw.Right++;
                progress = true;
            }

            return progress;
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

        // Advance left end of the cursor
        private bool AdvanceLeft()
        {
            bool progress = false;

            // Advance left cursor to skip all non-interesting tokens and tokens that have more than one occurence
            while(true)
            {
                var thisToken = this.sequence[this.cw.Left];

                // If left end has hit right end, break
                if(this.cw.Left == this.cw.Right)
                    break;

                CheckAndUpdateMinWindow();

                // If current token is not interesting, skip it
                if(!interestingTokens.Contains(thisToken))
                {
                    this.cw.Left++;
                    progress = true;
                    continue;
                }

                if(this.interestingTokens.Contains(thisToken) && this.positions[thisToken].Count > 1)
                {
                    // If we have same token more than once in the current window, we can advance left
                    this.positions[thisToken].RemoveAt(0);
                    this.cw.Left++;
                    progress = true;
                    continue;
                }

                // The current token is interesting and we have seen it only once, we are done
                break;
            }

            return progress;
        }
    }
}