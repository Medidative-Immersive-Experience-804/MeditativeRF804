using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBalancing : MonoBehaviour
{
    public float ballRadius = 20f;
    public float maxSpeed = 0.8f;
    public float size = 256f;

    private Vector2 ballPosition;
    private Vector2 ballSpeed;

    void Start()
    {
        // Set screen size
        Screen.SetResolution((int)size, (int)size, false);

        ballPosition = new Vector2(size / 2, size / 2);
        ballSpeed = Vector2.zero;
    }

    void Update()
    {
        // Get mouse position
        float mouseX_mapped = Mathf.Lerp(-1f, 1f, Input.mousePosition.x / size);
        float mouseY_mapped = Mathf.Lerp(-1f, 1f, Input.mousePosition.y / size);

        // Adjust ball speed based on pitch and roll
        ballSpeed.x = mouseX_mapped;
        ballSpeed.y = mouseY_mapped;

        // Limit ball speed
        ballSpeed.x = Mathf.Clamp(ballSpeed.x, -maxSpeed, maxSpeed);
        ballSpeed.y = Mathf.Clamp(ballSpeed.y, -maxSpeed, maxSpeed);

        // Update ball position
        ballPosition += ballSpeed;

        // Boundaries checking
        if (ballPosition.x < ballRadius)
        {
            ballPosition.x = ballRadius;
            ballSpeed.x = 0;
        }
        else if (ballPosition.x > size - ballRadius)
        {
            ballPosition.x = size - ballRadius;
            ballSpeed.x = 0;
        }

        if (ballPosition.y < ballRadius)
        {
            ballPosition.y = ballRadius;
            ballSpeed.y = 0;
        }
        else if (ballPosition.y > size - ballRadius)
        {
            ballPosition.y = size - ballRadius;
            ballSpeed.y = 0;
        }

        // Draw ball (not needed in Unity as it is done automatically)
        // You can create a GameObject with a SpriteRenderer component for the ball
        // and update its position accordingly.
    }
}
