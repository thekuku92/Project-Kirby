using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    private float screenBoundsX = 2.06f;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

                // Reset the position of the clouds when they go off-screen
        if (transform.position.x > screenBoundsX)
        {

            Destroy(gameObject);
        }
    }
}
