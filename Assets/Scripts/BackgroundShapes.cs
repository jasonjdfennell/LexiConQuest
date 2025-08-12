using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShapes : MonoBehaviour
{

    private float moveSpeed;
    private float rotateSpeed;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(camera.transform.position.x - (camera.orthographicSize * 2), camera.transform.position.y + Random.Range(-10, 10));
        moveSpeed = Random.Range(10, 100);
        rotateSpeed = Random.Range(10, 100);
        
    }

    void Update()
    {
        transform.position = transform.position + (transform.right * Time.deltaTime * moveSpeed);
        if (transform.position.x >= camera.transform.position.x + (camera.orthographicSize * 2))
        {
            Destroy(gameObject);
        }
    }
}
