using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arrow : Projectile_Weapon
{
    private void Start()
    {
        effects = new WEffect[0];

        minBasicDmg = 30f;
        maxBasicDmg = 40f;
        critPoss = 0.25f;
        critMult = 3f;
    }
}
