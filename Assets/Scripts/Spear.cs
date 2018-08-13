using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spear : Melee_Weapon
{
    private void Start()
    {
        army = SM.GetArmy(transform.parent.parent);

        allPossibleAttacks = new Attack[] { new Attack_FastPierce() };
        effects = new WEffect[] { SM.Push };

        minRange = 2.6f;
        maxRange = 3.6f;
        weight = 2f;
        minBasicDmg = 50f;
        maxBasicDmg = 70f;
        critPoss = 0.3f;
        critMult = 3f;
    }
}
