using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Warlord : Soldier_Upper
{
    void Start()
    {
        StartNow();

        transform.name = army + "_Warlord";
        switch (army)
        {
            case "R":
                SM.rWarlord = transform.Find("Soldier").transform;
                break;
            case "B":
                SM.bWarlord = transform.Find("Soldier").transform;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        UpdateNow();
    }
}
