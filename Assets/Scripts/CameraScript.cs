using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour

{
    public float baseSpeed;
    private float dynamicSpeed;
    public GameObject target;
    public bool tracking;

    //make it private and camera later
    private Camera gameCamera;
    public float hue;
    public GameObject[] hudParts;
    [SerializeField] private int[] colorOffset;
    [SerializeField] private Vector3[] colorVector;

    void Start()
    {
        gameCamera = GetComponent<Camera>();
        InvokeRepeating("AdjustSpeed", 0.3f, 0.3f);
    }

    void Update()
    {
        //moving the camera
        if (tracking == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.localPosition.x, target.transform.localPosition.y, transform.position.z), dynamicSpeed * Time.deltaTime);
        }

        //changing the background color
        gameCamera.backgroundColor = Color.HSVToRGB(hue/360, 0.4f, 0.86f);
        //changing the color for different HUD elements
        for (float i = 0; i < hudParts.Length; i++)
        {
            float tempOffset = (hue - colorOffset[Mathf.RoundToInt(i)]);
            if (tempOffset < 0)
            {
                tempOffset = 360 + tempOffset;
            }

            //Vector4 tempVector = new Vector4(tempOffset, 1f, 0.9f, 1);
            //hudParts[Mathf.RoundToInt(i)].GetComponent<Image>().color = Color.HSVToRGB(tempVector);
            hudParts[Mathf.RoundToInt(i)].GetComponent<Image>().color = Color.HSVToRGB(tempOffset / 360, 1f, 0.9f);// - (i/10));
        }
        if (hue >= 360)
        {
            hue = 0;
        }
        else
        {
            hue = hue + (10 * Time.deltaTime);
        }
    }

    void AdjustSpeed()
    {
        dynamicSpeed = Mathf.Pow(baseSpeed, Vector3.Distance(target.transform.position, transform.position));
    }
}
