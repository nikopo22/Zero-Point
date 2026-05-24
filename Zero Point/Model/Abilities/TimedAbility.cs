using Microsoft.Xna.Framework;

namespace ZeroPoint.Abilities;

public abstract class TimedAbility : IAbility
{
    public string Name { get; protected set; }
    public bool IsActive { get; set; }
    public float Duration { get; set; }
    public float CurrentTime { get; set; }

    protected TimedAbility(string name, float duration)
    {
        Name = name;
        Duration = duration;
        IsActive = false;
        CurrentTime = 0f;
    }

    public virtual void Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            CurrentTime = Duration;
        }
    }

    public virtual void Deactivate()
    {
        IsActive = false;
        CurrentTime = 0f;
    }

    public virtual void Update(GameTime gameTime)
    {
        if (!IsActive) return;

        CurrentTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (CurrentTime <= 0)
        {
            Deactivate();
        }
    }
}
