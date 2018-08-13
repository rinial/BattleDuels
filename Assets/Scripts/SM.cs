using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SM : MonoBehaviour
{
    public static System.Random _rnd = new System.Random();
    public const float g = 10f;
    public const float soldHeight = 1f;
    public GameObject _trash, _UI;
    public static GameObject trash, UI;
    public static readonly string playerArmy = "B";
    public static Transform rWarlord, bWarlord;
    private static bool gameover;
    public static List<Transform> solds;

    private void Start()
    {
        SoldierLayer = LayerMask.NameToLayer("Soldier");
        ProtectionLayer = LayerMask.NameToLayer("Protection");
        trash = _trash;
        UI = _UI;
        gameover = false;
        solds = new List<Transform>();
    }

    private void Update()
    {
        //Debug.Log(1f / Time.deltaTime);
    }

    public static string GetArmy(Transform gO)
    {
        return GetUpperParent(gO).GetComponent<Soldier_Upper>().army;
    }

    public static Transform GetUpperParent(Transform obj)
    {
        if (obj.parent == null || obj.parent.name == "Trash" || obj.parent.name == "Soldiers")
            return obj;
        else
            return GetUpperParent(obj.parent);
    }

    public static float GetAngleFromTo(Vector3 fromDir, Vector3 toDir)
    {
        Vector3 newDir = Vector3.Cross(fromDir, Vector3.forward);
        float ang0 = Vector3.Angle(newDir, toDir);
        float ang1 = Vector3.Angle(fromDir, toDir);
        return ang1 * (ang0 > 90 ? 1 : -1);
    }

    public static float GetRndFloat(float a, float b)
    {
        if (a > b)
        {
            float c = a;
            a = b;
            b = c;
        }
        return (float)_rnd.NextDouble() * (b - a) + a;
    }

    public static int SoldierLayer;
    public static int ProtectionLayer;

    public const float globalDngK = 1.5f;


    public static void Push(Transform target, Vector3 dir, float damage)
    {
        double ver = 0.3f;
        if (_rnd.NextDouble() < ver)
        {
            float minPower = 150f;
            float maxPower = 250f;
            float power = (float)_rnd.NextDouble() * (maxPower - minPower) + minPower;
            Vector3 force = dir * power;
            target.parent.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }
    public static void StrongPush(Transform target, Vector3 dir, float damage)
    {
        double ver = 0.2f;
        if (_rnd.NextDouble() < ver)
        {
            float minPower = 300f;
            float maxPower = 500f;
            float power = (float)_rnd.NextDouble() * (maxPower - minPower) + minPower;
            Vector3 force = dir * power;
            target.parent.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }

    public static void MakeBleed(Transform target, Vector3 dir, float damage)
    {
        double ver = 0.25f;
        if (_rnd.NextDouble() < ver)
        {
            float bleedTime = 3f;
            target.GetComponent<Soldier>().CauseBleeding(damage, bleedTime);
        }
    }

    public static void OnVictory()
    {
        if (!gameover)
        {
            UI.transform.Find("WinText").gameObject.SetActive(true);
            UI.transform.Find("RestartText").gameObject.SetActive(true);
            Time.timeScale = 0.3f;
            gameover = true;
        }
        else
        {
            UI.transform.Find("WinText").gameObject.SetActive(false);
            UI.transform.Find("LoseText").gameObject.SetActive(false);
            UI.transform.Find("DrawText").gameObject.SetActive(true);
            Time.timeScale = 0.3f;
        }
    }

    public static void OnDefeat()
    {
        if (!gameover)
        {
            UI.transform.Find("LoseText").gameObject.SetActive(true);
            UI.transform.Find("RestartText").gameObject.SetActive(true);
            Time.timeScale = 0.3f;
            gameover = true;
        }
        else
        {
            UI.transform.Find("WinText").gameObject.SetActive(false);
            UI.transform.Find("LoseText").gameObject.SetActive(false);
            UI.transform.Find("DrawText").gameObject.SetActive(true);
            Time.timeScale = 0.3f;
        }
    }

    public static void FillBars(string ar, float hp, float ep)
    {
        UI.transform.Find("Panel_" + (ar == "B" ? "L" : "R") + "T").Find("Health").Find("Filler").GetComponent<UnityEngine.UI.Image>().fillAmount = hp;
        UI.transform.Find("Panel_" + (ar == "B" ? "L" : "R") + "T").Find("Endurance").Find("Filler").GetComponent<UnityEngine.UI.Image>().fillAmount = ep;
    }


    public static void AddSoldier(Transform sol)
    {
        solds.Add(sol);
    }

    public static void RemoveSoldier(Transform sol)
    {
        solds.Remove(sol);
    }

    public static bool IsWarlord(Soldier s)
    {
        return s.transform.parent.GetComponent<Warlord>() != null;
    }
    public static Warlord AsWarlord(Soldier s)
    {
        if (IsWarlord(s))
            return s.transform.parent.GetComponent<Warlord>();
        return null;
    }

    public static bool IsOfficer(Soldier s)
    {
        return s.transform.parent.GetComponent<Officer>() != null;
    }
    public static Officer AsOfficer(Soldier s)
    {
        if (IsOfficer(s))
            return s.transform.parent.GetComponent<Officer>();
        return null;
    }

    public static bool IsPrivate(Soldier s)
    {
        return s.transform.parent.GetComponent<Private>() != null;
    }
    public static Private AsPrivate(Soldier s)
    {
        if (IsPrivate(s))
            return s.transform.parent.GetComponent<Private>();
        return null;
    }

    public static Soldier GetOfficer(Soldier sold)
    {
        if (IsPrivate(sold))
        {
            Soldier off = AsPrivate(sold).officer;
            if (off == null)
                return null;
            else
                return GetOfficer(off);
        }
        else if (IsOfficer(sold))
        {
            return sold;
        }
        return null;
    }
}

public delegate void WEffect(Transform target, Vector3 dir, float damage);