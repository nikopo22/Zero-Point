using Microsoft.Xna.Framework;

namespace ZeroPoint.Abilities;

public interface IAbility
{
    string Name { get; }
    bool IsActive { get; set; }
    float Duration { get; set; }
    float CurrentTime { get; set; }

    void Activate();
    void Deactivate();
    void Update(GameTime gameTime);
}
