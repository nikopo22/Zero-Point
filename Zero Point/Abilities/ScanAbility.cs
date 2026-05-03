using Microsoft.Xna.Framework;

namespace ZeroPoint.Abilities;

public class ScanAbility : IAbility
{
    public string Name => "Сканер";
    public bool IsActive { get; set; }
    public float Duration { get; set; }
    public float CurrentTime { get; set; }

    public ScanAbility()
    {
        IsActive = false;
        Duration = 2f;      //2 секунды
        CurrentTime = 0f;
    }

    public void Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            CurrentTime = Duration;
        }
    }

    public void Deactivate()
    {
        IsActive = false;
        CurrentTime = 0f;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsActive) return;

        CurrentTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (CurrentTime <= 0)
        {
            Deactivate();
        }
    }
}

