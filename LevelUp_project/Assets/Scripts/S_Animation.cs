using UnityEngine;

public class CambiarAnimacion : MonoBehaviour
{
    public Animator animator; // Arrastra tu Animator desde el Inspector
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
       

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetBool("WalkingBackward", false);
            animator.SetBool("WalkingForward", false);
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingRight", true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetBool("WalkingBackward", false);
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingForward", true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingForward", false);
            animator.SetBool("WalkingBackward", true);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingBackward", false);
            animator.SetBool("WalkingForward", false);
            animator.SetBool("WalkingLeft", true);
        }
    }
}