using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;

    private Vector3 moveDirection;

    private CharacterController controller;
    public Animator animator;

    public int direction;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.skinWidth -= 0.0799f;
        controller.minMoveDistance -= 0.0009f;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();        
    }

    void Move()
    {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(moveX, 0f, moveZ);        

        if (moveDirection != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.X)) Run();
            else Walk();
        }
        else Idle();

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
        {
            moveDirection *= moveSpeed / 1.414f;
        }
        else moveDirection *= moveSpeed;

        controller.Move(moveDirection * Time.deltaTime);

        animator.SetFloat("Horizontal", moveX);
        animator.SetFloat("Vertical", moveZ);
        animator.SetFloat("Speed", moveSpeed);

        if ((Mathf.Abs(moveX) < Mathf.Abs(moveZ)) && moveZ > 0)
            direction = 1; //up
        else if ((Mathf.Abs(moveX) < Mathf.Abs(moveZ)) && moveZ < 0)
            direction = 2; //down
        else if ((Mathf.Abs(moveX) >= Mathf.Abs(moveZ)) && moveX < 0)
            direction = 3; //left
        else if ((Mathf.Abs(moveX) >= Mathf.Abs(moveZ)) && moveX > 0)
            direction = 4; //right

        SetLastDirection(direction);
    }

    private void Idle()
    {
        moveSpeed = 0;
    }
    
    private void Walk()
    {
        moveSpeed = walkSpeed;
    }

    private void Run()
    {
        moveSpeed = runSpeed;
    }

    void SetLastDirection(int direction)
    {
        switch(direction)
        {
            case 1:
                animator.SetBool("LastUp", true);
                animator.SetBool("LastDown", false);
                animator.SetBool("LastLeft", false);
                animator.SetBool("LastRight", false);
                break;
            case 2:
                animator.SetBool("LastUp", false);
                animator.SetBool("LastDown", true);
                animator.SetBool("LastLeft", false);
                animator.SetBool("LastRight", false);
                break;
            case 3:
                animator.SetBool("LastUp", false);
                animator.SetBool("LastDown", false);
                animator.SetBool("LastLeft", true);
                animator.SetBool("LastRight", false);
                break;
            case 4:
                animator.SetBool("LastUp", false);
                animator.SetBool("LastDown", false);
                animator.SetBool("LastLeft", false);
                animator.SetBool("LastRight", true);
                break;
        }
    }
}
