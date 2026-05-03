using Microsoft.Xna.Framework;
using ZeroPoint.Entities;
using System.Collections.Generic;

namespace ZeroPoint.Core;

public static class CollisionManager
{
    //проверяет, пересекаются ли два прямоугольника
    public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
    {
        return rect1.Intersects(rect2); 
    }


    //столкновения игрока с платформами
    public static void HandleCollisions(Player player, List<Platform> platforms)
    {
        player.IsGrounded = false;

        foreach (var platform in platforms)
        {
            if (CheckCollision(player.Bounds, platform.Bounds))
            {
                //приземление        
                if (player.Velocity.Y > 0 && player.PreviousBounds.Bottom <= platform.Bounds.Top + 5)
                {
                    player.Position = new Vector2(
                        player.Position.X,
                        platform.Bounds.Top - player.Bounds.Height
                    );
                    //останавливаем падение
                    player.Velocity = new Vector2(player.Velocity.X, 0);
                    player.IsGrounded = true;      // игрок на земле
                }
                //снизу
                else if (player.Velocity.Y < 0 && player.PreviousBounds.Top >= platform.Bounds.Bottom - 5)
                {
                    player.Position = new Vector2(
                        player.Position.X,
                        platform.Bounds.Bottom
                    );
                    player.Velocity = new Vector2(player.Velocity.X, 0);
                }
                //влево вправо
                else
                {
                    //с левой стороны
                    if (player.PreviousBounds.Right <= platform.Bounds.Left + 5)
                    {
                        player.Position = new Vector2(
                            platform.Bounds.Left - player.Bounds.Width,
                            player.Position.Y
                        );
                    }
                    //с правой стороны
                    else if (player.PreviousBounds.Left >= platform.Bounds.Right - 5)
                    {
                        player.Position = new Vector2(
                            platform.Bounds.Right,
                            player.Position.Y
                        );
                    }
                }
            }
        }
    }

    //коснулся ли игрок шипа
    public static bool CheckSpikeCollision(Player player, List<Spike> spikes)
    {
        foreach (var spike in spikes)
        {
            if (CheckCollision(player.Bounds, spike.Bounds))
            {
                return true;  
            }
        }
        return false; 
    }
}
