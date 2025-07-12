using UnityEngine;
using System.Collections;
using player;

//Este método gestiona básicamente cuando el jugador debe respawnear, cómo y dónde
public class PlayerHealth : MonoBehaviour
{
    private Rigidbody rb; // Referencia al Rigidbody del jugador
    private Collider playerCollider; // Referencia al Collider del jugador

    [Header("Respawn Settings")]
    [SerializeField] private float arcHeight = 6.5f; // Altura máxima del arco ajustable desde el inspector

    private void Start()
    {
        //Inicializar las referencias
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        //Asegurarse de que el jugador está activo al inicio
        gameObject.SetActive(true);
    }
    //Gestión del respawn del jugador
    public void Respawn()
    {
        // Reseteo del padre, por si estaba en una plataforma
        transform.SetParent(null);
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

        //Esperar un segundo antes de iniciar el movimiento hacia el respawn
        yield return new WaitForSeconds(1f);

        //Mover al jugador en un arco hacia la posición del checkpoint
        float elapsedTime = 0f;
        float duration = 1f; // Duración del movimiento
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // Interpolación lineal entre la posición inicial y final
            Vector3 flatPosition = Vector3.Lerp(startPosition, respawnPosition, t);
            // Añadir la curvatura del arco
            float heightOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;
            transform.position = new Vector3(flatPosition.x, flatPosition.y + heightOffset, flatPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la posición final sea exacta
        transform.position = respawnPosition;

        //Reactivar al jugador
        SetPlayerActive(true);

        //! Añadir una animación de parpadeo en el sprite del jugador para indicar el respawn???
    }

    //Método para activar o desactivar los componentes del jugador
    public void SetPlayerActive(bool isActive)
    {
        rb.useGravity = isActive; // Activar o desactivar gravedad para evitar que siga cayendo y la camara lo siga
        playerCollider.enabled = isActive; // Activar o desactivar el Collider

        // Desactivar o activar el control del jugador
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = isActive;
        }
    }

    //Gestión del trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            Respawn(); // Si el jugador colisiona con una zona de muerte de jugador, respawnear
        }
    }

    //Gestión de la colisión
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Respawn(); // Si el jugador colisiona con un objeto de muerte, respawnear
        }
    }
}
