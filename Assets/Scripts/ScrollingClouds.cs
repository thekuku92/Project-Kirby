using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingClouds : MonoBehaviour
{
    public float scrollSpeed = -1.0f;  // Speed of the cloud scrolling

    public float screenBoundsX = -1.95f;

    void Update()
    {
        // Update the position of the clouds based on scrollSpeed and Time.deltaTime
        transform.position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);

        // Reset the position of the clouds when they go off-screen
        if (transform.position.x < screenBoundsX)
        {

            // Set the new position of the clouds
            transform.position = new Vector2(2, 0);
        }
    }
}