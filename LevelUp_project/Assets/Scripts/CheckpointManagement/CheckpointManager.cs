using UnityEngine;
//Prioridad baja para que cuando cargue esten todos los objetos disponibles
[DefaultExecutionOrder(100)]
public class CheckpointManager : MonoBehaviour
{
    //Lo hago singleton para poder acceder fácilmente desde otros scripts
    public static CheckpointManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Asegurarse de que solo haya una instancia
        }
    }
    public Checkpoint firstCheckpoint; // El primer checkpoint del nivel, se asigna en el editor
    private Checkpoint currentCheckpoint;

    void Start()
    {
        SetCurrentCheckpoint(firstCheckpoint); // Establecer el primer checkpoint como el actual
    }

    //Método que asigna el checkpoint actual
    public void SetCurrentCheckpoint(Checkpoint checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    //Método para obtener la posición de spawn del checkpoint actual
    public Vector3 GetSpawnPosition()
    {
        if (currentCheckpoint != null)
        {
            return currentCheckpoint.GetSpawnPosition(); // Retornar la posición de spawn del checkpoint actual
        }
        else
        {
            Debug.LogWarning("No current checkpoint set.");
            return Vector3.zero; // Retornar un valor por defecto si no hay un checkpoint actual
        }
    }
}
