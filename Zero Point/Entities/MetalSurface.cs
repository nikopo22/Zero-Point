using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Utils;

namespace ZeroPoint.Entities;

public class MetalSurface
{
    public Rectangle Bounds { get; private set; }
    private Color color;

    public MetalSurface(int x, int y, int width, int height)
    {
        Bounds = new Rectangle(x, y, width, height);
        color = new Color(150, 140, 130);  
    }

}
