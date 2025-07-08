using UnityEngine;

public class JumpPlantManager : MonoBehaviour
{
    [SerializeField]
    private float launchForce;

    [SerializeField]
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LaunchPlayer(other.GetComponent<Rigidbody>());
        }
    }

        

    private void LaunchPlayer(Rigidbody playerRb)
    {
        //Reset the Y force before applying the impulse.
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);



        playerRb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

        animator.SetTrigger("LaunchPlayer");
    }

}
