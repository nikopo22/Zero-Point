using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Utils;

namespace ZeroPoint.Entities;

public class Platform
{
    public Rectangle Bounds { get; private set; }  
    private Color color;                           

    public Platform(int x, int y, int width, int height)
    {
        Bounds = new Rectangle(x, y, width, height);
        color = new Color(100, 100, 100);  
    }

}