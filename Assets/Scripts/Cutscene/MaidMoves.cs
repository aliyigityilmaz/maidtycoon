using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaidMoves : MonoBehaviour
{
    public static MaidMoves instance;
    public bool spawned = false;

    public Transform target;
    public bool reached = false;
    public Animator animator;
    public Transform spawnPoint;

    private float speed = 1f;

    public GameObject dialogueBox;
    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        instance = this;
        transform.position = spawnPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(transform.position == target.position)
        {
            reached = true;
        }
        if (!reached)
        {
            animator.SetBool("isWalking", true);
            float moveSpeed = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        if (reached)
        {
            spawned = false;
            dialogueBox.SetActive(true);
        }
            
    }
    private void OnEnable()
    {
        spawned = true;
    }
    private void OnDisable()
    {
        spawned = false;
    }
}
