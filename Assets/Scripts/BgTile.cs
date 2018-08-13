using UnityEngine;
using System.Collections;

public class BgTile : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = transform.parent.name == "WLbackgroundL" ? SM.bWarlord.parent.Find("WLcam").GetComponent<Camera>() : transform.parent.name == "WLbackgroundR" ? SM.rWarlord.parent.Find("WLcam").GetComponent<Camera>() : Camera.main;
    }

    void Update()
    {
        Vector3 topRight = cam.ScreenToWorldPoint(cam.WorldToScreenPoint(cam.transform.position) + new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
        Vector3 bottomLeft = cam.ScreenToWorldPoint(cam.WorldToScreenPoint(cam.transform.position) - new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));

        float
            left = bottomLeft.x,
            right = topRight.x,
            top = topRight.y,
            bottom = bottomLeft.y;

        Vector3 pos = transform.position;
        if (pos.x < left - transform.parent.GetComponent<Background>().width || pos.x > right + transform.parent.GetComponent<Background>().width || pos.y < bottom - transform.parent.GetComponent<Background>().height || pos.y > top + transform.parent.GetComponent<Background>().height)
            GameObject.Destroy(gameObject);
    }
}
