using System.Linq;
using UnityEngine;

namespace LaunchCountDown.Config
{
    class RectWrapper
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        internal Rect ToRect(string data)
        {
            var items = data.Split(',');

            if (!items.Any() || items.Count() < 4) return new Rect(0, 0, 459, 120);

            X = float.Parse(items[0]);
            Y = float.Parse(items[1]);
            Width = float.Parse(items[2]);
            Height = float.Parse(items[3]);

            return new Rect(X, Y, Width, Height);
        }

        internal void FromRect(Rect source)
        {
            X = source.x;
            Y = source.y;
            Width = source.width;
            Height = source.height;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", X, Y, Width, Height);
        }
    }
}