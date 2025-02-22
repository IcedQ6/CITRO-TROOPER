using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineRenderScript : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>(); 
    }

    void Update()
    {
        lineRenderer.SetPosition(0, object1.transform.position); 

        lineRenderer.SetPosition(1, object2.transform.position); 
    }

}
