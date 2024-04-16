using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auroraBehavior : MonoBehaviour
{
    public Material Material;
    Color Color;

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
        Material.SetColor("_Color", Color);
    }

    void updateGreen(float paceOfChange)
    {
        Color.g += paceOfChange;
    }

    void updateBlue(float paceOfChange)
    {
        Color.b += paceOfChange;
    }

    float getGreen()
    {
        return Color.g;
    }

    float getBlue()
    {
        return Color.b;
    }
    
}
