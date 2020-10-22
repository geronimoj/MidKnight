using System.Collections;
using System.Collections.Generic;
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
    //Set function for the upgradeState
    public void SetUpgradeState(int setToo)
    {
        upgradeState = setToo;
    }
    //Bools to check current state
    private bool CheckNewMoon()
    {
        if (upgradeState >= (int)Unlocks.NewMoon)
        {
            return true;
        }

        return false;
    }
    private bool CheckCrescentMoon()
    {
        if (upgradeState >= (int)Unlocks.CrescentMoon)
        {
            return true;
        }

        return false;
    }
    private bool CheckHalfMoon()
    {
        if (upgradeState >= (int)Unlocks.HalfMoon)
        {
            return true;
        }

        return false;
    }
    private bool CheckDash()
    {
        if (upgradeState >= (int)Unlocks.Dash)
        {
            return true;
        }

        return false;
    }
    private bool CheckFullMoon()
    {
        if (upgradeState >= (int)Unlocks.FullMoon)
        {
            return true;
        }

        return false;
    }
    private bool CheckDoubleJump()
    {
        if (upgradeState >= (int)Unlocks.DoubleJump)
        {
            return true;
        }

        return false;
    }
    private bool CheckMoonBeam()
    {
        if (upgradeState >= (int)Unlocks.MoonBeam)
        {
            return true;
        }

        return false;
    }
    private bool CheckEclipse()
    {
        if (upgradeState >= (int)Unlocks.Eclipse)
        {
            return true;
        }

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
