using UnityEngine;
using System.Collections;

public class TopPanel : MonoBehaviour
{
    void Start() 
    {
        string army = (transform.name[transform.name.Length-2]=='L')?"B":"R";
        Color color = army == CM.yourArmy ? new Color32(87, 193, 0, 255) : new Color32(193, 0, 0, 255);
        transform.Find("Health").Find("Filler").GetComponent<UnityEngine.UI.Image>().color = color;
	}

    void Update()
    {

    }
}
