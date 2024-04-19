using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auroraBehavior : MonoBehaviour
{
    public Material Material;
    Color Color;
    public Vector2 Speed;

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
        moveSpeed();
        Material.SetColor("_Color", Color);
        Material.SetVector("_speed", Speed);
    }

    public void updateGreen(float paceOfChange)
    {
        Color.g += paceOfChange;
    }

    public void updateBlue(float paceOfChange)
    {
        Color.b += paceOfChange;
    }

    public float getGreen()
    {
        return Color.g;
    }

    public float getBlue()
    {
        return Color.b;
    }

    public void moveSpeed()
    {
        Speed.x += 0.0005f;
    }
    
}
