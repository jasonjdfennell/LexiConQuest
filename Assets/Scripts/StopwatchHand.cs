using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchHand : MonoBehaviour
{

    public int difficulty;
    private Transform clicker;
    private Transform hand;
    public float handAngle;
    private bool countingDown;

    void Start()
    {
        clicker = transform.GetChild(0);
        hand = transform.GetChild(2);
        countingDown = true;
    }

    void Update()
    {
        //ROTATING THE HAND
        if(hand.localEulerAngles.z > 3)
        {
            hand.eulerAngles = new Vector3(0, 0, hand.eulerAngles.z - (difficulty*Time.deltaTime*360) / 10f);
            handAngle = hand.localEulerAngles.z;
        }
        else
        {
            //WHEN THE HAND REACHES ZERO FOR THE FIRST TIME
            if (countingDown == true)
            {
                Zero();
            }
        }
    }

    //RESETTING THE STOPWATCH
    public IEnumerator Click()
    {
        hand.localEulerAngles = new Vector3(0, 0, 359.9f);
        clicker.position = new Vector2(clicker.position.x, clicker.position.y - 8);
        countingDown = true;
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().color = new Color(1, 1, 1);
        }

        yield return new WaitForSeconds(0.15f);

        clicker.position = new Vector2(clicker.position.x, clicker.position.y + 8);
    }

    //TURNING THE WATCH RED AND GIVING A TIME BONUS OF ZERO
    private void Zero()
    {
        countingDown = false;
        hand.localEulerAngles = new Vector3(0, 0, 3);
        handAngle = 0;
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().color = new Color(1, 0, 0);
        }
    }
}
