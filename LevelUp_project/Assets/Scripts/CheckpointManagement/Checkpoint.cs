using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Usar para acabar de ajustar dónde se quiere spawnear al jugador al activar el checkpoint")]
    private Vector3 spawningOffset; // Desplazamiento de la posición de spawn del checkpoint

    private Vector3 spawnPosition; // Posición de spawn del checkpoint

    private void Start()
    {
        spawnPosition = transform.position + spawningOffset; // Calcular la posición de spawn al iniciar
    }

    //Método para activar el checkpoint
    public void ActivateCheckpoint()
    {
        CheckpointManager.instance.SetCurrentCheckpoint(this); // Establecer este checkpoint como el actual
    }

    //Gestión del trigger enter
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ActivateCheckpoint(); // Activar el checkpoint si el jugador entra en el trigger
        }
    }

    //Método para obtener la posición de spawn del checkpoint
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition; // Retornar la posición de spawn del checkpoint
    }
}

