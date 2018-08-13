using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CM : MonoBehaviour
{
    private static List<Soldier> targets;

    private static float lMouseTimer, lMouseTime, clickTime, nearClick, remTimeSc;
    private static Vector3 mousePos, firstMousePos;
    public static readonly string yourArmy = SM.playerArmy;
    private static bool keepFormation, toAttack;
    public GameObject uiCamera;
    private TrailRenderer tR;
    private int cMask;
    public GameObject _UI;
    public static GameObject UI;
    private bool attackWarlord;

    public static int tarNum
    {
        get
        {
            return targets.Count;
        }
    }
    private static bool lDoingLong
    {
        get
        {
            return lMouseTime > clickTime || (firstMousePos - mousePos).magnitude > nearClick;
        }
    }

    private void Start()
    {
        lMouseTimer = 0f;
        lMouseTime = 0f;
        clickTime = 1f;
        nearClick = 0.3f;
        remTimeSc = 1f;
        targets = new List<Soldier>();
        cMask = uiCamera.GetComponent<Camera>().cullingMask;
        tR = transform.GetComponent<TrailRenderer>();
        UI = _UI;
        attackWarlord = false;
        keepFormation = true;
        toAttack = true;
        Time.timeScale = 1f;
        Pause();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, -5f);
        tR.startWidth = 0.3f * uiCamera.GetComponent<Camera>().orthographicSize / 20f;
        tR.endWidth = 0.05f * uiCamera.GetComponent<Camera>().orthographicSize / 20f;
        tR.enabled = Input.GetMouseButton(1);
        //float k = uiCamera.GetComponent<Camera>().orthographicSize/6f;
        //transform.localScale = new Vector3(k, k, 1);
        lMouseTime = Time.time - lMouseTimer;

        if (Input.GetMouseButtonDown(0))
        {
            lMouseTimer = Time.time;
            firstMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lMouseTimer = Time.time;
            if (!lDoingLong)
            {
                lDoWhenClick();
            }
            else
            {
                lDoWhenLong();
            }
        }

        if (lDoingLong && Input.GetMouseButton(0))
        {
            DrawRect(firstMousePos, mousePos);
        }
        else
        {
            DrawRect(Vector3.zero, Vector3.zero);
        }

        if (Input.GetMouseButton(1))
        {
            rDoWhenClick();
        }

        //test
        if (Input.GetKeyDown(KeyCode.T))
        {
            Test();
        }

        //defend
        if (Input.GetKeyDown(KeyCode.D))
        {
            Defend();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            WarlordAttack();
        }
        if (attackWarlord)
        {
            Transform warlordTr = yourArmy == "B" ? SM.bWarlord : SM.rWarlord;
            Soldier warlord = warlordTr.GetComponent<Soldier>();
            warlord.AttackNow(yourArmy == "B" ? SM.rWarlord.position : SM.bWarlord.position);
        }

        //bluearmy
        if (Input.GetKeyDown(KeyCode.F1))
        {
            string army = "B";
            GameObject off = (GameObject)Instantiate((GameObject)Resources.Load("Officer"), new Vector3(mousePos.x, mousePos.y, -1), Quaternion.LookRotation(Vector3.forward, Vector3.right));
            off.GetComponent<Officer>().army = army;
            off.GetComponent<Officer>().eqSet = "Sword";
            off.transform.name = army + "_Officer";
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            string army = "B";
            GameObject off = (GameObject)Instantiate((GameObject)Resources.Load("Officer"), new Vector3(mousePos.x, mousePos.y, -1), Quaternion.LookRotation(Vector3.forward, Vector3.right));
            off.GetComponent<Officer>().army = army;
            off.GetComponent<Officer>().eqSet = "Spear";
            off.transform.name = army + "_Officer";
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            string army = "B";
            GameObject off = (GameObject)Instantiate((GameObject)Resources.Load("Officer"), new Vector3(mousePos.x, mousePos.y, -1), Quaternion.LookRotation(Vector3.forward, Vector3.right));
            off.GetComponent<Officer>().army = army;
            off.GetComponent<Officer>().eqSet = "Bow";
            off.transform.name = army + "_Officer";
        }

        //redarmy
        if (Input.GetKeyDown(KeyCode.F5))
        {
            string army = "R";
            GameObject off = (GameObject)Instantiate((GameObject)Resources.Load("Officer"), new Vector3(mousePos.x, mousePos.y, -1), Quaternion.LookRotation(Vector3.forward, -Vector3.right));
            off.GetComponent<Officer>().army = army;
            off.GetComponent<Officer>().eqSet = "Sword";
            off.transform.name = army + "_Officer";
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            string army = "R";
            GameObject off = (GameObject)Instantiate((GameObject)Resources.Load("Officer"), new Vector3(mousePos.x, mousePos.y, -1), Quaternion.LookRotation(Vector3.forward, -Vector3.right));
            off.GetComponent<Officer>().army = army;
            off.GetComponent<Officer>().eqSet = "Spear";
            off.transform.name = army + "_Officer";
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            string army = "R";
            GameObject off = (GameObject)Instantiate((GameObject)Resources.Load("Officer"), new Vector3(mousePos.x, mousePos.y, -1), Quaternion.LookRotation(Vector3.forward, -Vector3.right));
            off.GetComponent<Officer>().army = army;
            off.GetComponent<Officer>().eqSet = "Bow";
            off.transform.name = army + "_Officer";
        }

        //hud
        if (Input.GetKeyDown(KeyCode.H))
        {
            UIoff();
        }

        //restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        //speedUp
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            Faster();
        }

        //speedDown
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            Slower();
        }

        //pause
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }

        //quit
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu();
        }
    }

    public void Help()
    {
        //showhelp
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        Transform panel = UI.transform.Find("Panel_LB");
        if (panel.Find("Menu").Find("Text").GetComponent<UnityEngine.UI.Text>().text == @"/\ /\ /\")
        {
            panel.Find("Exit").gameObject.SetActive(true);
            panel.Find("Restart").gameObject.SetActive(true);
            panel.Find("Help").gameObject.SetActive(true);
            panel.Find("Menu").Find("Text").GetComponent<UnityEngine.UI.Text>().text = @"\/ \/ \/";
        }
        else
        {
            panel.Find("Exit").gameObject.SetActive(false);
            panel.Find("Restart").gameObject.SetActive(false);
            panel.Find("Help").gameObject.SetActive(false);
            panel.Find("Menu").Find("Text").GetComponent<UnityEngine.UI.Text>().text = @"/\ /\ /\";
        }
    }

    public void OpenHelp()
    {
        if (Time.timeScale != 0)
            Pause();
        UI.transform.Find("PauseText").GetComponent<UnityEngine.UI.Text>().enabled = false;
        UI.transform.Find("WinText").GetComponent<UnityEngine.UI.Text>().enabled = false;
        UI.transform.Find("LoseText").GetComponent<UnityEngine.UI.Text>().enabled = false;
        UI.transform.Find("DrawText").GetComponent<UnityEngine.UI.Text>().enabled = false;
        UI.transform.Find("RestartText").GetComponent<UnityEngine.UI.Text>().enabled = false;

        UI.transform.Find("HelpPanel").gameObject.SetActive(true);
    }
    public void CloseHelp()
    {
        UI.transform.Find("HelpPanel").gameObject.SetActive(false);

        UI.transform.Find("PauseText").GetComponent<UnityEngine.UI.Text>().enabled = true;
        UI.transform.Find("WinText").GetComponent<UnityEngine.UI.Text>().enabled = true;
        UI.transform.Find("LoseText").GetComponent<UnityEngine.UI.Text>().enabled = true;
        UI.transform.Find("DrawText").GetComponent<UnityEngine.UI.Text>().enabled = true;
        UI.transform.Find("RestartText").GetComponent<UnityEngine.UI.Text>().enabled = true;
    }

    public void Pause()
    {
        Transform panel = UI.transform.Find("Panel_RB");
        if (Time.timeScale == 0)
        {
            UI.transform.Find("PauseText").gameObject.SetActive(false);
            panel.Find("PausePlay").Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Pause";
            Time.timeScale = remTimeSc;
        }
        else
        {
            UI.transform.Find("PauseText").gameObject.SetActive(true);
            panel.Find("PausePlay").Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Play";
            remTimeSc = Time.timeScale;
            Time.timeScale = 0;
        }
    }

    public void Faster()
    {
        if (Time.timeScale == 0)
        {
            Transform panel = UI.transform.Find("Panel_RB");
            UI.transform.Find("PauseText").gameObject.SetActive(false);
            panel.Find("PausePlay").Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Pause";
            Time.timeScale = remTimeSc;
        }
        float max = 5f;
        if (Time.timeScale * 2 > max)
            Time.timeScale = max;
        else
            Time.timeScale *= 2;
    }

    public void Slower()
    {
        if (Time.timeScale == 0)
        {
            Transform panel = UI.transform.Find("Panel_RB");
            UI.transform.Find("PauseText").gameObject.SetActive(false);
            panel.Find("PausePlay").Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Pause";
            Time.timeScale = remTimeSc;
        }
        Time.timeScale /= 2;
    }

    public void UIoff()
    {
        uiCamera.GetComponent<Camera>().cullingMask = uiCamera.GetComponent<Camera>().cullingMask != 0 ? 0 : cMask;
    }
    public void WarlordAttack()
    {
        Transform panel = UI.transform.Find("Panel_B");
        if (!attackWarlord)
        {
            attackWarlord = true;
            panel.Find("WLattackButton").Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Warlord Defence";
        }
        else
        {
            attackWarlord = false;
            Transform warlordTr = yourArmy == "B" ? SM.bWarlord : SM.rWarlord;
            Soldier warlord = warlordTr.GetComponent<Soldier>();

            panel.Find("WLattackButton").Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Warlord Attack";
            warlord.DefendNow();
        }
    }

    public void AttackOrMove()
    {
        Transform panel = UI.transform.Find("Panel_B");
        if (!toAttack)
        {
            toAttack = true;
            panel.Find("AttackMove").Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Move Mode";
        }
        else
        {
            toAttack = false;
            panel.Find("AttackMove").Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Attack Mode";
        }
    }

    public void KeepFormChange()
    {
        keepFormation = !keepFormation;
    }

    private static void lDoWhenClick()
    {
        Transform s = GetObjectUnderMouse(SM.SoldierLayer);
        Soldier soldier = s ? s.GetComponent<Soldier>() : null;
        Soldier officer = soldier ? SM.GetOfficer(soldier) : null;

        if (officer && (officer.army == yourArmy))
        {
            if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                ClearTargets();
            }

            Soldier[] subs = SM.AsOfficer(officer).subs;

            if (!targets.Contains(officer))
            {
                officer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_Officer_HL_" + yourArmy);
                plusTarget(officer);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_HL_" + yourArmy);
            }
            else
            {
                officer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_Officer_" + yourArmy);
                minusTarget(officer);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_" + yourArmy);
            }
        }
        //attack
        else if (Input.GetKey(KeyCode.A))
        {
            Attack();
        }
        //move
        else if (Input.GetKey(KeyCode.G))
        {
            Move();
        }
        else
        {
            ClearTargets();
        }
    }

    private static void Attack()
    {
        if (!keepFormation)
        {
            foreach (Soldier off in targets)
            {
                Soldier[] subs = SM.AsOfficer(off).subs;
                off.AttackNow(mousePos);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.AttackNow(SM.AsPrivate(sold).PrivPos(mousePos));
            }
        }
        else
        {
            Vector3 cent = Vector3.zero;
            foreach (Soldier off in targets)
            {
                cent += off.transform.position;
            }
            cent /= targets.Count;
            foreach (Soldier off in targets)
            {
                Vector3 coord = mousePos + off.transform.position - cent;
                Soldier[] subs = SM.AsOfficer(off).subs;
                off.AttackNow(coord);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.AttackNow(SM.AsPrivate(sold).PrivPos(coord));
            }
        }
    }

    private static void Move()
    {
        if (!keepFormation)
        {
            foreach (Soldier off in targets)
            {
                Soldier[] subs = SM.AsOfficer(off).subs;
                off.MoveNow(mousePos);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.MoveNow(SM.AsPrivate(sold).PrivPos(mousePos));
            }
        }
        else
        {
            Vector3 cent = Vector3.zero;
            foreach (Soldier off in targets)
            {
                cent += off.transform.position;
            }
            cent /= targets.Count;
            foreach (Soldier off in targets)
            {
                Vector3 coord = mousePos + off.transform.position - cent;
                Soldier[] subs = SM.AsOfficer(off).subs;
                off.MoveNow(coord);
                foreach (Soldier sold in subs)
                    if (sold != null)
                        sold.MoveNow(SM.AsPrivate(sold).PrivPos(coord));
            }
        }
    }

    public void Defend()
    {
        Debug.Log("!!");
        foreach (Soldier off in targets)
        {
            Soldier[] subs = SM.AsOfficer(off).subs;
            off.DefendNow(3f);
            foreach (Soldier sold in subs)
                if (sold != null)
                    sold.DefendNow(SM.AsPrivate(sold).PrivPos(), 3f);
        }
    }

    private static void Test()
    {
        foreach (Soldier off in targets)
        {
            Soldier[] subs = SM.AsOfficer(off).subs;
            off.TestNow();
            foreach (Soldier sold in subs)
                if (sold != null)
                    sold.TestNow();
        }
    }

    private static void rDoWhenClick()
    {
        if (toAttack)
        {
            Attack();
        }
        else
        {
            Move();
        }
    }

    private static void lDoWhenLong()
    {
        if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            ClearTargets();
        }

        Collider2D[] soldiers = Physics2D.OverlapAreaAll(firstMousePos, mousePos, 1 << SM.SoldierLayer);

        if (soldiers.Length != 0)
            if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                ClearTargets();
            }

        foreach (Collider2D sold in soldiers)
        {
            Soldier soldd = sold.GetComponent<Soldier>();
            Soldier officer = SM.GetOfficer(soldd);
            if (officer && officer.army == yourArmy)
            {
                Soldier[] subs = SM.AsOfficer(officer).subs;

                if (!targets.Contains(officer))
                {
                    officer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_Officer_HL_" + yourArmy);
                    plusTarget(officer);
                    foreach (Soldier sol in subs)
                        if (sol != null)
                            sol.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_HL_" + yourArmy);
                }
            }
        }

    }

    private static void ClearTargets()
    {
        foreach (Soldier officer in targets)
        {
            Soldier[] subs = SM.AsOfficer(officer).subs;

            officer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_Officer_" + yourArmy);
            foreach (Soldier sold in subs)
                if (sold != null)
                    sold.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_" + yourArmy);
        }
        targets.Clear();
    }

    private static Transform GetObjectUnderMouse(int Layer)
    {
        Collider2D midObj = Physics2D.OverlapPoint(mousePos, 1 << Layer);
        if (midObj)
            return midObj.transform;
        return null;
    }

    private static void plusTarget(Soldier target)
    {
        targets.Add(target);
    }

    public static void minusTarget(Soldier target)
    {
        target.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_Officer_" + yourArmy);
        targets.Remove(target);
    }

    public static bool Contains(Soldier target)
    {
        return targets.Contains(target);
    }

    private void DrawRect(Vector3 p1, Vector3 p2)
    {
        Transform rect = transform.Find("Rect");
        LineRenderer left = rect.Find("Left").GetComponent<LineRenderer>();
        LineRenderer right = rect.Find("Right").GetComponent<LineRenderer>();
        LineRenderer top = rect.Find("Top").GetComponent<LineRenderer>();
        LineRenderer bottom = rect.Find("Bottom").GetComponent<LineRenderer>();
        float zC = -10;
        Vector3 topLeft = new Vector3(Mathf.Min(p1.x, p2.x), Mathf.Max(p1.y, p2.y), zC);
        Vector3 topRight = new Vector3(Mathf.Max(p1.x, p2.x), Mathf.Max(p1.y, p2.y), zC);
        Vector3 bottomLeft = new Vector3(Mathf.Min(p1.x, p2.x), Mathf.Min(p1.y, p2.y), zC);
        Vector3 bottomRight = new Vector3(Mathf.Max(p1.x, p2.x), Mathf.Min(p1.y, p2.y), zC);

        float w = 0.10f;
        float width = w * uiCamera.GetComponent<Camera>().orthographicSize / 20;

        left.SetWidth(width, width);
        left.SetPosition(0, topLeft);
        left.SetPosition(1, bottomLeft);

        right.SetWidth(width, width);
        right.SetPosition(0, topRight);
        right.SetPosition(1, bottomRight);

        top.SetWidth(width, width);
        top.SetPosition(0, topLeft);
        top.SetPosition(1, topRight);

        bottom.SetWidth(width, width);
        bottom.SetPosition(0, bottomLeft);
        bottom.SetPosition(1, bottomRight);
    }
}
