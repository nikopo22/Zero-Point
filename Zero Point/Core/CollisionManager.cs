using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZeroPoint.Entities;
using ZeroPoint.Utils;

namespace ZeroPoint.Core;

public static class CollisionManager
{
    public static bool CheckCollision(Rectangle rect1, Rectangle rect2) => rect1.Intersects(rect2);

    public static void HandleCollisions(Player player, List<Platform> platforms, List<MetalSurface> metalSurfaces, List<HiddenPlatform> hiddenPlatforms = null, List<InvisibleWall> invisibleWalls = null)
    {
        player.IsGrounded = false;

        if (platforms.Count > 0)
        {
            var platformTree = BuildQuadTree(platforms, platform => platform.Bounds);
            var candidates = platformTree.Query(player.Bounds, new List<Platform>());
            foreach (var platform in candidates)
                ResolveCollision(player, platform.Bounds);
        }

        if (invisibleWalls != null && invisibleWalls.Count > 0)
        {
            var wallTree = BuildQuadTree(invisibleWalls, wall => wall.Bounds);
            var candidates = wallTree.Query(player.Bounds, new List<InvisibleWall>());
            foreach (var wall in candidates)
                ResolveCollision(player, wall.Bounds);
        }

        if (metalSurfaces.Count > 0)
        {
            var metalTree = BuildQuadTree(metalSurfaces, metal => metal.Bounds);
            var candidates = metalTree.Query(player.Bounds, new List<MetalSurface>());
            foreach (var metal in candidates)
                ResolveCollision(player, metal.Bounds);
        }

        if (hiddenPlatforms != null && hiddenPlatforms.Count > 0)
        {
            var hiddenTree = BuildQuadTree(hiddenPlatforms, hidden => hidden.Bounds);
            var candidates = hiddenTree.Query(player.Bounds, new List<HiddenPlatform>());
            foreach (var hidden in candidates)
            {
                if (!hidden.IsRevealed)
                    continue;

                ResolveCollision(player, hidden.Bounds);
            }
        }
    }

    private static QuadTree<T> BuildQuadTree<T>(IEnumerable<T> items, Func<T, Rectangle> boundsAccessor)
    {
        var itemList = items as IList<T> ?? new List<T>(items);
        var bounds = CreateBounds(itemList, boundsAccessor);
        var quadTree = new QuadTree<T>(bounds, boundsAccessor);

        foreach (var item in itemList)
            quadTree.Insert(item);

        return quadTree;
    }

    private static Rectangle CreateBounds<T>(IList<T> items, Func<T, Rectangle> boundsAccessor)
    {
        if (items.Count == 0)
            return new Rectangle(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (var item in items)
        {
            var itemBounds = boundsAccessor(item);
            minX = Math.Min(minX, itemBounds.X);
            minY = Math.Min(minY, itemBounds.Y);
            maxX = Math.Max(maxX, itemBounds.Right);
            maxY = Math.Max(maxY, itemBounds.Bottom);
        }

        int width = Math.Max(1, maxX - minX);
        int height = Math.Max(1, maxY - minY);

        return new Rectangle(minX, minY, width, height);
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
        if (spikes == null || spikes.Count == 0)
            return false;

        var spikeTree = BuildQuadTree(spikes, spike => spike.Bounds);
        var candidates = spikeTree.Query(player.Bounds, new List<Spike>());

        foreach (var spike in candidates)
        {
            if (CheckCollision(player.Bounds, spike.Bounds))
                return true;
        }

        return false;
    }
}
