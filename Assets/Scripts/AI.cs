using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour
{
    private List<Soldier> targets;
    private Soldier warlord;
    public bool active;

    public string aiArmy;
    private bool keepFormation = false;

    private void Start()
    {
        targets = new List<Soldier>();
        Transform warlordTr = aiArmy == "B" ? SM.bWarlord : SM.rWarlord;
        warlord = warlordTr.GetComponent<Soldier>();
    }

    private void Update()
    {
        if (active && Time.timeScale != 0)
        {
            Transform enemyWarlord = aiArmy == "B" ? SM.rWarlord : SM.bWarlord;
            if (targets.Count == 0)
            {
                warlord.AttackNow(enemyWarlord.position);
            }
            else
            {
                warlord.DefendNow();
                if (Time.timeScale > 0)
                {
                    Attack(enemyWarlord.position);
                }
            }
        }
    }

    public void SetActive(bool b)
    {
        active = b;
    }

    private Vector3 lastPos;

    private void Attack(Vector3 pos)
    {
        if (!keepFormation)
        {
            foreach (Soldier off in targets)
            {
                Soldier[] subs = SM.AsOfficer(off).subs;
                off.AttackNow(pos);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.AttackNow(SM.AsPrivate(sold).PrivPos(pos));
            }
        }
        else
        {
            Vector3 cent = Vector3.zero;
            foreach (Soldier off in targets)
            {
                cent += off.transform.position;
            }
            cent /= targets.Count;
            foreach (Soldier off in targets)
            {
                Vector3 coord = pos + off.transform.position - cent;
                Soldier[] subs = SM.AsOfficer(off).subs;
                off.AttackNow(coord);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.AttackNow(SM.AsPrivate(sold).PrivPos(coord));
            }
        }
    }

    private void Move(Vector3 pos)
    {
        if (!keepFormation)
        {
            foreach (Soldier off in targets)
            {
                Soldier[] subs = SM.AsOfficer(off).subs;
                off.MoveNow(pos);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.MoveNow(SM.AsPrivate(sold).PrivPos(pos));
            }
        }
        else
        {
            Vector3 cent = Vector3.zero;
            foreach (Soldier off in targets)
            {
                cent += off.transform.position;
            }
            cent /= targets.Count;
            foreach (Soldier off in targets)
            {
                Vector3 coord = pos + off.transform.position - cent;
                Soldier[] subs = SM.AsOfficer(off).subs;
                off.MoveNow(coord);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.MoveNow(SM.AsPrivate(sold).PrivPos(coord));
            }
        }
    }

    private void Defend()
    {
        foreach (Soldier off in targets)
        {
            Soldier[] subs = SM.AsOfficer(off).subs;
            off.DefendNow(3f);
            foreach (Soldier sold in subs)
                if (sold != null)
                    sold.DefendNow(SM.AsPrivate(sold).PrivPos(), 3f);
        }
    }

    public void plusTarget(Soldier target)
    {
        targets.Add(target);
    }

    public void minusTarget(Soldier target)
    {
        targets.Remove(target);
    }

    public bool Contains(Soldier target)
    {
        return targets.Contains(target);
    }
}
