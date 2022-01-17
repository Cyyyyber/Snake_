﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_CSharpGame
{
    internal class Settings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static string directions;

        public Settings()
        {
            Width = 16;
            Height = 16;
            directions = "left"; //default direction
        }
    }
}
