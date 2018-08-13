using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Axe : Melee_Weapon
{
    private void Start()
    {
        army = SM.GetArmy(transform.parent.parent);

        allPossibleAttacks = new Attack[] { new Attack_Swing()};
        effects = new WEffect[]{SM.Push};

        minRange = 1.2f;
        maxRange = 4f;
        weight = 5f;
        minBasicDmg = 50f;
        maxBasicDmg = 80f;
        critPoss = 0.04f;
        critMult = 4f;
    }
}
