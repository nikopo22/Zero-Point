namespace ZeroPoint.Abilities;

public class MagnetAbility : TimedAbility
{
    private const float MAGNET_DURATION = 2f; // 2 секунды

    public MagnetAbility() : base("Магнит", MAGNET_DURATION) { }
}
