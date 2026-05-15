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

    //столкновения игрока с платформами и металлическими поверхностями
    public static void HandleCollisions(Player player, List<Platform> platforms, List<MetalSurface> metalSurfaces)
    {
        player.IsGrounded = false;

        // Проверяем обычные платформы
        CheckPlatformCollisions(player, platforms);
        
        // Проверяем металлические поверхности
        CheckMetalSurfaceCollisions(player, metalSurfaces);
    }

    public static void HandleCollisions(Player player, List<Platform> platforms, List<MetalSurface> metalSurfaces, List<HiddenPlatform> hiddenPlatforms)
    {
        player.IsGrounded = false;

        // Проверяем обычные платформы
        CheckPlatformCollisions(player, platforms);
        
        // Проверяем металлические поверхности
        CheckMetalSurfaceCollisions(player, metalSurfaces);

        // Проверяем скрытые платформы только когда они видимы
        CheckHiddenPlatformCollisions(player, hiddenPlatforms);
    }

    //столкновения игрока с платформами
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

    /// <summary>
    /// Решает коллизию между игроком и поверхностью
    /// </summary>
    private static void ResolveCollision(Player player, Rectangle platformBounds)
    {
        //приземление        
        if (player.Velocity.Y > 0 && player.PreviousBounds.Bottom <= platformBounds.Top + 5)
        {
            player.Position = new Vector2(
                player.Position.X,
                platformBounds.Top - player.Bounds.Height
            );
            //останавливаем падение
            player.Velocity = new Vector2(player.Velocity.X, 0);
            player.IsGrounded = true;      // игрок на земле
        }
        //снизу
        else if (player.Velocity.Y < 0 && player.PreviousBounds.Top >= platformBounds.Bottom - 5)
        {
            player.Position = new Vector2(
                player.Position.X,
                platformBounds.Bottom
            );
            player.Velocity = new Vector2(player.Velocity.X, 0);
        }
        //влево вправо
        else
        {
            //с левой стороны
            if (player.PreviousBounds.Right <= platformBounds.Left + 5)
            {
                player.Position = new Vector2(
                    platformBounds.Left - player.Bounds.Width,
                    player.Position.Y
                );
            }
            //с правой стороны
            else if (player.PreviousBounds.Left >= platformBounds.Right - 5)
            {
                player.Position = new Vector2(
                    platformBounds.Right,
                    player.Position.Y
                );
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
