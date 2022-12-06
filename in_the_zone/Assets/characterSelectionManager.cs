using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSelectionManager : MonoBehaviour
{
    public float numberofplayers;
    public CharacterSelection characterSelection;
    public GameObject selectedCharacter;
    public CharacterClassENUM enumPlayer1;
    public CharacterClassENUM enumPlayer2;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        enumPlayer1 = CharacterClassENUM.EMPTY;
        enumPlayer2 = CharacterClassENUM.EMPTY;
    }

    // Start is called before the first frame update
    void Update()
    {
        numberofplayers = characterSelection.slider.value;
    }

    CharacterClassENUM characterClass() 
    {
        return selectedCharacter.GetComponent<CharacterClassManager>().GetClass();
    }
}
