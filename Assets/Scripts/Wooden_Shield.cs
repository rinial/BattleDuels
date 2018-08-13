using UnityEngine;
using System.Collections;

public class Wooden_Shield : Defence
{
    public override float dmgDecK
    {
        get;
        protected set;
    }

    public override float height
    {
        get;
        protected set;
    }

    void Start()
    {
        dmgDecK = 0.3f;
        height = 1.4f;
    }
}
