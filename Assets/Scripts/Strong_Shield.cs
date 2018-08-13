using UnityEngine;
using System.Collections;

public class Strong_Shield : Defence
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
        dmgDecK = 0.45f;
        height = 1.4f;
    }
}
