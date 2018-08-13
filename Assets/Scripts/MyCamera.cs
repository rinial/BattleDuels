using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCamera : MonoBehaviour
{
    public float
        minSize = 2,
        maxSize = 100,
        startSize = 15;
    private float
        scaleSp = 10,
        buttonScaleSp = 10,
        xSp = 0.8f,
        ySp = 0.6f;
    private Vector3
        lastMousePos;

    private void Start()
    {
        lastMousePos = Input.mousePosition;
        foreach (Camera c in Camera.allCameras)
        {
            if (c.name != "WLcam")
                c.orthographicSize = startSize;
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Vector3 pos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float s = Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * scaleSp;
            s = s < minSize ? minSize : s > maxSize ? maxSize : s;

            foreach (Camera c in Camera.allCameras)
            {
                if (c.name != "WLcam")
                    c.orthographicSize = s;
            }

            Vector3 pos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = Camera.main.transform.position - (pos2 - pos1);

            foreach (Camera c in Camera.allCameras)
            {
                if (c.name != "WLcam")
                    c.transform.position = newPos;
            }
        }

        if (Input.GetKey(KeyCode.KeypadPlus) && !Input.GetKey(KeyCode.KeypadMinus))
        {
            float k = Camera.main.orthographicSize / 5;

            float s = Camera.main.orthographicSize - buttonScaleSp * Time.deltaTime * k;
            s = s < minSize ? minSize : s > maxSize ? maxSize : s;

            foreach (Camera c in Camera.allCameras)
            {
                if (c.name != "WLcam")
                    c.orthographicSize = s;
            }
        }
        else if (Input.GetKey(KeyCode.KeypadMinus) && !Input.GetKey(KeyCode.KeypadPlus))
        {
            float k = Camera.main.orthographicSize / 5;

            float s = Camera.main.orthographicSize + buttonScaleSp * Time.deltaTime * k;
            s = s < minSize ? minSize : s > maxSize ? maxSize : s;

            foreach (Camera c in Camera.allCameras)
            {
                if (c.name != "WLcam")
                    c.orthographicSize = s;
            }
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 newPos = Camera.main.transform.position - (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(lastMousePos));

            foreach (Camera c in Camera.allCameras)
            {
                if (c.name != "WLcam")
                    c.transform.position = newPos;
            }
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            foreach (Camera c in Camera.allCameras)
            {
                float k = Camera.main.orthographicSize / 15;
                if (c.name != "WLcam")
                    c.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * xSp * k, Input.GetAxis("Vertical") * ySp * k, 0));
            }
        }

        lastMousePos = Input.mousePosition;
    }
}
