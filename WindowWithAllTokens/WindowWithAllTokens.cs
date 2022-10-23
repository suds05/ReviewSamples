/*
**  Find smallest window within a text that contains all the specified characters
*/
namespace WindowMatcher
{
    using System.Collections.Generic;

    struct Window
    {
        public int Left;
        public int Right;
        public static Window Sentinel = new Window(){};
    }

    class WindowMatcher<T>
    {
        List<T> text;
        HashSet<T> tokens;
        Window minW = Window.Sentinel;
        Window cursor = new Window() { Left = 0, Right = 0 };

        public WindowMatcher(List<T> text, HashSet<T> tokens)
        {
            this.text = text;
            this.tokens = tokens;
        }

        public Window Scan()
        {
            // todo: put implementation here
            return Window.Sentinel;
        }
    }
}