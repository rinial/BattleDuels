using UnityEngine;
using System.Collections;

public abstract class Order
{
    public string text;
}

public class OrdNothing : Order
{
    public OrdNothing()
    {
        text = "nothing";
    }
}

public class OrdTest : Order
{
    public OrdTest()
    {
        text = "test";
    }
}

public class OrdMove : Order
{
    public float X
    {
        get;
        private set;
    }

    public float Y
    {
        get;
        private set;
    }

    public OrdMove(float x, float y)
    {
        text = "move";
        X = x;
        Y = y;
    }

    public OrdMove(Vector3 v)
        : this(v.x, v.y)
    { }
}

public class OrdAttack : Order
{
    public float X
    {
        get;
        private set;
    }

    public float Y
    {
        get;
        private set;
    }

    public OrdAttack(float x, float y)
    {
        text = "attack";
        X = x;
        Y = y;
    }

    public OrdAttack(Vector3 v)
        : this(v.x, v.y)
    { }
}

public class OrdDefend : Order
{
    public float X
    {
        get;
        private set;
    }

    public float Y
    {
        get;
        private set;
    }

    public float R
    {
        get;
        private set;
    }

    public OrdDefend(float x, float y, float r = 15f)
    {
        text = "defend";
        X = x;
        Y = y;
        R = r;
    }
}

