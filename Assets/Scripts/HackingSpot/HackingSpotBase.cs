using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HackingSpotBase : MonoBehaviour
{
    public enum HackSpotType
    {
        Player,
        Drone,
        Both
    }
    public HackSpotType hackSpotType;
    protected Action action;

    protected bool isHacked = false;

    public void Hack(HackSpotType type)
    {
        if (type != hackSpotType && hackSpotType != HackSpotType.Both)
            return;

        if (isHacked)
            return;

        action.Invoke();
        isHacked = true;
    }
}
