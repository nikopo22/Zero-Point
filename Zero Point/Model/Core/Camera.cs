using Microsoft.Xna.Framework;
using ZeroPoint.Entities;
using ZeroPoint.Utils;
using System.Collections.Generic;

namespace ZeroPoint.Core;

public class Camera
{
    public Matrix Transform { get; private set; }
    private Vector2 position;
    public Vector2 CameraPosition => position;
    public void Follow(Player player, List<InvisibleWall> invisibleWalls = null)
    {
        position.X = player.Position.X + Constants.PLAYER_WIDTH / 2 - Constants.SCREEN_WIDTH / 2;
        position.Y = player.Position.Y + Constants.PLAYER_HEIGHT / 2 - Constants.SCREEN_HEIGHT / 2;

        position.X = MathHelper.Clamp(position.X, 0, Constants.LEVEL_WIDTH - Constants.SCREEN_WIDTH);
        position.Y = MathHelper.Clamp(position.Y, 0, Constants.LEVEL_HEIGHT - Constants.SCREEN_HEIGHT);

        if (invisibleWalls != null)
        {
            AdjustCameraForWalls(invisibleWalls);
        }

        Transform = Matrix.CreateTranslation(new Vector3(-position, 0));
    }

    private void AdjustCameraForWalls(List<InvisibleWall> invisibleWalls)
    {
        Rectangle cameraViewport = new Rectangle((int)position.X, (int)position.Y, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);

        foreach (var wall in invisibleWalls)
        {
            if (!cameraViewport.Intersects(wall.Bounds))
                continue;

            if (wall.Bounds.Right <= cameraViewport.Right && wall.Bounds.Left < cameraViewport.Left + Constants.SCREEN_WIDTH / 2)
            {
                position.X = wall.Bounds.Right;
            }
            else if (wall.Bounds.Left >= cameraViewport.Left && wall.Bounds.Right > cameraViewport.Right - Constants.SCREEN_WIDTH / 2)
            {
                position.X = wall.Bounds.Left - Constants.SCREEN_WIDTH;
            }

            position.X = MathHelper.Clamp(position.X, 0, Constants.LEVEL_WIDTH - Constants.SCREEN_WIDTH);
        }
    }
}
