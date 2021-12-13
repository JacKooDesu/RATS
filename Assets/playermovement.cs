using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JacDev.Fix
{
    public class playermovement : MonoBehaviour
{
    public static Transform player;
    public float moveSpeed = 3f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        player = transform;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetBool("Shifting",true);
            animator.SetTrigger("Shift");
        }
        if(Input.GetKeyUp(KeyCode.LeftControl)){
             animator.SetBool("Shifting",false);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}

}
