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

    public static void HandleCollisions(Player player, List<Platform> platforms, List<MetalSurface> metalSurfaces, List<HiddenPlatform> hiddenPlatforms = null, List<InvisibleWall> invisibleWalls = null)
    {
        player.IsGrounded = false;

        if (invisibleWalls != null)
        {
            CheckInvisibleWallCollisions(player, invisibleWalls);
        }

        CheckPlatformCollisions(player, platforms);
        
        CheckMetalSurfaceCollisions(player, metalSurfaces);

        if (hiddenPlatforms != null)
        {
            CheckHiddenPlatformCollisions(player, hiddenPlatforms);
        }
    }

    private static void CheckPlatformCollisions(Player player, List<Platform> platforms)
    {
        foreach (var platform in platforms)
        {
            if (CheckCollision(player.Bounds, platform.Bounds))
            {
                ResolveCollision(player, platform.Bounds);
            }
        }
    }

    private static void CheckInvisibleWallCollisions(Player player, List<InvisibleWall> invisibleWalls)
    {
        foreach (var wall in invisibleWalls)
        {
            if (CheckCollision(player.Bounds, wall.Bounds))
            {
                ResolveCollision(player, wall.Bounds);
            }
        }
    }

    private static void CheckMetalSurfaceCollisions(Player player, List<MetalSurface> metalSurfaces)
    {
        foreach (var metal in metalSurfaces)
        {
            if (CheckCollision(player.Bounds, metal.Bounds))
            {
                ResolveCollision(player, metal.Bounds);
            }
        }
    }

    private static void CheckHiddenPlatformCollisions(Player player, List<HiddenPlatform> hiddenPlatforms)
    {
        foreach (var hidden in hiddenPlatforms)
        {
            if (!hidden.IsRevealed)
                continue;

            if (CheckCollision(player.Bounds, hidden.Bounds))
            {
                ResolveCollision(player, hidden.Bounds);
            }
        }
    }

    private static void ResolveCollision(Player player, Rectangle platformBounds)
    {       
        if (player.Velocity.Y > 0 && player.PreviousBounds.Bottom <= platformBounds.Top + 5)
        {
            player.Position = new Vector2(
                player.Position.X,
                platformBounds.Top - player.Bounds.Height
            );
            player.Velocity = new Vector2(player.Velocity.X, 0);
            player.IsGrounded = true;     
        }

        else if (player.Velocity.Y < 0 && player.PreviousBounds.Top >= platformBounds.Bottom - 5)
        {
            player.Position = new Vector2(
                player.Position.X,
                platformBounds.Bottom
            );
            player.Velocity = new Vector2(player.Velocity.X, 0);
        }

        else
        {
  
            if (player.PreviousBounds.Right <= platformBounds.Left + 5)
            {
                player.Position = new Vector2(
                    platformBounds.Left - player.Bounds.Width,
                    player.Position.Y
                );
            }
  
            else if (player.PreviousBounds.Left >= platformBounds.Right - 5)
            {
                player.Position = new Vector2(
                    platformBounds.Right,
                    player.Position.Y
                );
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
