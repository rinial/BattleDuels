using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Melee_Weapon : Weapon
{
    public override float Attack(out float attackEndToUse, float endurance, Transform target = null)
    {
        List<Attack> possibleAttacks;
        GetPossibleAttacks(endurance, out possibleAttacks);
        if (possibleAttacks.Count == 0f)
        {
            attackEndToUse = 0f;
            return (-1f);
        }
        Attack att = possibleAttacks[SM._rnd.Next(possibleAttacks.Count)];
        dmgK = att.dmgK;
        lastAttack = Time.time;
        attackTime = att.attackTime;

        float type = att.AttackNow(out attackEndToUse);
        attackEndToUse = attackEndToUse * weight;
        return type;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == SM.SoldierLayer && SM.GetArmy(col.transform) != army && dmgK > 0f)
        {
            float basicDmg = (float)(SM._rnd.NextDouble()) * (maxBasicDmg - minBasicDmg) + minBasicDmg;
            bool isCritical = SM._rnd.NextDouble() <= critPoss;
            basicDmg = isCritical ? basicDmg * critMult : basicDmg;
            float dmg = basicDmg * dmgDecK * dmgK * dKbyClass;

            foreach (WEffect eff in effects)
            {
                eff(col.transform, (col.transform.position - transform.parent.parent.position).normalized, dmg);
            }

            col.transform.GetComponent<Soldier>().TakeDamage(dmg);
            //Debug.Log(army + ": " + dmg + ((isCritical || dmgDecK < 1f) ? " (" : "") + (isCritical ? "  critical: x" + critMult : "") + (dmgDecK < 1f ? "  blocked: x" + dmgDecK : "") + ((isCritical || dmgDecK < 1f) ? "  )" : "") + ".");
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == SM.ProtectionLayer)
        {
            dmgDecK = col.transform.GetComponent<Defence>().dmgDecK;
            //transform.parent.parent.GetComponent<Soldier>().StopBattleAnimations();
        }
    }

    private void Update()
    {
        if (Time.time - lastAttack > attackTime)
        {
            dmgDecK = 1f;
            dmgK = 0f;
        }
    }
}
