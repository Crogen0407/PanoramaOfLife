using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] private float _speed;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _speed * Vector3.left * Time.deltaTime;
        if(transform.position.x < -18.7f)
		{
            transform.position = new Vector3(18.7f, transform.position.y);
		}
    }
}
