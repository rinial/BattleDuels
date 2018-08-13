using UnityEngine;
using System.Collections;

public abstract class Defence : InHand
{
    public abstract float dmgDecK
    {
        get;
        protected set;
    }

    public abstract float height
    {
        get;
        protected set;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
