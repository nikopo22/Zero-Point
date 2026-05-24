namespace ZeroPoint.Abilities;

public class ScanAbility : TimedAbility
{
    private const float SCAN_DURATION = 5f; 

    public ScanAbility() : base("Сканер", SCAN_DURATION) { }
}
