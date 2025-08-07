using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreEffect : MonoBehaviour
{
    private float opacity;

    // Start is called before the first frame update
    void Start()
    {
        opacity = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector2(transform.position.x, transform.position.y + Time.deltaTime*5);
        transform.position = transform.position + (transform.up * Time.deltaTime * 5);
        GetComponent<TextMeshPro>().color = new Color(0, 0, 0, opacity);
        opacity = opacity - Time.deltaTime;
        if(opacity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
