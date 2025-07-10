using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSequenceSO dialogueSequence;
    private bool hasTriggered = false;

    void Start()
    {
        //Lo inicializamos en falso porque como en principio es un juego que no tiene puntos de guardado
        // siempre se va a tener que ejecutar el diálogo. Se podría modificar con otro encargado de gestionar el punto de guardado
        hasTriggered = false;
    }

    //Método para activar el diálogo
    public void TriggerDialogue()
    {
        if (!hasTriggered)
        {
            hasTriggered = true; // Marcar como activado para evitar reactivaciones
            DialogueManager.instance.StartDialogueSequence(dialogueSequence);
        }
    }

    //Método de ejecución OnTriggerEnter2D
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player")) // Comprobar si el objeto que entra en el trigger es el jugador
        {
            TriggerDialogue(); // Llamar al método para activar el diálogo
        }
    }
}
