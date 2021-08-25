using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKCalculator
{
    static class Input
    {
        public static KeyboardState keyboardInput;
        public static MouseState mouseInput;
        public static bool[] keys = new bool[132];
        public static bool[] mouse = new bool[3];
        public static int[] prevMousePos = new int[2];
    }
}
