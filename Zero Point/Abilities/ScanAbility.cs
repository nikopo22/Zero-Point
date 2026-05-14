namespace ZeroPoint.Abilities;

public class ScanAbility : TimedAbility
{
    private const float SCAN_DURATION = 2f; // 2 секунды

    public ScanAbility() : base("Сканер", SCAN_DURATION) { }
}
