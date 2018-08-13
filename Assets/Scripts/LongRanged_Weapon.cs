using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class LongRanged_Weapon : Weapon
{
    protected string[] projectiles;
    protected float deltaTar;
    protected bool attacking = false;
    protected Transform Target;
    protected Vector3 lastPos;
    protected float startTime;
    protected GameObject pro;
    protected float projSpeed
    {
        get
        {
            return (float)Math.Sqrt(maxRange * SM.g);
        }
    }

    public override float Attack(out float attackEndToUse, float endurance, Transform target = null)
    {
        if (target == null)
        {
            attackEndToUse = 0f;
            return -1f;
        }

        List<Attack> possibleAttacks;
        GetPossibleAttacks(endurance, out possibleAttacks);
        if (possibleAttacks.Count == 0f)
        {
            attackEndToUse = 0f;
            return (-1f);
        }
        Attack att = possibleAttacks[SM._rnd.Next(possibleAttacks.Count)];
        dmgK = att.dmgK;

        string pr = projectiles[SM._rnd.Next(projectiles.Length)];
        string[] forGolden = transform.GetComponent<SpriteRenderer>().sprite.name.Split(new char[] { '_' });
        string golden = forGolden[forGolden.Length - 1] == "Golden" ? "_Golden" : "";
        Transform RH = transform.parent.parent.Find("R_Hand");
        GameObject proj = (GameObject)Instantiate((GameObject)Resources.Load("Soldier/S_Projectiles_" + pr + golden), new Vector3(RH.position.x, RH.position.y, RH.position.z), Quaternion.LookRotation(Vector3.forward, RH.up));
        proj.transform.parent = RH;
        proj.GetComponent<Projectile_Weapon>().SetArmy(army);
        proj.GetComponent<Projectile_Weapon>().SetDmgK(dmgK);
        proj.gameObject.name = pr;
        pro = proj;

        Target = target;
        lastPos = Target.position + Vector3.forward * 0.3f;
        startTime = Time.time;

        float type = att.AttackNow(out attackEndToUse);
        attackEndToUse = attackEndToUse * weight;
        return type;
    }

    private void Update()
    {
        Animator anim = transform.parent.parent.GetComponent<Animator>();
        AnimatorStateInfo curBaseState = anim.GetCurrentAnimatorStateInfo(1);

        if (curBaseState.IsTag("attack"))
        {
            attacking = true;
        }

        if (attacking && curBaseState.IsTag("after attack") && pro != null)
        {
            if (Target != null)
            {
                attacking = false;

                Vector3 newPos = Target.position + Vector3.forward * 0.3f;
                float timeZero = Time.time - startTime;
                Vector3 speedZero = (newPos - lastPos) / timeZero;

                Vector3 toTar0 = newPos - transform.parent.position;
                //float angle0 =  (float)(Math.PI - Math.Asin(toTar0.magnitude * SM.g / (projSpeed * projSpeed)))/2;
                float angle0 = (float)(Math.Asin(toTar0.magnitude * SM.g / (projSpeed * projSpeed))) / 2;
                float time = (float)(2 * projSpeed * Math.Sin(angle0) / SM.g);
                newPos += time * speedZero;

                Vector3 deltaVector = new Vector3((float)(SM._rnd.NextDouble() * 2 - 1), (float)(SM._rnd.NextDouble() * 2 - 1), 0f).normalized * (float)(SM._rnd.NextDouble()) * (newPos - transform.parent.parent.position).magnitude * deltaTar;
                newPos += deltaVector;

                Vector3 toTar = newPos - transform.parent.position;
                //float angle = (float)(Math.PI - Math.Asin(toTar.magnitude * SM.g / (projSpeed * projSpeed))) / 2;
                float angle = (float)(Math.Asin(toTar.magnitude * SM.g / (projSpeed * projSpeed))) / 2;
                Vector3 speed = projSpeed * ((toTar.normalized * (float)Math.Cos(angle)) - (Vector3.forward * (float)Math.Sin(angle)));

                pro.GetComponent<Collider2D>().enabled = true;
                pro.transform.parent = SM.trash.transform;
                pro.transform.rotation = Quaternion.LookRotation(Vector3.Cross(speed, Vector3.Cross(toTar, Vector3.forward)), speed);

                pro.GetComponent<Projectile_Weapon>().SetSpeed(speed);

                pro = null;
            }
            else
            {
                GameObject.Destroy(pro);
                pro = null;
            }
        }
    }
}