using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OpenTKCalculator
{
    static class Input
    {
        public static KeyboardState keyboardInput;
        public static MouseState mouseInput;
        public static bool[] keys = new bool[256];
        public static bool[] mouse = new bool[3];
        //The mouse position on the previous update
        public static Point prevMousePos = new Point(0,0);
        //The mouse position on before the mouse was hidden
        public static Point initMousePos = new Point(0, 0);
    }
}
