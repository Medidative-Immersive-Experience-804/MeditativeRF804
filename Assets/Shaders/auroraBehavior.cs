using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auroraBehavior : MonoBehaviour
{
    public Material Material;
    float paceOfChange;
    Color Color;
    string PropertyName;



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

    }

    
}
