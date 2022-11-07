using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////
//  구름 이동
////
public class Scrolling : MonoBehaviour
{
    float startPosition = -8.0f;
    float endPosition = 5.0f;
    float speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed, 0, 0);

        if(transform.position.x >= endPosition) {
            transform.position = new Vector3(startPosition, transform.position.y, transform.position.z);
        }
    }
}
