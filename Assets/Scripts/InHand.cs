using UnityEngine;
using System.Collections;

public class InHand : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void Drop()
    {
        Collider2D col = transform.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
        transform.SetParent(SM.trash.transform);
        transform.position = transform.position - new Vector3(0, 0, transform.position.z - 0.1f);
        transform.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
        transform.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = 1;
        transform.GetComponent<InHand>().enabled = false;
    }
}
