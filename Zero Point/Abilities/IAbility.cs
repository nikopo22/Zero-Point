using Microsoft.Xna.Framework;

namespace ZeroPoint.Abilities;

public interface IAbility
{
    string Name { get; }
    //активна ли способность сейчас
    bool IsActive { get; set; }
    //время действия в сек
    float Duration { get; set; }
    //таймер обратного отсчёта
    float CurrentTime { get; set; }

    void Activate();
    void Deactivate();
    void Update(GameTime gameTime);
}
