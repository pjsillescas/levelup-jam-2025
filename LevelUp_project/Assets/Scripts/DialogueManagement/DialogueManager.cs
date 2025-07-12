using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    //Se hace singleton para que solo haya un gestor de diálogos en toda la escena
    public static DialogueManager instance;

    // public static Action OnDialogueEnded; //Evento que se dispara al finalizar un diálogo
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; //Asignar la instancia si no existe
            DontDestroyOnLoad(gameObject); //No destruir este objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject); //Destruir este objeto si ya existe una instancia
        }
    }
    //Referencias a los objetos que llenan el dialogo
    [SerializeField] TMP_Text text; //Texto
                                    //Aquí se podría añadir una referencia al banco de retratos de jugador para cambiar entre ellos si se desea

    [SerializeField] GameObject dialoguePanel; //Panel del diálogo que contiene el texto del jugador y del PNJ
    [Tooltip("Velocidad del texto (1 = lento, 10 = rápido)")]
    [SerializeField][Range(1f, 10f)] float textSpeed = 5f; //Velocidad del texto
    [Tooltip("Tiempo máximo (en segundos) hasta que pase automáticamente al siguiente diálogo")]
    [SerializeField] float maxTimeBetweenDialogues = 2.5f; //Tiempo máximo entre diálogos
    private float calculatedSpeed;
    private Language currentLanguage; //Idioma actual del juego, se puede cambiar en el menú opciones


    void Start()
    {
        HideDialoguePanel(); //Asegurarse de que el panel de diálogo está oculto al inicio del juego
    }


    //Función para animar el llenado de texto en los bocadillos
    public void WriteText(TMP_Text textObject, string textToFill, float txtSpeed)
    {
        //Se usa un callback para poder llamar a la siguiente fase una vez se haya rellenado el bocadillo, si no el juego funciona mal.
        StartCoroutine(FillWritedTextCo(textObject, textToFill, txtSpeed));
    }

    IEnumerator FillWritedTextCo(TMP_Text Text, string textToFill, float txtSpeed)
    {
        float time = Time.time + 0.3f;
        //Limpiar el texto del bocadillo antes de empezar a escribir
        Text.text = "";
        //Bucle para escribir los carácteres uno a uno
        foreach (char c in textToFill)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && time < Time.time)
            {
                //Si se pulsa espacio, se salta la animación de escritura y se muestra el texto completo
                Text.text = textToFill;
                yield break; //Salir de la corroutine para no seguir escribiendo
            }
            if (dialoguePanel.activeSelf == false)
                ShowDialoguePanel(); //Asegurarse de que el panel de diálogo está visible

            yield return new WaitForSeconds(txtSpeed);

            //!Gestión de juego en pausa?

            //Si el juego esta en pausa se espera a que se reanude
            //yield return new WaitUntil(() => GameManager.instance.IsGamePaused() == false);
            //Rellenar el bocadillo de texto letra a letra

            Text.text += c;
        }
        yield return null; //Esperar un frame para asegurar que el texto se ha actualizado
    }

    //Método para mostrar el panel de diálogo
    public void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    //Método para ocultar el panel de diálogo
    public void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
        text.text = ""; //Limpiar el texto del jugador al ocultar el panel
    }

    //Método para ejecutar toda una secuencia de diálogo
    IEnumerator ExecuteDialogueSequence(DialogueSequenceSO dialogueSequence)
    {
        yield return new WaitUntil(() => dialoguePanel.activeSelf == false); //Esperar a que el panel de diálogo esté oculto antes de iniciar la secuencia
        ShowDialoguePanel();
        for (int i = 0; i < dialogueSequence.dialogues.Count; i++)
        {
            Dialogue dialogue = dialogueSequence.dialogues[i];
            //Escribir el texto del diálogo
            calculatedSpeed = 1 / (textSpeed * 20f); //Reiniciar la velocidad del texto
            WriteText(text, dialogue.text, calculatedSpeed);
            //Esperar a que el texto se haya escrito completamente
            yield return new WaitUntil(() => text.text == dialogue.text);
            //Al acabar esperar un tiempo antes de continuar con el siguiente diálogo
            yield return new WaitForSeconds(0.5f);
            float time = Time.time + maxTimeBetweenDialogues; //Tiempo máximo para esperar antes de continuar
            //Esperar a pulsar una tecla para continuar
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || time < Time.time);
        }

        HideDialoguePanel(); //Ocultar el panel de diálogo al finalizar la secuencia


        yield return null; //Esperar un frame para asegurar que el panel se ha ocultado correctamente

    }

    //Método para iniciar una secuencia de diálogo
    public void StartDialogueSequence(DialogueSequenceSO dialogueSequence)
    {


        //!Aquí se podría añadir la gestión de la miniatura del interlocutor

        StartCoroutine(ExecuteDialogueSequence(dialogueSequence));
    }

    //Método para cambiar el idioma del juego
    public void ChangeLanguage(int languagueIndex)
    {
        currentLanguage = Language.Castellano;
        currentLanguage += languagueIndex; //Actualizar el idioma actual
    }

    //Método para obtener el indice del idioma actual
    public int GetCurrentLanguageIndex()
    {
        return (int)currentLanguage; //Devolver el índice del idioma actual
    }

}
