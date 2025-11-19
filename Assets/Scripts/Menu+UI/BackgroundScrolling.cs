using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    public float speed;
    public float resetX = -20f;
    public float startX = 20f; 

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x <= resetX)
        {
            Vector3 newPos = transform.position;
            newPos.x = startX;
            transform.position = newPos;
        }
    }
}
