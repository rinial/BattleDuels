using UnityEngine;
using System.Collections;


public class Soldier_Textures : MonoBehaviour
{

    private static string _army;

    void Start()
    {
        Soldier s = transform.parent.GetComponent<Soldier>();
        if (!SM.IsWarlord(s))
        {
            _army = SM.GetArmy(transform.parent);

            int _LtexNum = Resources.LoadAll<Sprite>("S_Patterns/" + _army + "/L/").Length;
            int _RtexNum = Resources.LoadAll<Sprite>("S_Patterns/" + _army + "/R/").Length;

            transform.Find("Ltex").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("S_Patterns/" + _army + "/L/S_Pattern_L_" + SM._rnd.Next(_LtexNum) + "_" + _army);
            transform.Find("Rtex").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("S_Patterns/" + _army + "/R/S_Pattern_R_" + SM._rnd.Next(_RtexNum) + "_" + _army);

            if (!SM.IsOfficer(s))
            {
                int _MtexNum = Resources.LoadAll<Sprite>("S_Patterns/" + _army + "/M/").Length;
                transform.Find("Mtex").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("S_Patterns/" + _army + "/M/S_Pattern_M_" + SM._rnd.Next(_MtexNum) + "_" + _army);
            }
        }
    }

    void Update()
    {
        // Blood Textures Here
    }
}
