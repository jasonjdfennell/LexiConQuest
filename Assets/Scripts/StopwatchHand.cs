using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopwatchHand : MonoBehaviour
{

    public int difficulty;

    void Start()
    {
        
    }

    void Update()
    {
        if(transform.localEulerAngles.z > 3)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - (difficulty*Time.deltaTime*360) / 10f);
        }
    }
}
