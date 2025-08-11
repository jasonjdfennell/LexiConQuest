using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterMovement : MonoBehaviour
{
    public float wobble;
    [SerializeField] private float wobbleSwing;
    [SerializeField] private float wobbleSpeed;
    
    void Start()
    {
        
    }

    //I had difficulty basing this on time.deltaTime, but that would be preferable.
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, (Mathf.Sin(wobble)) * wobbleSwing);
        wobble = wobble + wobbleSpeed;
    }
}
