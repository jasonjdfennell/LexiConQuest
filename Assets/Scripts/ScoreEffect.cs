using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreEffect : MonoBehaviour
{
    private float opacity;
    public bool UI;

    // Start is called before the first frame update
    void Start()
    {
        opacity = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(UI == true)
        {
            GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, opacity);
            transform.position = transform.position + (transform.up * Time.deltaTime * 50);
        }
        else
        {
            GetComponent<TextMeshPro>().color = new Color(0, 0, 0, opacity);
            transform.position = transform.position + (transform.up * Time.deltaTime * 5);
        }
        opacity = opacity - Time.deltaTime;
        if(opacity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
