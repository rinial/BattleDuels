using UnityEngine;
using System.Collections;

public abstract class Attack
{
    private float _endUse;
    public float endUse
    {
        get
        {
            return _endUse;
        }
        protected set
        {
            _endUse = value;
        }
    }
    private float _dmgK;
    public float dmgK
    {
        get
        {
            return _dmgK;
        }
        protected set
        {
            _dmgK = value;
        }
    }
    private float _attackTime;
    public float attackTime
    {
        get
        {
            return _attackTime;
        }
        protected set
        {
            _attackTime = value;
        }
    }
    private float _attackNum;
    public float attackNum
    {
        get
        {
            return _attackNum;
        }
        protected set
        {
            _attackNum = value;
        }
    }

    public float AttackNow(out float attackEndToUse)
    {
        attackEndToUse = endUse;
        return attackNum;
    }
}
