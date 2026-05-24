using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeroPoint.Utils;

namespace ZeroPoint.Entities;

public class HiddenPlatform
{
    public Rectangle Bounds { get; private set; }
    public bool IsRevealed { get; set; } 

    private Color hiddenColor;   
    private Color revealedColor; 

    public HiddenPlatform(int x, int y, int width, int height)
    {
        Bounds = new Rectangle(x, y, width, height);
        hiddenColor = new Color(80, 80, 80, 50);    
        revealedColor = new Color(150, 180, 200);   
        IsRevealed = false;
    }

}
