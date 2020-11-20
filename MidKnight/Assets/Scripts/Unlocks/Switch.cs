public class Switch : Enemy
{
    public string switchNumber = "Switch 1";
    private UnlockTracker UT;

    public override void Start()
    {
        base.Start();
        UT = FindObjectOfType<UnlockTracker>();
        
        if (UT.GetKeyValue(switchNumber))
        {
            health = 0;
        }
    }

    protected override void ExtraUpdate() {}

    protected override void OnDeath()
    {
        base.OnDeath();
        UT.SetKey(switchNumber, true);
    }
}
