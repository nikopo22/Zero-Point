using Microsoft.Xna.Framework;
using ZeroPoint.Entities;
using ZeroPoint.Utils;

namespace ZeroPoint.Core;

public class Camera
{
    //матрица определяет сдвиг всего изображения на экране
    public Matrix Transform { get; private set; }

    //текущая позиция камеры 
    private Vector2 position;

    // следит за игроком, центрируя его на экране
    public void Follow(Player player)
    {
        position.X = player.Position.X + Constants.PLAYER_WIDTH / 2 - Constants.SCREEN_WIDTH / 2;
        position.Y = player.Position.Y + Constants.PLAYER_HEIGHT / 2 - Constants.SCREEN_HEIGHT / 2;

        position.X = MathHelper.Clamp(position.X, 0, 2000 - Constants.SCREEN_WIDTH);
        position.Y = MathHelper.Clamp(position.Y, 0, 1000 - Constants.SCREEN_HEIGHT);

        Transform = Matrix.CreateTranslation(new Vector3(-position, 0));
    }
}
