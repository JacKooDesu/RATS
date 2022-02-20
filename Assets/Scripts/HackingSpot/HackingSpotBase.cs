using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HackingSpotBase : MonoBehaviour
{
    protected Action action;

    protected bool isHacked = false;

    public void Hack()
    {
        if (isHacked)
            return;

        action.Invoke();
        isHacked = true;
    }
}
