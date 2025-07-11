using UnityEngine;
using System.Collections;

//Este método gestiona básicamente cuando el jugador debe respawnear, cómo y dónde
public class PlayerHealth : MonoBehaviour
{
    private Rigidbody rb; // Referencia al Rigidbody del jugador
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del jugador
    private Collider playerCollider; // Referencia al Collider del jugador

    private void Start()
    {
        //Inicializar las referencias
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerCollider = GetComponent<Collider>();

        //Asegurarse de que el jugador está activo al inicio
        gameObject.SetActive(true);
    }
    //Gestión del respawn del jugador
    public void Respawn()
    {
        //Obtener la posición del checkpoint actual
        Vector3 respawnPosition = CheckpointManager.instance.GetSpawnPosition();

        AudioManager.instance.PlaySFX(0); // Reproducir sonido al caer

        StartCoroutine(RespawnPlayer(respawnPosition));
    }

    //Corrutina para gestionar los tempos de respawn
    private IEnumerator RespawnPlayer(Vector3 respawnPosition)
    {
        //Desactivar el jugador durante el respawn
        SetPlayerActive(false);
        rb.linearVelocity = Vector3.zero; // Asegurarse de que el Rigidbody no se mueve durante el respawn

        //Esperar un segundo antes de respawnear
        yield return new WaitForSeconds(1f);

        //Reubicar al jugador en la posición del checkpoint
        transform.position = respawnPosition;

        //Reactivar al jugador
        SetPlayerActive(true);

        //! Añadir una animación de parpadeo en el sprite del jugador para indicar el respawn???
    }

    //Método para activar o desactivar los componentes del jugador
    public void SetPlayerActive(bool isActive)
    {
        rb.useGravity = isActive; // Activar o desactivar gravedad para evitar que siga cayendo y la camara lo siga
        spriteRenderer.enabled = isActive; // Activar o desactivar el SpriteRenderer
        playerCollider.enabled = isActive; // Activar o desactivar el Collider
    }

    //Gestión del trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            Respawn(); // Si el jugador colisiona con una zona de muerte de jugador, respawnear
        }
    }
}
