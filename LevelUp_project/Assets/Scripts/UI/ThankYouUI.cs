using TMPro;
using UnityEngine;

public class ThankYouUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI thankYouText;
    [SerializeField] private TextMeshProUGUI goBackText;


    private void Start()
    {
    int languageIndex =  DialogueManager.instance.GetCurrentLanguageIndex();

        switch (languageIndex)
        {
            case 0:
                {
                    thankYouText.text = "¡Gracias por jugar!";
                    goBackText.text = "Volver al menu";
                    break;
                }
            case 1:
                {
                    thankYouText.text = "Gr�cies per jugar!";
                    goBackText.text = "Tornar al men�";

                    break;
                }
            case 2:
                {
                    thankYouText.text = "Thank you for playing!";
                    goBackText.text = "Back to menu";
                    break;
                }




        }


    }
}
