using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterMovement : MonoBehaviour
{
    public float wobble;
    [SerializeField] private float wobbleSwing;
    [SerializeField] private float wobbleSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, (Mathf.Sin(wobble)) * wobbleSwing);
        wobble = wobble + wobbleSpeed;
    }
}
