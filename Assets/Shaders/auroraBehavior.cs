using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class auroraBehavior : MonoBehaviour
{    
    public Material Material;
    Color Blue, Green, Purple, ColorOne, ColorTwo; 
    public Vector2 Speed;
    public GameObject ColorCube, SizeCube, PaceCube;
    float speedIncrease = 0.005f;
    float xMin = -0.277f;
    float xMax = 0.473f;
    float zMin = -0.409f;
    float zMax = 0.336f;
    float yMin = 0.001f;
    float yMax = 0.05f;
    float redBlueThresh = 0.08f;

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
        updateColor();
        
        //Setting the Material shader variables
        Material.SetColor("_ColorOne", ColorOne);
        Material.SetColor("_ColorTwo", ColorTwo);
        //Material.SetVector("_speed", Speed);
    }

    public void updateColorX()
    {
        //Maps x pos to green color
        if (getCubePosX(ColorCube) > xMin && getCubePosX(ColorCube) < xMax/2f)  
        {
            ColorTwo = Color.Lerp(Green, Blue, map(getCubePosX(ColorCube), xMin, xMax/2f, 0, 1));
            Debug.Log("ColorTwo should be between  Green and blue");
        }
        if (getCubePosX(ColorCube) > xMax/2f && getCubePosX(ColorCube) < xMax)
        {
            ColorTwo = Color.Lerp(Blue, Purple, map(getCubePosZ(ColorCube), xMax/2f, xMax, 0, 1));
            Debug.Log("ColorTwo should be between Blue and Purple");
        }
    }

    public void updateColorZ()
    {
        //Maps x pos to green color
        if (getCubePosZ(ColorCube) > zMin && getCubePosZ(ColorCube) < zMax / 2f)
        {
            ColorOne = Color.Lerp(Green, Blue, map(getCubePosZ(ColorCube), zMin, zMax / 2f, 0, 1));
            Debug.Log("ColorOne should be between Green and Blue");
        }
        if (getCubePosZ(ColorCube) > zMax / 2f && getCubePosZ(ColorCube) < zMax)
        {
            ColorOne = Color.Lerp(Blue, Purple, map(getCubePosZ(ColorCube), zMax / 2f, zMax, 0, 1));
            Debug.Log("ColorOne should be between Blue and Purple");
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
        return Cube.transform.localPosition.z;
    }

    float getCubePosX(GameObject Cube)
    {
        return Cube.transform.localPosition.x;

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
