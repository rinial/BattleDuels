using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Soldier : MonoBehaviour
{
    private Animator anim;

    public string army
    {
        get
        {
            return transform.parent.GetComponent<Soldier_Upper>().army;
        }
    }
    private Order order;
    private GameObject rWeapon, lWeapon;
    private AnimatorStateInfo curBaseState;
    private enum LR { Left, Right };
    private LR LorR;

    private GameObject weapon;

    private bool inBattle
    {
        get
        {
            return enemies.Count > 0 && InDefenceField();
        }
    }

    private bool attacking
    {
        get
        {
            curBaseState = anim.GetCurrentAnimatorStateInfo(1);
            return curBaseState.IsTag("attack") || curBaseState.IsTag("after attack");
        }
    }

    private bool wasRunning;
    private bool runModeOn
    {
        get
        {
            wasRunning = false;
            if (speed > walkSp)
                wasRunning = true;
            if (!wasRunning && endurance < maxEnd / 2)
                return false;
            return !inBattle && endurance > 0;
        }
    }

    private bool wasAttacking = false;
    private bool attackModeOn
    {
        get
        {
            if (!wasAttacking && endurance < maxEnd / 2)
                return false;
            return endurance > 0;
        }
    }

    public float moral;

    private float rotateSp;
    private float acsel;
    private float stopAcs;
    private float stepForce;
    private float runSp;
    private float walkSp;
    private float speed;

    private float seeRadius;
    public float mRange
    {
        get;
        private set;
    }

    public float health
    {
        get;
        private set;
    }
    private float maxHP;
    private float hpPerSecond;
    private float lastLostHP;
    private float timeBeforeHPrec;

    private float maxEnd;
    private float endurance;
    private float endPerSecond;
    private float lastUsedEnd;
    private float timeBeforeEndRec;
    private float runEndUse;


    private float bleedingTimeLeft;
    private float lastBleeding;
    private float bleedingIntervals;
    private float minBleedK;
    private float maxBleedK;
    private float bleedBaseDamage;

    private bool fixedAttack;

    //private Transform fixedEnemy;
    private Transform closestEnemy, fixedEnemy;
    private float closestRad, fixedRad;
    private List<Transform> enemies;

    private Vector3 runV;
    public bool running;

    private void Start()
    {
        bool isWarlord = SM.IsWarlord(this);
        bool isOfficer = SM.IsOfficer(this);

        anim = transform.GetComponent<Animator>();
        anim.SetFloat("forDefence", 0.5f);

        order = new OrdNothing();

        enemies = new List<Transform>();
        //fixedEnemy = null;
        closestEnemy = null;
        closestRad = float.MaxValue;

        moral = (isWarlord || isOfficer) ? float.MaxValue : SM.GetRndFloat(70f, 130f);

        rotateSp = isWarlord ? 3.5f : 2.5f; //radians
        acsel = 3f;
        stopAcs = 8f;
        stepForce = isWarlord ? 10f : isOfficer ? 7f : 5f;
        runSp = 8f;
        walkSp = 3f;
        speed = 0f;
        LorR = SM._rnd.Next(2) == 0 ? LR.Left : LR.Right;

        seeRadius = 30f;

        maxHP = isWarlord ? 900f : isOfficer ? SM.GetRndFloat(250f, 350f) : SM.GetRndFloat(70f, 130f);
        health = maxHP;
        hpPerSecond = 7f;
        lastLostHP = Time.time;
        timeBeforeHPrec = 7f;

        minBleedK = isWarlord ? 0.1f : 0.2f;
        maxBleedK = isWarlord ? 0.2f : 0.4f;

        maxEnd = isWarlord ? 500f : isOfficer ? SM.GetRndFloat(200f, 250f) : SM.GetRndFloat(80f, 120f);
        endurance = maxEnd;
        endPerSecond = isWarlord ? 85 : isOfficer ? 40f : 20f;
        lastUsedEnd = Time.time;
        timeBeforeEndRec = 1f;
        runEndUse = isWarlord ? 60f : isOfficer ? 30f : 15f;

        bleedingTimeLeft = 0f;
        lastBleeding = -100f;
        bleedingIntervals = 0.3f;
        bleedBaseDamage = 0f;

        fixedAttack = false;

        if (SM.IsPrivate(this))
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_" + army);
        }
        else if (isOfficer)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_Officer_" + army);
        }
        else if (isWarlord)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_Warlord_" + army);
        }
        transform.Find("L_Hand").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Hand_" + army);
        transform.Find("R_Hand").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Hand_" + army);

        string eqSet = transform.parent.GetComponent<Soldier_Upper>().eqSet;
        string golden = isOfficer || isWarlord ? "_Golden" : "";
        if (eqSet == "Axe")
        {
            string wep = eqSet;
            Transform RH = transform.Find("R_Hand");
            GameObject rWeapon = (GameObject)Instantiate((GameObject)Resources.Load("Soldier/S_Equip_" + wep + golden), RH.position, RH.rotation);
            rWeapon.transform.parent = RH;
            rWeapon.gameObject.name = wep;

            Transform LH = transform.Find("L_Hand");
            GameObject lWeapon = (GameObject)Instantiate((GameObject)Resources.Load("Soldier/S_Equip_StrongShield" + golden), LH.position, LH.rotation);
            lWeapon.transform.parent = LH;
            lWeapon.gameObject.name = "StrongShield";

            weapon = rWeapon;
        }
        else if (eqSet == "Sword" || eqSet == "Spear")
        {
            string wep = eqSet;
            Transform RH = transform.Find("R_Hand");
            GameObject rWeapon = (GameObject)Instantiate((GameObject)Resources.Load("Soldier/S_Equip_" + wep + golden), RH.position, RH.rotation);
            rWeapon.transform.parent = RH;
            rWeapon.gameObject.name = wep;

            Transform LH = transform.Find("L_Hand");
            GameObject lWeapon = (GameObject)Instantiate((GameObject)Resources.Load("Soldier/S_Equip_Shield" + golden), LH.position, LH.rotation);
            lWeapon.transform.parent = LH;
            lWeapon.gameObject.name = "Shield";

            weapon = rWeapon;
        }
        else if (eqSet == "Bow")
        {
            string wep = eqSet;
            Transform LH = transform.Find("L_Hand");
            GameObject lWeapon = (GameObject)Instantiate((GameObject)Resources.Load("Soldier/S_Equip_" + wep + golden), LH.position, LH.rotation);
            lWeapon.transform.parent = LH;
            lWeapon.gameObject.name = wep;

            weapon = lWeapon;
        }
        mRange = weapon.GetComponent<Weapon>().maxRange;

        transform.parent.Find("Bars").Find("Health").Find("Filler").GetComponent<UnityEngine.UI.Image>().fillAmount = (float)(health / maxHP);
        transform.parent.Find("Bars").Find("Endurance").Find("Filler").GetComponent<UnityEngine.UI.Image>().fillAmount = (float)(endurance / maxEnd);
        if (isWarlord)
        {
            SM.FillBars(army, (float)(health / maxHP), (float)(endurance / maxEnd));
        }

        SM.AddSoldier(transform);
    }

    private void FixedUpdate()
    {
        GetEnemiesAround();
    }

    private void Update()
    {
        //if (!SM.IsWarlord(this))
        //    Debug.Log(order.text);
        //Debug.Log(attacking);

        curBaseState = anim.GetCurrentAnimatorStateInfo(0);

        ResetTriggers();
        Bleed();

        //OrderForLost();

        LoseGainMoral();
        CheckMoral();

        if (attacking)
        {
            fixedAttack = false;
        }

        //GetEnemiesAround();

        if (order is OrdNothing)
            order = new OrdDefend(transform.position.x, transform.position.y);
        ExecuteOrder();

        SetForAnimations();

        RegainEndAndHealth();

        FillBars();

        if (health <= 0)
            transform.parent.GetComponent<Soldier_Upper>().DestroyIt();
    }

    private void GetEnemiesAround()
    {
        enemies.Clear();
        closestEnemy = null;
        closestRad = float.MaxValue;
        fixedEnemy = null;
        fixedRad = float.MaxValue;
        //bool foundFixed = false;
        bool lookForWarlord = weapon.GetComponent<LongRanged_Weapon>() == null;
        //bool lookForWarlord = true;
        foreach (Transform soldier in SM.solds)
        {
            if ((soldier.position - transform.position).magnitude <= seeRadius)
            {
                if (SM.GetArmy(soldier) != army)
                {
                    //if (fixedEnemy == soldier)
                    //{
                    //    foundFixed = true;
                    //}
                    float newRad = Vector3.Magnitude(soldier.position - transform.position);
                    enemies.Add(soldier);

                    if (SM.IsWarlord(soldier.GetComponent<Soldier>()) && lookForWarlord)
                    {
                        fixedRad = newRad;
                        fixedEnemy = soldier;
                    }
                    if (newRad < closestRad)
                    {
                        closestRad = newRad;
                        closestEnemy = soldier;
                    }
                }
            }
        }
        //if (enemies.Count == 0)
        //    fixedEnemy = null;
        //else if (!foundFixed)
        //    fixedEnemy = closestEnemy;
    }

    private void ExecuteOrder()
    {
        switch (order.text)
        {
            case "nothing":
                StopMoving();
                break;

            case "move":
                Move((order as OrdMove).X, (order as OrdMove).Y);
                break;

            case "attack":
                AttackMove((order as OrdAttack).X, (order as OrdAttack).Y);
                break;

            case "defend":
                Defend((order as OrdDefend).X, (order as OrdDefend).Y, (order as OrdDefend).R);
                break;

            case "test":
                Test();
                break;

            default:
                break;
        }
    }

    public void MoveNow(float x, float y)
    {
        order = new OrdMove(x, y);
    }
    public void MoveNow(Vector3 v)
    {
        MoveNow(v.x, v.y);
    }

    private void Move(float x, float y)
    {
        Vector3 glTarget = new Vector3(x, y, 0f);

        if (IsNearDestination(glTarget))
        {
            order = new OrdNothing();
            StopMoving();
        }
        else
        {
            Vector3 target = CreateNewWay(glTarget);
            Vector3 tarDir = target - transform.position;
            TurnTo(tarDir);
            if (TurnedEnough(tarDir))
            {
                float speed2 = speed + acsel * Time.deltaTime;
                if (speed2 > runSp)
                    speed2 = runSp;
                if (!runModeOn)
                {
                    if (speed > walkSp)
                        StopMoving();
                    else
                    {
                        speed = speed2 > walkSp ? walkSp : speed2;
                        transform.parent.Translate(Vector3.up * speed * Time.deltaTime);
                    }
                }
                else
                {
                    speed = speed2;
                    transform.parent.Translate(Vector3.up * speed * Time.deltaTime);
                    if (speed > walkSp)
                    {
                        UseEndurance(runEndUse * Time.deltaTime);
                    }
                }
            }
            else
                StopMoving();
        }
        //Debug.Log("End" + endurance);
        //Debug.Log("Sp" + speed);
    }

    public void AttackNow(float x, float y)
    {
        order = new OrdAttack(x, y);
    }
    public void AttackNow(Vector3 v)
    {
        AttackNow(v.x, v.y);
    }

    private void AttackMove(float x, float y)
    {
        float r = 15f;
        if (IsNearDestination(new Vector3(x, y, 0), r))
        {
            DefendNow(x, y, r);
            Defend(x, y, r);
        }
        else
            Attack(x, y);
    }

    private void Attack(float x, float y)
    {
        if (enemies.Count == 0 || weapon.GetComponent<Weapon>().InAttackRange(closestRad) == 1)
            Move(x, y);
        else
        {
            StopMoving();

            float rad;
            Vector3 tarDir;
            Transform s;
            if (fixedEnemy && weapon.GetComponent<Weapon>().InAttackRange(fixedRad) != 1)
            {
                tarDir = fixedEnemy.position - transform.position;
                rad = fixedRad;
                s = fixedEnemy;
            }
            else
            {
                tarDir = closestEnemy.position - transform.position;
                rad = closestRad;
                s = closestEnemy;
            }

            if (weapon.GetComponent<Weapon>().InAttackRange(closestRad) == -1)
            {
                Step(transform.position - closestEnemy.position);
            }

            TurnTo(tarDir);
            if (TurnedEnough(tarDir, 10f) && weapon.GetComponent<Weapon>().InAttackRange(rad) <= 0)
            {
                AttackAhead(s);
            }

            //if (weapon.GetComponent<Weapon>().InAttackRange(rad) == 0)
            //{
            //    StopMoving();
            //}
        }
    }

    public void DefendNow()
    {
        order = new OrdDefend(transform.position.x, transform.position.y);
    }
    public void DefendNow(float r)
    {
        order = new OrdDefend(transform.position.x, transform.position.y, r);
    }
    public void DefendNow(float x, float y)
    {
        order = new OrdDefend(x, y);
    }
    public void DefendNow(Vector3 v)
    {
        DefendNow(v.x, v.y);
    }
    public void DefendNow(float x, float y, float r)
    {
        order = new OrdDefend(x, y, r);
    }
    public void DefendNow(Vector3 v, float r)
    {
        DefendNow(v.x, v.y, r);
    }

    private void Defend(float x, float y, float rad)
    {
        Vector3 center = new Vector3(x, y, 0);
        float closestToCenter = float.MaxValue;
        //Vector3 center = transform.position;
        float defR = (rad + mRange) > seeRadius ? seeRadius : (rad + mRange);
        Transform remEnemy = null;
        bool lookForWarlord = weapon.GetComponent<LongRanged_Weapon>() == null;
        foreach (Transform soldier in SM.solds)
        {
            if ((soldier.position - center).magnitude <= defR)
            {
                if (SM.GetArmy(soldier) != army)
                {
                    float r = (soldier.position - center).magnitude;

                    if (SM.IsWarlord(soldier.GetComponent<Soldier>()) && lookForWarlord)
                    {
                        fixedRad = r;
                        fixedEnemy = soldier;
                    }

                    if (r < closestToCenter)
                    {
                        remEnemy = soldier;
                        closestToCenter = r;
                    }
                }
            }
        }
        if (remEnemy == null)
        {
            if (!IsNearDestination(new Vector3(center.x, center.y)))
                Attack(center.x, center.y);
            else
            {
                StopMoving();
                if (speed == 0f)
                {
                    Transform off = (SM.IsWarlord(this) || SM.IsOfficer(this) || SM.AsPrivate(this).officer == null) ? transform : SM.AsPrivate(this).officer.transform;
                    Vector3 toTar = army == "B" ? (SM.rWarlord.position - off.position) : (SM.bWarlord.position - off.position);
                    Vector3 v = Math.Abs(toTar.x) >= Math.Abs(toTar.y) ? (toTar.x >= 0 ? Vector3.right : -Vector3.right) : (toTar.y >= 0 ? Vector3.up : -Vector3.up);
                    TurnTo(v);
                }
            }
        }
        else
        {
            if (fixedEnemy != null)
                Attack(fixedEnemy.position.x, fixedEnemy.position.y);
            else
                Attack(remEnemy.position.x, remEnemy.position.y);
        }
    }

    public void TestNow()
    {
        order = new OrdTest();
    }

    private void Test()
    {
        AttackAhead(closestEnemy);
        order = new OrdNothing();
    }

    //private const float l = 5f;
    //private Vector3 placeNearWarlord;
    //private void OrderForLost()
    //{
    //    if (placeNearWarlord == null)
    //    {
    //        float k = army == "B" ? -1f : 1f;
    //        placeNearWarlord = new Vector3((1 + l * (float)(SM._rnd.NextDouble()) * k), l * (float)(SM._rnd.NextDouble() * 2 - 1)).normalized;
    //    }
    //    if (SM.IsPrivate(this) && SM.AsPrivate(this).officer == null)
    //    {
    //        Vector3 WLpos = army == "B" ? SM.bWarlord.position : SM.rWarlord.position;
    //        order = new OrdAttack(WLpos + placeNearWarlord);
    //    }
    //}

    private void StopMoving()
    {
        speed -= stopAcs * Time.deltaTime;
        if (speed < 0f)
            speed = 0f;
        transform.parent.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void Step(Vector3 tarDir)
    {
        //Debug.Log("step");
        transform.parent.GetComponent<Rigidbody2D>().AddForce(tarDir.normalized * stepForce);
    }

    private Vector3 CreateNewWay(Vector3 glTarget)
    {
        //TODO
        return glTarget;
    }

    private void TurnTo(Vector3 tarDir)
    {
        Vector3 direct = transform.parent.up;
        Vector3 newDir;
        if (Vector3.Angle(direct, tarDir) < 150f)
            newDir = Vector3.RotateTowards(direct, tarDir, rotateSp * Time.deltaTime, 1f);
        else
        {
            newDir = Vector3.RotateTowards(direct, transform.parent.right * (LorR == LR.Right ? 1 : -1), rotateSp * Time.deltaTime, 1f);
        }
        transform.parent.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f), newDir);
    }

    private bool TurnedEnough(Vector3 tarDir, float nearAngle = 70f)
    {
        Vector3 direct = transform.parent.up;
        return Vector3.Angle(direct, tarDir) <= nearAngle;
    }

    private bool IsNearDestination(Vector3 glTarget, float radius = float.MaxValue)
    {
        radius = radius == float.MaxValue ? GetNearSpace() : Math.Abs(radius);
        float s = (speed * speed) / (2f * stopAcs);
        return (glTarget - transform.position).magnitude - s <= radius;
    }

    private float GetNearSpace()
    {
        float rad = 1.5f;
        return rad;
    }

    private void RegainEndAndHealth()
    {
        if (Time.time - lastUsedEnd > timeBeforeEndRec)
            endurance += endPerSecond * Time.deltaTime;
        endurance = endurance > maxEnd ? maxEnd : endurance;

        if (Time.time - lastLostHP > timeBeforeHPrec)
            health += hpPerSecond * Time.deltaTime;
        health = health > maxHP ? maxHP : health;
    }

    private void FillBars()
    {
        transform.parent.Find("Bars").Find("Health").Find("Filler").GetComponent<UnityEngine.UI.Image>().fillAmount = (float)(health / maxHP);
        transform.parent.Find("Bars").Find("Endurance").Find("Filler").GetComponent<UnityEngine.UI.Image>().fillAmount = (float)(endurance / maxEnd);
        if (SM.IsWarlord(this))
        {
            SM.FillBars(army, (float)(health / maxHP), (float)(endurance / maxEnd));
        }
    }

    private void LoseGainMoral()
    {
        if (SM.IsPrivate(this))
        {
            if (Time.time - lastLostHP < timeBeforeHPrec)
            {
                Private pr = SM.AsPrivate(this);
                float k1 = 18f * (maxHP - health) / maxHP;
                float k2 = pr.officer == null ? 2f : (1f / (SM.AsOfficer(pr.officer).subs.Length + 3));
                float k3 = 20f;
                moral -= k1 * k2 * k3 * Time.deltaTime;
                if (moral < 0f)
                    moral = 0f;
            }
            //else
            //{
            //    float k = 5;
            //    moral += k * Time.deltaTime;
            //}
        }
    }
    private void CheckMoral()
    {
        if (SM.IsPrivate(this))
        {
            if (moral <= 0)
            {
                float l = 60f;
                Vector3 runV = -(army == "B" ? (SM.rWarlord.position - transform.position) : (SM.bWarlord.position - transform.position)).normalized * l;
                order = new OrdMove(transform.position + runV);
                if (SM.AsPrivate(this).officer != null)
                {
                    Officer off = SM.AsOfficer(SM.AsPrivate(this).officer);
                    off.subs[SM.AsPrivate(this).index] = null;
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_" + army + "_NO");
                    Private priv = SM.AsPrivate(this);
                    priv.deltaV = Vector3.zero;
                    priv.index = -1;
                    priv.SetOfficer(null);
                }
            }
        }
    }

    private void SetForAnimations()
    {
        anim.SetFloat("Speed", speed);
        anim.SetFloat("forWalkRun", speed / runSp);
        anim.SetBool("Defending", inBattle);
        SetForDefence();
    }

    private float maxAng = 100f;
    private float minAng = -60f;
    private float defSp = 1f;

    private float lastDef = -10f;
    private void SetForDefence()
    {
        if (InDefenceField())
        {
            float ang0 = transform.Find("L_Hand").eulerAngles.z - transform.eulerAngles.z + 90f;
            float ang = GetAngleToEnemy(closestEnemy);
            float ang1 = ang0 + defSp * Time.deltaTime * ((ang - ang0) / Math.Abs(ang - ang0));
            float forDef = (ang1 - minAng) / (maxAng - minAng);
            if (Time.time - lastDef > 0.2 && army == "B")
            {
                //Debug.Log("");
                //Debug.Log("ang0:\t" + ang0);
                //Debug.Log("ang:\t" + ang);
                //Debug.Log("ang1:\t" + ang1);
                //Debug.Log("def:\t" + forDef);
                lastDef = Time.time;
            }
            anim.SetFloat("forDefence", forDef);
        }
    }

    private bool InDefenceField()
    {
        if (closestEnemy == null)
            return false;
        float ang = GetAngleToEnemy(closestEnemy);
        float l = (closestEnemy.position - transform.position).magnitude;
        return (ang < maxAng && ang > minAng) && l < (closestEnemy.GetComponent<Soldier>().mRange + 7f);
    }

    //private float GetAngleToFixedEnemy()
    //{
    //    if (fixedEnemy == null)
    //        return 0f;
    //    Vector3 direct1 = transform.parent.up;
    //    Vector3 direct2 = fixedEnemy.position - transform.position;
    //    return SM.GetAngleFromTo(direct1, direct2);
    //}

    private float GetAngleToEnemy(Transform enemy)
    {
        if (enemy == null)
            return 0f;
        Vector3 direct1 = transform.parent.up;
        Vector3 direct2 = enemy.position - transform.position;
        return SM.GetAngleFromTo(direct1, direct2);
    }

    public void AttackAhead(Transform s)
    {
        //Debug.Log("!! " + attacking);
        if (!attacking && attackModeOn && !fixedAttack)
        {
            float attackEndToUse;
            float type = weapon.GetComponent<Weapon>().Attack(out attackEndToUse, endurance, s);
            if (type != -1f)
            {
                UseEndurance(attackEndToUse);

                wasAttacking = true;
                fixedAttack = true;

                anim.SetFloat("AttackType", type);
                anim.SetTrigger("Attack");
            }
            else
                wasAttacking = false;
        }
    }

    private void ResetTriggers()
    {
        anim.ResetTrigger("Attack");
    }

    public void StopBattleAnimations()
    {
        anim.SetTrigger("StopBattleAnim");
    }

    private void UseEndurance(float endUsed)
    {
        endurance -= endUsed;
        endurance = endurance < 0f ? 0f : endurance;
        lastUsedEnd = Time.time;
    }
    public void TakeDamage(float dmg)
    {
        LoseHealth(dmg);
    }

    public void CauseBleeding(float baseDmg, float time)
    {
        bleedBaseDamage = baseDmg;
        bleedingTimeLeft += time;
        if (Time.time - lastBleeding > 2 * bleedingIntervals)
            lastBleeding = 0f;
    }

    private void Bleed()
    {
        bleedingTimeLeft -= Time.deltaTime;
        bleedingTimeLeft = bleedingTimeLeft <= 0f ? 0f : bleedingTimeLeft;

        if (bleedingTimeLeft > 0f && Time.time - lastBleeding > bleedingIntervals)
        {
            lastBleeding = Time.time;
            float bleedK = minBleedK + (float)SM._rnd.NextDouble() * (maxBleedK - minBleedK);
            LoseHealth(bleedK * bleedBaseDamage);
        }
    }

    private void LoseHealth(float hpLost)
    {
        health -= hpLost;
        lastLostHP = Time.time;
    }

}