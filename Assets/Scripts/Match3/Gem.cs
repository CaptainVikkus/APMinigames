using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public Vector2 toVector()
    {
        return new Vector2(x, y);
    }

    public static bool operator ==(Point a, Point b)
        => a.x == b.x && a.y == b.y;
    public static bool operator !=(Point a, Point b)
        => a.x != b.x || a.y != b.y;

    public static Point operator +(Point a, Point b)
        => new Point(a.x + b.x, a.y + b.y);
    public static Point operator -(Point a, Point b)
        => new Point(a.x - b.x, a.y - b.y);

    public static Point operator +(Point p, int s)
        => new Point(p.x + s, p.y + s);
    public static Point operator -(Point p, int s)
    => new Point(p.x - s, p.y - s);

    public static Point operator *(Point p, int s)
        => new Point(p.x* s, p.y* s);
    public static Point toPoint(Vector2 v)
    {
        return new Point((int)v.x, (int)v.y);
    }
    public static Point toPoint(Vector3 v)
    {
        return new Point((int)v.x, (int)v.y);
    }
    public static Point clone(Point p)
    {
        return new Point(p.x, p.y);
    }

    public static readonly Point zero = new Point(0, 0);
    public static readonly Point one = new Point(1, 1);
    public static readonly Point up = new Point(0, 1);
    public static readonly Point down = new Point(0, -1);
    public static readonly Point left = new Point(-1, 0);
    public static readonly Point right = new Point(1, 0);
}

public enum GemType
{
    Hazard = -2,
    Block = -1,
    Square = 1,
    Circle,
    Triangle,
    Star
}

[System.Serializable]
public class Gem
{
    public GemType type;
    public Point location;
    public GemPiece piece;

    public Gem(GemType t, Point p)
    {
        type = t;
        location = p;
    }

    public static bool operator ==(Gem a, Gem b)
        => a.type == b.type;
    public static bool operator !=(Gem a, Gem b)
        => a.type != b.type;
}