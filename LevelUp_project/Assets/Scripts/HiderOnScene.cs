using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HiderOnScene : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName; // Nombre de la escena asignable desde el editor
    [SerializeField] private List<GameObject> uiElementsToHide;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        
        if (scene.name.Equals(targetSceneName))
        {
            foreach(GameObject element in uiElementsToHide)
            {
                element.SetActive(false);
            }


        }
        else
        {
            foreach (GameObject element in uiElementsToHide)
            {
                element.SetActive(true);
            }

        }
    }
}
