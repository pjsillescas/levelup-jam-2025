using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "Dialogue/Dialogue Sequence")]
public class DialogueSequenceSO : ScriptableObject
{
    // Clase ScriptableObject para definir una secuencia de diálogos
    //Crear el menú de creación de ScriptableObjects en Unity para poder crear diálogos fácilmente
    [Tooltip("Añadir un objeto a la lista por cada caja de diálogo que se quiera mostrar en la secuencia")]
    public List<Dialogue> dialogues = new List<Dialogue>(); // Lista de diálogos que componen la secuencia


}
