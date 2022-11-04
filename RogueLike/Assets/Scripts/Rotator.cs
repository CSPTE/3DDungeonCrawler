using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    //adjust this to change speed
    float speed = 3f;
    //adjust this to change how high it goes
    float height = 0.8f;
    Vector3 pos;

    void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);

       //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(transform.position.x, newY, transform.position.z) ;
    }
}
