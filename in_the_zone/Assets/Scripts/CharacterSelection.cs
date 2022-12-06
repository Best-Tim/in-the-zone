using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	public int selectedCharacter = 1;

	characterSelectionManager csm;
	public Slider slider;
	public Text character2Text;

	private void Start()
	{
        csm = FindObjectOfType<characterSelectionManager>();
		character2Text.gameObject.SetActive(false);
    }

	public void NextCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter = (selectedCharacter + 1) % characters.Length;
		characters[selectedCharacter].SetActive(true);
	}

	public void PreviousCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter--;
		if (selectedCharacter < 0)
		{
			selectedCharacter += characters.Length;
		}
		characters[selectedCharacter].SetActive(true);
	}

	public void StartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
	}

	public void StartChecker()
    {
		if (slider.value == 1)
		{
            csm.enumPlayer1 = characters[selectedCharacter].GetComponent<CharacterClassManager>().GetClass();
			StartGame();
        }
		if (slider.value == 2)
		{
            csm.enumPlayer1 = characters[selectedCharacter].GetComponent<CharacterClassManager>().GetClass();

            if (csm.enumPlayer2 != CharacterClassENUM.EMPTY)
            {
                StartGame();
            }
            if (csm.enumPlayer2 == CharacterClassENUM.EMPTY)
            {
                character2Text.gameObject.SetActive(enabled);
                StartCoroutine(Counter());
            }
        }
        
    }

	IEnumerator Counter()
	{
		yield return new WaitForSeconds(3);
        character2Text.gameObject.SetActive(false);
    }

    public void SetPlayer2()
	{
        csm.enumPlayer2 = characters[selectedCharacter].GetComponent<CharacterClassManager>().GetClass();
    }
}
