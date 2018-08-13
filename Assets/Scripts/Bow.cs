using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bow : LongRanged_Weapon
{
    private void Start()
    {
        army = SM.GetArmy(transform.parent.parent);

        allPossibleAttacks = new Attack[] { new Attack_Shoot() };
        projectiles = new string[] { "Arrow" };

        deltaTar = 0.08f; // per 1
        minRange = 5f;
        maxRange = 30f;
        weight = 1f;
    }
}
