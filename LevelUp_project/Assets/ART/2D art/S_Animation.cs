using UnityEngine;

public class S_Animation : MonoBehaviour
{
    public Animator animator; // Arrastra tu Animator desde el Inspector
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
       

        if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("WalkingBackward", false);
            animator.SetBool("WalkingForward", false);
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingRight", true);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("WalkingBackward", false);
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingForward", true);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingForward", false);
            animator.SetBool("WalkingBackward", true);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingBackward", false);
            animator.SetBool("WalkingForward", false);
            animator.SetBool("WalkingLeft", true);
        }
    }
}