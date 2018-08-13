using UnityEngine;
using System.Collections;

public class Soldier_Upper : MonoBehaviour
{
    public string army = "B";
    public string eqSet = "Sword";

    private void Start()
    {
        TurnBarsUp();
    }
    protected void StartNow()
    {
        Start();
    }

    private void Update()
    {
        TurnBarsUp();
        //if (transform.Find("Soldier").GetComponent<Soldier>().health <= 0)
        //    DestroyIt();
    }
    protected void UpdateNow()
    {
        Update();
    }

    private void TurnBarsUp()
    {
        transform.Find("Bars").rotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f), Vector3.up);
    }

    public void DestroyIt()
    {
        //Debug.Log("DEAD END");
        transform.Translate(new Vector3(0, 0, 0.6f));

        Transform sold = transform.Find("Soldier");
        SM.RemoveSoldier(sold);
        Soldier sol = sold.GetComponent<Soldier>();
        if (SM.IsWarlord(sol))
        {
            if (army == CM.yourArmy)
            {
                SM.OnDefeat();
            }
            else
            {
                SM.OnVictory();
            }
        }
        else if (SM.IsOfficer(sol))
        {
            foreach (Soldier s in SM.AsOfficer(sol).subs)
            {
                if (s != null)
                {
                    s.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_" + army + "_NO");
                    Private priv = SM.AsPrivate(s);
                    priv.deltaV = Vector3.zero;
                    priv.index = -1;
                    priv.SetOfficer(null);
                }
            }
        }
        else if (SM.IsPrivate(sol))
        {
            if (SM.AsPrivate(sol).officer != null)
            {
                Officer off = SM.AsOfficer(SM.AsPrivate(sol).officer);
                off.subs[SM.AsPrivate(sol).index] = null;
            }
        }

        sold.GetComponent<Collider2D>().enabled = false;
        //sold.GetComponent<Collider2D>().isTrigger = false;
        //transform.GetComponent<Rigidbody2D>().mass = 0.01f;
        //sold.gameObject.layer = LayerMask.NameToLayer("Default");
        sold.GetComponent<Soldier>().enabled = false;
        sold.GetComponent<Animator>().enabled = false;
        GameObject.Destroy(sold.Find("nonTrigger").gameObject);
        GameObject.Destroy(sold.Find("Textures").gameObject);

        Transform lH = sold.Find("L_Hand");
        Transform rH = sold.Find("R_Hand");
        lH.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Hand_G");
        lH.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        lH.GetComponent<SpriteRenderer>().sortingOrder = 2;
        lH.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = 1;
        rH.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Hand_G");
        rH.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        rH.GetComponent<SpriteRenderer>().sortingOrder = 2;
        rH.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = 1;
        Transform lW = lH.childCount > 1 ? lH.GetChild(1) : null;
        Transform rW = rH.childCount > 1 ? rH.GetChild(1) : null;
        if (lW != null)
            lW.GetComponent<InHand>().Drop();
        if (rW != null)
            rW.GetComponent<InHand>().Drop();

        //GameObject.Destroy(transform.Find("Bars").GetComponent<Canvas>());
        GameObject.Destroy(transform.Find("Bars").gameObject);

        CM.minusTarget(sol);
        foreach (AI ai in FindObjectsOfType<AI>())
            ai.minusTarget(sol);
        sold.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Soldier/S_Base_G_" + army);
        sold.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        sold.GetComponent<SpriteRenderer>().sortingOrder = 2;
        sold.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = 1;

        transform.SetParent(SM.trash.transform);

        transform.GetComponent<Soldier_Upper>().enabled = false;
    }
}
