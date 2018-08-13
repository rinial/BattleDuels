using UnityEngine;
using System.Collections;

public class Private : Soldier_Upper
{
    public Soldier officer
    {
        get;
        private set;
    }
    public Vector3 deltaV;
    public int index;

    void Start()
    {
        StartNow();
    }

    void Update()
    {
        UpdateNow();
    }

    public Vector3 PrivPos(Vector3 pos)
    {
        //Vector3 up = (pos - officer.transform.position).normalized;
        //Vector3 newV = pos + (new Vector3(up.y, -up.x) * deltaV.x + up * deltaV.y);
        Vector3 newV = pos + Vector3.right * deltaV.x + Vector3.up * deltaV.y;
        return newV;
    }
    public Vector3 PrivPos()
    {
        //return officer.transform.position + officer.transform.right * deltaV.x + officer.transform.up * deltaV.y;
        return officer.transform.position + Vector3.right * deltaV.x + Vector3.up * deltaV.y;
    }

    public void SetOfficer(Officer off)
    {
        officer = off == null ? null : off.transform.Find("Soldier").GetComponent<Soldier>();
    }
}
