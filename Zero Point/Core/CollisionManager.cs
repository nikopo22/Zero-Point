using Microsoft.Xna.Framework;
using ZeroPoint.Entities;
using System.Collections.Generic;

namespace ZeroPoint.Core;

public static class CollisionManager
{
    public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
    {
        return rect1.Intersects(rect2);
    }

    public static void HandleCollisions(Player player, List<Platform> platforms)
    {
        player.IsGrounded = false;

        foreach (var platform in platforms)
        {
            if (CheckCollision(player.Bounds, platform.Bounds))
            {
                // столкновение сверху
                if (player.Velocity.Y > 0 && player.PreviousBounds.Bottom <= platform.Bounds.Top + 5)
                {
                    player.Position = new Vector2(
                        player.Position.X,
                        platform.Bounds.Top - player.Bounds.Height
                    );
                    player.Velocity = new Vector2(player.Velocity.X, 0);
                    player.IsGrounded = true;
                }
                // столкновение снизу
                else if (player.Velocity.Y < 0 && player.PreviousBounds.Top >= platform.Bounds.Bottom - 5)
                {
                    player.Position = new Vector2(
                        player.Position.X,
                        platform.Bounds.Bottom
                    );
                    player.Velocity = new Vector2(player.Velocity.X, 0);
                }
                // столкновение лево-право
                else
                {
                    if (player.PreviousBounds.Right <= platform.Bounds.Left + 5)
                    {
                        player.Position = new Vector2(
                            platform.Bounds.Left - player.Bounds.Width,
                            player.Position.Y
                        );
                    }
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
