using UnityEngine;
using System.Collections;

public class Soldier_Bars : MonoBehaviour
{
    private void Start()
    {
        TurnUp();
        Color color = SM.GetArmy(transform) == CM.yourArmy ? new Color32(87, 193, 0, 255) : new Color32(193, 0, 0, 255);
        transform.Find("Health").Find("Filler").GetComponent<UnityEngine.UI.Image>().color = color;
    }

    private void Update()
    {
        TurnUp();
    }

    private void TurnUp()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f), Vector3.up);
    }
}
