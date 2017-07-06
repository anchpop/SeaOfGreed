using System;
using System.Collections.Generic;

public class RectanglePacker
{
    public float Width { get; private set; }
    public float Height { get; private set; }

    List<Node> nodes = new List<Node>();

    public RectanglePacker()
    {
        nodes.Add(new Node(0, 0, float.MaxValue, float.MaxValue));
    }

    public bool Pack(float w, float h, out float x, out float y)
    {
        for (int i = 0; i < nodes.Count; ++i)
        {
            if (w <= nodes[i].W && h <= nodes[i].H)
            {
                var node = nodes[i];
                nodes.RemoveAt(i);
                x = node.X;
                y = node.Y;
                float r = x + w;
                float b = y + h;
                nodes.Add(new Node(r, y, node.Right - r, h));
                nodes.Add(new Node(x, b, w, node.Bottom - b));
                nodes.Add(new Node(r, b, node.Right - r, node.Bottom - b));
                Width = Math.Max(Width, r);
                Height = Math.Max(Height, b);
                return true;
            }
        }
        x = 0;
        y = 0;
        return false;
    }

    public struct Node
    {
        public float X;
        public float Y;
        public float W;
        public float H;

        public Node(float x, float y, float w, float h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public float Right
        {
            get { return X + W; }
        }

        public float Bottom
        {
            get { return Y + H; }
        }
    }
}