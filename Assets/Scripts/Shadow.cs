using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour
{
    public static float shadowK = 1f;
    public static float dXshadow = -0.3f * shadowK;
    public static float dYshadow = -0.2f * shadowK;
    public static float dScaleSh = 0.1f;

    void Start()
    {
        string name = transform.parent.name;
        if (name == "Soldier")
        {
            if (SM.IsOfficer(transform.parent.GetComponent<Soldier>()))
            {
                name = "Officer";
            }
            if (SM.IsWarlord(transform.parent.GetComponent<Soldier>()))
            {
                name = "Warlord";
            }
        }
        transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shadows/Sh_" + name);
    }

    void Update()
    {
        float h = transform.parent.GetComponent<Projectile_Weapon>() == null ? (-transform.parent.position.z) : transform.parent.GetComponent<Projectile_Weapon>().zEnd;
        float scale = 1f - dScaleSh * h < 0 ? 0 : 1f - dScaleSh * h;
        transform.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(scale, scale, scale);
        transform.position = transform.parent.position + new Vector3(dXshadow * h, dYshadow * h, h);
    }
}
