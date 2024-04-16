using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{
    public auroraBehavior auroraBehavior;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("m"))
        {
            auroraBehavior.updateBlue(0.005f);
            Debug.Log("m is down");
        }
        if (Input.GetKey("n"))
        {
            auroraBehavior.updateBlue(-0.005f);
            Debug.Log("n is down");
        }
    }
}
