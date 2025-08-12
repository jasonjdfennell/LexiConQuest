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
    //public GameObject[] hudParts;
    public List<GameObject> hudParts = new List<GameObject>();
    public GameObject[] trailParts;
    [SerializeField] private List<int> colorOffset = new List<int>();
    //[SerializeField] private Vector3[] colorVector;

    public GameObject backgroundShape;
    public Sprite[] shapeSprites;

    void Start()
    {
        gameCamera = GetComponent<Camera>();
        InvokeRepeating("AdjustSpeed", 0.3f, 0.3f);
        //InvokeRepeating("SpawnShape", 0f, 5f);
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
        for (float i = 0; i < hudParts.Count; i++)
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

        //doing the same thing for the trail
        for (float j = 0; j < trailParts.Length; j++)
        {
            float tempOffset2 = (hue - colorOffset[hudParts.Count+Mathf.RoundToInt(j)]);
            if (tempOffset2 < 0)
            {
                tempOffset2 = 360 + tempOffset2;
            }
            trailParts[Mathf.RoundToInt(j)].GetComponent<SpriteRenderer>().color = Color.HSVToRGB(tempOffset2 / 360, 1f, 0.9f);
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

    void SpawnShape()
    {
        GameObject tempShape = Instantiate(backgroundShape);
        tempShape.SetActive(true);
        tempShape.GetComponent<SpriteRenderer>().sprite = shapeSprites[Random.Range(0,shapeSprites.Length)];
    }

    void AdjustSpeed()
    {
        dynamicSpeed = Mathf.Pow(baseSpeed, Vector3.Distance(target.transform.position, transform.position));
    }
}
