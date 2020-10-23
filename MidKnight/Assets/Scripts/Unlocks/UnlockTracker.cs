using UnityEngine;

public class UnlockTracker : MonoBehaviour
{
    //Integer that holds current upgrade state
    [SerializeField]
    private int upgradeState = (int)Unlocks.NewMoon;
    //Get function for the upgradeState
    public int GetUpgradeState()
    {
        return upgradeState;
    }
    //Add functions which add a specific powerup to the upgradeState
    public void AddNewMoon()
    {
        if (!CheckNewMoon())
        {
            upgradeState += (int)Unlocks.NewMoon;
        }
    }
    public void AddCrescentMoon()
    {
        if (!CheckCrescentMoon())
        {
            upgradeState += (int)Unlocks.CrescentMoon;
        }
    }
    public void AddHalfMoon()
    {
        if (!CheckHalfMoon())
        {
            upgradeState += (int)Unlocks.HalfMoon;
        }
    }
    public void AddDash()
    {
        if (!CheckDash())
        {
            upgradeState += (int)Unlocks.Dash;
        }
    }
    public void AddFullMoon()
    {
        if (!CheckFullMoon())
        {
            upgradeState += (int)Unlocks.FullMoon;
        }
    }
    public void AddDoubleJump()
    {
        if (!CheckDoubleJump())
        {
            upgradeState += (int)Unlocks.DoubleJump;
        }
    }
    public void AddMoonBeam()
    {
        if (!CheckMoonBeam())
        {
            upgradeState += (int)Unlocks.MoonBeam;
        }
    }
    public void AddEclipse()
    {
        if (!CheckEclipse())
        {
            upgradeState += (int)Unlocks.Eclipse;
        }
    }
    //Bools to check current state
    public bool CheckNewMoon()
    {
        int toCheck = upgradeState - (int)Unlocks.NewMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.CrescentMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.HalfMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Dash;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.FullMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.DoubleJump;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.MoonBeam;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Eclipse;

        if (toCheck == 0) { return true; }

        return false;
    }
    public bool CheckCrescentMoon()
    {
        int toCheck = upgradeState - (int)Unlocks.CrescentMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.NewMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.HalfMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Dash;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.FullMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.DoubleJump;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.MoonBeam;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Eclipse;

        if (toCheck == 0) { return true; }

        return false;
    }
    public bool CheckHalfMoon()
    {
        int toCheck = upgradeState - (int)Unlocks.HalfMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.NewMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.CrescentMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Dash;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.FullMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.DoubleJump;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.MoonBeam;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Eclipse;

        if (toCheck == 0) { return true; }

        return false;
    }
    public bool CheckDash()
    {
        int toCheck = upgradeState - (int)Unlocks.Dash;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.NewMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.CrescentMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.HalfMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.FullMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.DoubleJump;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.MoonBeam;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Eclipse;

        if (toCheck == 0) { return true; }

        return false;
    }
    public bool CheckFullMoon()
    {
        int toCheck = upgradeState - (int)Unlocks.FullMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.NewMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.CrescentMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.HalfMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Dash;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.DoubleJump;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.MoonBeam;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Eclipse;

        if (toCheck == 0) { return true; }

        return false;
    }
    public bool CheckDoubleJump()
    {
        int toCheck = upgradeState - (int)Unlocks.DoubleJump;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.NewMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.CrescentMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.HalfMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Dash;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.FullMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.MoonBeam;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Eclipse;

        if (toCheck == 0) { return true; }

        return false;
    }
    public bool CheckMoonBeam()
    {
        int toCheck = upgradeState - (int)Unlocks.MoonBeam;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.NewMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.CrescentMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.HalfMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Dash;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.FullMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.DoubleJump;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Eclipse;

        if (toCheck == 0) { return true; }

        return false;
    }
    public bool CheckEclipse()
    {
        int toCheck = upgradeState - (int)Unlocks.Eclipse;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.NewMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.CrescentMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.HalfMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.Dash;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.FullMoon;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.DoubleJump;

        if (toCheck < 0) { return false; }
        if (toCheck == 0) { return true; }

        toCheck -= (int)Unlocks.MoonBeam;

        if (toCheck == 0) { return true; }

        return false;
    }
}

//Enum to check against
enum Unlocks
{
    NewMoon = 1,
    CrescentMoon = 2,
    HalfMoon = 4,
    Dash = 8,
    FullMoon = 16,
    DoubleJump = 32,
    MoonBeam = 64,
    Eclipse = 128
}
