using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightHackingSpot : HackingSpotBase
{
    public List<Light2D> lights = new List<Light2D>();
    private void OnEnable()
    {
        action = () =>
        {
            print("Light Hacked!");
            foreach (var l in lights)
                l.color = Color.black;
        };
    }
}
