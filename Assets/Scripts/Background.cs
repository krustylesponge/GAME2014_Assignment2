using UnityEngine;

public class Background : MonoBehaviour
{
    public float parallaxMultiplier = 0.5f;

    private Transform cam;
    private float spriteWidth;
    private float spriteHeight;
    private Vector3 startPos;

    void Start()
    {
        cam = Camera.main.transform;
        startPos = transform.position;

       
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

        spriteWidth = sr.bounds.size.x;
        spriteHeight = sr.bounds.size.y;
    }

    void Update()
    {
     
        float distX = cam.position.x * parallaxMultiplier;
        float distY = cam.position.y * parallaxMultiplier;

        transform.position = new Vector3(startPos.x + distX, startPos.y + distY, transform.position.z);

   
        float camDistX = cam.position.x - transform.position.x;
        float camDistY = cam.position.y - transform.position.y;

        // Horizontal Looping
        if (Mathf.Abs(camDistX) >= spriteWidth)
        {
            float offsetX = (camDistX > 0) ? spriteWidth : -spriteWidth;
            startPos.x += offsetX;
        }

        // Vertical Looping
        if (Mathf.Abs(camDistY) >= spriteHeight)
        {
            float offsetY = (camDistY > 0) ? spriteHeight : -spriteHeight;
            startPos.y += offsetY;
        }
    }
}
