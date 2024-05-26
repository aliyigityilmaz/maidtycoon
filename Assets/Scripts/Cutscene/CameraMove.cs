using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform cameraTarget;
    private float speed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MaidMoves.instance.spawned)
        {
            float moveSpeed = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, cameraTarget.position, moveSpeed);
        }
        
    }
}
