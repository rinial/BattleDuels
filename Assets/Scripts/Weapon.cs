using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Weapon : InHand
{
    protected string army;

    protected Attack[] allPossibleAttacks;
    protected WEffect[] effects;

    protected float minRange;
    public float maxRange;
    protected float weight;
    protected float minBasicDmg;
    protected float maxBasicDmg;
    protected float critPoss;
    protected float critMult;
    protected float lastAttack = 0f;
    protected float attackTime = 10f;
    protected float dmgDecK = 1f;
    protected float dmgK = 0f;
    protected float dKbyClass
    {
        get
        {
            Soldier sold;
            try
            {
                sold = transform.parent.parent.GetComponent<Soldier>();
            }
            catch
            {
                return 1f;
            }
            return sold ? (SM.IsWarlord(sold) ? 2f : SM.IsOfficer(sold) ? 1.5f : 1f) : 1f;
        }
    }

    public int InAttackRange(float toEnemy)
    {
        return toEnemy > maxRange ? 1 : toEnemy < minRange ? -1 : 0;
    }

    public abstract float Attack(out float attackEndToUse, float endurance, Transform target);

    protected void GetPossibleAttacks(float endurance, out List<Attack> possibleAttacks)
    {
        possibleAttacks = new List<Attack>();
        foreach (Attack att in allPossibleAttacks)
        {
            if (att.endUse <= endurance)
                possibleAttacks.Add(att);
        }
    }
}
