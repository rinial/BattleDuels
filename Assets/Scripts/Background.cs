using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    public float
        width,
        height,
        left,
        right,
        top,
        bottom;

    private Transform zeroTile;
    private Camera cam;

    void Start()
    {
        zeroTile = transform.Find("bgTileZero");
        Vector3 tileSize = zeroTile.GetComponent<SpriteRenderer>().bounds.size;
        width = tileSize.x;
        height = tileSize.y;

        cam = transform.name == "WLbackgroundL" ? SM.bWarlord.parent.Find("WLcam").GetComponent<Camera>() : transform.name == "WLbackgroundR" ? SM.rWarlord.parent.Find("WLcam").GetComponent<Camera>() : Camera.main;
        Vector3 topRight = cam.ScreenToWorldPoint(cam.WorldToScreenPoint(cam.transform.position) + new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
        Vector3 bottomLeft = cam.ScreenToWorldPoint(cam.WorldToScreenPoint(cam.transform.position) - new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));

        left = bottomLeft.x;
        right = topRight.x;
        top = topRight.y;
        bottom = bottomLeft.y;

        float l = zeroTile.position.x, r = l;
        float y = zeroTile.position.y, t = 0, b = 0;

        while (l > left - width)
        {
            t = y;
            b = y;

            while (t < top + height)
            {
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(l, t, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
                t += height;
            }

            while (b > bottom)
            {
                b -= height;
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(l, b, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
            }

            l -= width;
        }

        while (r < right)
        {
            r += width;

            t = y;
            b = y;

            while (t < top + height)
            {
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(r, t, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
                t += height;
            }

            while (b > bottom)
            {
                b -= height;
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(r, b, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
            }
        }

        left = l + width;
        right = r;
        top = t - height;
        bottom = b;
    }

    void Update()
    {
        Vector3 topRight = cam.ScreenToWorldPoint(cam.WorldToScreenPoint(cam.transform.position) + new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
        Vector3 bottomLeft = cam.ScreenToWorldPoint(cam.WorldToScreenPoint(cam.transform.position) - new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));

        float
            nL = bottomLeft.x,
            nR = topRight.x,
            nT = topRight.y,
            nB = bottomLeft.y;

        while (nL < left)
        {
            left -= width;

            float t0 = zeroTile.position.y, b0 = t0;

            while (t0 <= top)
            {
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(left, t0, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
                t0 += height;
            }

            while (b0 > bottom)
            {
                b0 -= height;
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(left, b0, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
            }
        }
        while (nL > left + width)
        {
            left += width;
        }

        while (nR > right)
        {
            right += width;

            float t0 = zeroTile.position.y, b0 = t0;

            while (t0 <= top)
            {
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(right, t0, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
                t0 += height;
            }

            while (b0 > bottom)
            {
                b0 -= height;
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(right, b0, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
            }
        }
        while (nR < right - width)
        {
            right -= width;
        }

        while (nT > top)
        {
            top += height;

            float l0 = zeroTile.position.x, r0 = l0;

            while (l0 > left)
            {
                l0 -= width;
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(l0, top, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
            }

            while (r0 <= right)
            {
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(r0, top, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
                r0 += width;
            }
        }
        while (nT < top - height)
        {
            top -= height;
        }

        while (nB < bottom)
        {
            bottom -= height;

            float l0 = zeroTile.position.x, r0 = l0;

            while (l0 > left)
            {
                l0 -= width;
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(l0, bottom, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
            }

            while (r0 <= right)
            {
                GameObject tile = (GameObject)Instantiate((GameObject)Resources.Load("bgTile"), new Vector3(r0, bottom, 0), transform.rotation);
                tile.transform.parent = transform;
                tile.gameObject.name = "bgTile";
                r0 += width;
            }
        }
        while (nB > bottom + height)
        {
            bottom += height;
        }
    }
}
