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

    private Camera gameCamera;
    public float hue;
    public List<GameObject> hudParts = new List<GameObject>();
    public GameObject[] trailParts;
    [SerializeField] private Vector4[] colorVector;

    public GameObject backgroundShape;
    public Sprite[] shapeSprites;

    void Start()
    {
        gameCamera = GetComponent<Camera>();
        InvokeRepeating("AdjustSpeedChangeColor", 0f, 0.3f);
        //InvokeRepeating("SpawnShape", 0f, 5f);
    }

    void Update()
    {
        //CAMERA MOVEMENT
        if (tracking == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.localPosition.x, target.transform.localPosition.y, transform.position.z), dynamicSpeed * Time.deltaTime);
        }
    }

    void SpawnShape()
    {
        GameObject tempShape = Instantiate(backgroundShape);
        tempShape.SetActive(true);
        tempShape.GetComponent<SpriteRenderer>().sprite = shapeSprites[Random.Range(0,shapeSprites.Length)];
    }

    void AdjustSpeedChangeColor()
    {
        dynamicSpeed = Mathf.Pow(baseSpeed, Vector3.Distance(target.transform.position, transform.position));

        //CHANGING BACKGROUND COLOR
        gameCamera.backgroundColor = Color.HSVToRGB(hue / 360, 0.4f, 0.86f);
        //CHANGING HUD COLORS
        for (int i = 0; i < hudParts.Count; i++)
        {
            float tempOffset = (hue - colorVector[i].x);
            if (tempOffset < 0)
            {
                tempOffset = 360 + tempOffset;
            }

            hudParts[i].GetComponent<Image>().color = Color.HSVToRGB(tempOffset / 360, colorVector[i].y, colorVector[i].z) - new Color(0, 0, 0, 1 - colorVector[i].w);
        }
        //CHANGING TRAIL COLORS
        for (int j = 0; j < trailParts.Length; j++)
        {
            float tempOffset2 = (hue - colorVector[hudParts.Count + j].x);
            if (tempOffset2 < 0)
            {
                tempOffset2 = 360 + tempOffset2;
            }
            trailParts[j].GetComponent<SpriteRenderer>().color = Color.HSVToRGB(tempOffset2 / 360, colorVector[j].y, colorVector[j].z) - new Color(0, 0, 0, 1 - colorVector[j].w);
        }
        //INCREASING HUE
        if (hue >= 360)
        {
            hue = 0;
        }
        else
        {
            hue = hue + (1f);
            //hue = hue + (10 * Time.deltaTime);
        }
    }
}
