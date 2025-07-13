using System;
using System.Collections;
using UnityEngine;
[System.Serializable]
public class Dialogue
{
    //Por si queremos ampliarlo
    public enum CharacterName
    {
        Cyntia,

    }  
    public CharacterName characterName; // Nombre del personaje que habla

    [TextArea]     
    public string text;

}
