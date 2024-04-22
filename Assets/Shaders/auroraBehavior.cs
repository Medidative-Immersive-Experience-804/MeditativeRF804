using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auroraBehavior : MonoBehaviour
{

    //Blue 0.55
    //Red 0.47
    //20 = 0.08
    
    public Material Material;
    Color Blue, Green, Purple, ColorOne, ColorTwo; 
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
        //Setting the different Colors
        Blue = Color.clear;
        Blue.r = 0f;
        Blue.g = 0f;
        Blue.b = 1f;

        Purple = Color.clear;
        Purple.r = 0.47f;
        Purple.g = 0f;
        Purple.b = 0.55f;

        Green = Color.clear;
        Green.r = 0f;
        Green.g = 1f;
        Green.b = 0f;

        ColorOne = Blue;
        ColorTwo = Green;
        
        Material.SetColor("_ColorOne", ColorOne);
        Material.SetColor("_ColorTwo", ColorTwo);
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
        Material.SetColor("_ColorOne", ColorOne);
        Material.SetColor("_ColorTwo", ColorTwo);
        Material.SetVector("_speed", Speed);
    }

    public void updateColorX()
    {
        //Maps x pos to green color
        if (getCubePosX(ColorCube) > xMin && getCubePosX(ColorCube) < xMax/2f)  
        {
            ColorTwo = Color.Lerp(Green, Blue, map(getCubePosZ(ColorCube), xMin, xMax/2f, 0, 1));
        }
        if (getCubePosX(ColorCube) > xMax/2f && getCubePosX(ColorCube) < xMax)
        {
            ColorTwo = Color.Lerp(Blue, Purple, map(getCubePosZ(ColorCube), xMax/2f, xMax, 0, 1));
        }
    }

    public void updateColorZ()
    {
        //Maps x pos to green color
        if (getCubePosZ(ColorCube) > zMin && getCubePosZ(ColorCube) < zMax / 2f)
        {
            ColorOne = Color.Lerp(Green, Blue, map(getCubePosZ(ColorCube), zMin, zMax / 2f, 0, 1));
        }
        if (getCubePosX(ColorCube) > zMax / 2f && getCubePosX(ColorCube) < zMax)
        {
            ColorOne = Color.Lerp(Blue, Purple, map(getCubePosZ(ColorCube), zMax / 2f, zMax, 0, 1));
        }
    }


    public void updateColor()
    {
        updateColorX();
        updateColorZ();
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
