﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace sth1edwv
{
    public class Palette
    {
        private readonly string _label;
        public IList<Color> Colors { get; } = new List<Color>();

        private Palette(byte[] mem, int offset, string label, int paletteCount)
        {
            _label = label;
            for (var i = 0; i < 16*paletteCount; i++)
            {
                var color = mem[offset + i];
                var r = ScaleColor((color >> 0) & 0b11);
                var g = ScaleColor((color >> 2) & 0b11);
                var b = ScaleColor((color >> 4) & 0b11);
                Colors.Add(Color.FromArgb(r, g, b));
            }
        }

        public override string ToString()
        {
            return _label;
        }

        private static int ScaleColor(int c)
        {
            switch (c)
            {
                case 0:
                    return 0;
                case 1:
                    return 80;
                case 2:
                    return 175;
                case 3:
                    return 255;
            }
            return 0;
        }

        public static IEnumerable<Palette> ReadPalettes(byte[] mem, int offset, int count)
        {
            var counts = new[]
            {
                2, 2, 2, 2, 2, 2, 2, 2, // Regular tile+sprite palettes
                3, 3, 3, 3, 4, 12, 4, 1, 4 // Palette 
            };

            for (var i = 0; i < count; i++)
            {
                var address = BitConverter.ToUInt16(mem, offset);
                yield return new Palette(mem, address, $"{i} @ {offset:X}", counts[i]);
                offset += 2;
            }
        }

        public Image ToImage(int width)
        {
            var rows = Colors.Count / 16;
            var rectSize = width / 16;
            var bmp = new Bitmap(width, rows * rectSize);
            using var g = Graphics.FromImage(bmp);
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < 16; x++)
                {
                    var index = x + y * 16;
                    using var b = new SolidBrush(Colors[index]);
                    g.FillRectangle(b, rectSize*x, rectSize*y, rectSize, rectSize);
                }
            }

            return bmp;
        }
    }
}