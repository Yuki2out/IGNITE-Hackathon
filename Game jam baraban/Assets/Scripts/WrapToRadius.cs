using System;
using UnityEngine;

public class WarpToRadius : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject camera;

    public float radius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(targetObject.transform.position, transform.position);

        if (distance > radius)
        {
            float ogHeight = transform.position.y;
            transform.position = targetObject.transform.position - camera.transform.forward * (radius / 2f);
            transform.position = new Vector3(transform.position.x, ogHeight, transform.position.z);
        }
    }
}
