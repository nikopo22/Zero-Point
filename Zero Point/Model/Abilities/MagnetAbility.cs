namespace ZeroPoint.Abilities;

public class MagnetAbility : TimedAbility
{
    private const float MAGNET_DURATION = 2f; 

    public MagnetAbility() : base("Магнит", MAGNET_DURATION) { }
}
