using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Officer : Soldier_Upper
{
    public Soldier[] subs
    {
        get;
        private set;
    }
    public int freeIndex
    {
        get
        {
            for (int i = 0; i < subs.Length; i++)
            {
                Soldier s = subs[i];
                if (s == null)
                {
                    return i; ;
                }
            }
            return -1;
        }
    }
    public Vector3[] formation
    {
        get
        {
            return new Vector3[] { new Vector3(3, 0), new Vector3(-3, 0), new Vector3(0, 3), new Vector3(0, -3), new Vector3(3, 3), new Vector3(-3, 3), new Vector3(3, -3), new Vector3(-3, -3) };
        }
    }

    void Start()
    {
        StartNow();

        transform.name = army + "_Officer";

        subs = new Soldier[formation.Length];

        for (int i = 0; i < formation.Length; i++)
        {
            Vector3 pos = formation[i];
            GameObject priv = (GameObject)Instantiate((GameObject)Resources.Load("Private"), transform.position + Vector3.right * pos.x + Vector3.up * pos.y, transform.rotation);
            priv.GetComponent<Private>().deltaV = pos;
            priv.GetComponent<Private>().index = i;
            //priv.transform.parent = soldiers;
            priv.GetComponent<Private>().army = army;
            priv.GetComponent<Private>().eqSet = eqSet;
            priv.transform.name = army + "_Private";
            subs[i] = priv.transform.Find("Soldier").GetComponent<Soldier>();
        }

        foreach (Soldier sub in subs)
        {
            SM.AsPrivate(sub).SetOfficer(this);
        }

        foreach (AI ai in FindObjectsOfType<AI>())
            if (army == ai.aiArmy)
                ai.plusTarget(transform.Find("Soldier").GetComponent<Soldier>());
    }

    void Update()
    {
        UpdateNow();

        float takeRad = 10f;
        Collider2D[] soldiersToTake = Physics2D.OverlapCircleAll(transform.position, takeRad, 1 << SM.SoldierLayer);
        foreach (Collider2D sold in soldiersToTake)
        {
            Transform soldier = sold.transform;
            int index = freeIndex;
            if (SM.GetArmy(soldier) == army && index != -1 && SM.IsPrivate(soldier.GetComponent<Soldier>()))
            {
                Private priv = SM.AsPrivate(soldier.GetComponent<Soldier>());
                if (!priv.officer && priv.eqSet == eqSet)
                {
                    Vector3 pos = formation[index];
                    priv.deltaV = pos;
                    priv.index = index;
                    subs[index] = priv.transform.Find("Soldier").GetComponent<Soldier>();
                    priv.SetOfficer(this);
                    soldier.GetComponent<Soldier>().moral = soldier.GetComponent<Soldier>().moral > 50f ? soldier.GetComponent<Soldier>().moral : 50f;
                    soldier.GetComponent<Soldier>().DefendNow();
                    if (CM.Contains(this.transform.Find("Soldier").GetComponent<Soldier>()))
                    {
                        soldier.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_HL_" + army);
                    }
                    else
                        soldier.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_" + army);
                }
            }
        }
    }
}
