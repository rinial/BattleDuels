using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Projectile_Weapon : Weapon
{
    private Vector3 speed = Vector3.zero;
    float minHeight = SM.soldHeight;
    public float zEnd = float.MaxValue;

    public override float Attack(out float attackEndToUse, float endurance, Transform target = null)
    {
        attackEndToUse = 0f;
        return -1f;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        zEnd = -(transform.position.z + transform.GetComponent<Collider2D>().offset.y * (float)System.Math.Cos(Vector3.Angle(transform.up, Vector3.forward) * System.Math.PI / 180f));
        if (zEnd <= minHeight && col.gameObject.layer == SM.SoldierLayer && SM.GetArmy(col.transform) != army && dmgK > 0f)
        {
            float basicDmg = (float)(SM._rnd.NextDouble()) * (maxBasicDmg - minBasicDmg) + minBasicDmg;
            bool isCritical = SM._rnd.NextDouble() <= critPoss;
            basicDmg = isCritical ? basicDmg * critMult : basicDmg;
            float dmg = basicDmg * dmgDecK * dmgK * dKbyClass;
            if(SM.IsWarlord(col.transform.GetComponent<Soldier>()))
                dmg = 9f;

            foreach (WEffect eff in effects)
            {
                eff(col.transform, (col.transform.position - transform.position).normalized, dmg);
            }

            col.transform.GetComponent<Soldier>().TakeDamage(dmg);
            //Debug.Log(army + ": " + dmg + ((isCritical || dmgDecK < 1f) ? " (" : "") + (isCritical ? "  critical: x" + critMult : "") + (dmgDecK < 1f ? "  blocked: x" + dmgDecK : "") + ((isCritical || dmgDecK < 1f) ? "  )" : "") + ".");

            speed = Vector3.zero;
            //if(zEnd>=minHeight)
            //    transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/" + transform.GetComponent<SpriteRenderer>().sprite.name + "_In");
            transform.parent = col.transform;
            transform.position = transform.position + Vector3.forward * (col.transform.position.z + zEnd);
            transform.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            transform.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = 0;
            transform.GetComponent<Collider2D>().enabled = false;
            transform.GetComponent<Projectile_Weapon>().enabled = false;
        }
        else if (col.gameObject.layer == SM.ProtectionLayer && SM.GetArmy(col.transform) != army && zEnd <= col.transform.GetComponent<Defence>().height)
        {
            dmgDecK = col.transform.GetComponent<Defence>().dmgDecK;
            speed = Vector3.zero;
            transform.parent = col.transform;
            transform.position = transform.position + Vector3.forward * (col.transform.position.z + zEnd);
            transform.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            transform.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = 0;
            transform.GetComponent<Collider2D>().enabled = false;
            transform.GetComponent<Projectile_Weapon>().enabled = false;
        }
    }

    public void SetArmy(string ar)
    {
        //if (army == null)
        //    army = ar;
        army = "G";
    }

    public void SetDmgK(float dK)
    {
        if (dmgK == 0f)
        {
            dmgK = dK;
        }
    }

    public void SetSpeed(Vector3 sp)
    {
        if (speed == Vector3.zero)
        {
            speed = sp;
        }
    }

    private void Update()
    {
        zEnd = -(transform.position.z + transform.GetComponent<Collider2D>().offset.y * (float)System.Math.Cos(Vector3.Angle(transform.up, Vector3.forward) * System.Math.PI / 180f));
        //Debug.Log(transform.up + " " + Vector3.Angle(transform.up, Vector3.forward) + " " + transform.position.z + " " + zEnd);
        if (zEnd <= SM.soldHeight)
        {
            transform.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
            transform.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sortingLayerName = "Infantry";
            transform.GetComponent<SpriteRenderer>().sortingOrder = 10;
            transform.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = 3;
        }

        if (zEnd < 0)
        {
            speed = Vector3.zero; 
            transform.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
            transform.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = 1;
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/" + transform.GetComponent<SpriteRenderer>().sprite.name + "_In");
            transform.GetComponent<Collider2D>().enabled = false;
            transform.GetComponent<Projectile_Weapon>().enabled = false;
        }

        if (speed != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.Cross(speed, Vector3.forward), speed), speed);
            transform.position += speed * Time.deltaTime;
            speed += Vector3.forward * SM.g * Time.deltaTime;
            //transform.rotation = Quaternion.LookRotation(Vector3.Cross(speed, -transform.right), speed);
        }
    }
}
