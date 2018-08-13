using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Long_Sword : Melee_Weapon
{
    private void Start()
    {
        army = SM.GetArmy(transform.parent.parent);

        allPossibleAttacks = new Attack[] { new Attack_Swing(), new Attack_FastPierce() };
        effects = new WEffect[]{SM.MakeBleed};

        minRange = 0f;
        maxRange = 2.6f;
        weight = 1.5f;
        minBasicDmg = 30f;
        maxBasicDmg = 40f;
        critPoss = 0.1f;
        critMult = 4f;
    }
}
