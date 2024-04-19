using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auroraBehavior : MonoBehaviour
{

    //Blue 0.55
    //Red 0.47
    //20 = 0.08
    public Material Material;
    Color Color;
    public Vector2 Speed;
    public GameObject ColorCube, SizeCube, PaceCube;
    float speedIncrease = 0.005f;
    float xMin = -0.508f;
    float xMax = 0.449f;
    float zMin = -0.506f;
    float zMax = 0.455f;
    float yMin = 0.001f;
    float yMax = 0.05f;
    float redBlueThresh = 0.08f;

    // Start is called before the first frame update
    void Start()
    {
        Color = Color.clear;
        Color.g = 1;
        Color.b = 1;
        Material.SetColor("_Color", Color);
    }

    // Update is called once per frame
    void Update()
    {
        //Moves the 
        moveSpeed();

        //Updates the color of the light according to the colorcube
        updateColor();

        //Update speed according to x pos of the speedcube
        updateSpeed();
        
        //Setting the Material shader variables
        Material.SetColor("_Color", Color);
        Material.SetVector("_speed", Speed);
    }

    public void updateGreen()
    {
        //Maps x pos to green color
        Color.g = map(getCubePosZ(ColorCube), xMin, xMax, 0, 1);
    }

    public void updateBlue()
    {
        //Maps z pos to blue color
        Color.b = map(getCubePosX(ColorCube), zMin, zMax, 0, 1);
    }

    public void updateRed()
    {
        //Maps y pos to red color
        Color.r = map(getCubePosY(ColorCube), yMin, yMax, 0f, 0.47f);

        //If statement makes sure we don't have to much red in the color compared to blue
        if(Color.r > Color.b - redBlueThresh)
        {
            Color.r = Color.b - redBlueThresh;
        }
    }

    public void updateColor()
    {
        updateGreen();
        updateBlue();
        updateRed();
    }

    public void updateSpeed()
    {
        speedIncrease = map(getCubePosX(PaceCube), xMin, xMax, -0.005f, 0.005f);
    }

    public void moveSpeed()
    {
        Speed.x += speedIncrease;
    }

    float getCubePosZ(GameObject Cube)
    {
        return Cube.transform.position.z;
    }

    float getCubePosX(GameObject Cube)
    {
        return Cube.transform.position.x;
    }

    float getCubePosY(GameObject Cube)
    {
        return Cube.transform.position.y;
    }


    //Remaps values to another from one arbitrery range to another
    public static float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }

}
