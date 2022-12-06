using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PointUIController : MonoBehaviour
{
    [SerializeField] GameDirector gameDirector;
    [SerializeField] List<Slider> sliderPrefabList = new List<Slider>();
    private List<Slider> sliderThemeChosen = new List<Slider>();
    private List<Slider> sliderInstantiated = new List<Slider>();
    public float maxScore = 500f;
    private List<Vector3> positions = new List<Vector3>();
    public String nameOfWinner;
    public List<Slider> instantiatedSliders;

    public delegate void winAction(string winnerName);
    
    public static event winAction onWin;
    
    
    private void Start()
    {
        sliderThemeChosen = new List<Slider>();
        sliderInstantiated = new List<Slider>();
        gameDirector = GetComponentInParent<GameDirector>();
        GetPlayerThemes();

        for (int i = 0; i < 4; i++)
        {
            positions.Add(new Vector3(0, 30 - (45 * i), 0));
            /* Number 1 position y = 30 , decrease 20 in y for each position
             * number 2 position y = 10
             * number 3 position y = -10 ... etc */
        }
        for (int i = 0; i < sliderThemeChosen.Count; i++)
        {
            Slider s = Instantiate(sliderThemeChosen[i], this.gameObject.transform);
            instantiatedSliders.Add(s);
            s.transform.localPosition = positions[i];
            s.value = 0;
            s.maxValue = maxScore;
            int counter = i + 1;
            s.gameObject.name = "Player" + counter;
            sliderInstantiated.Add(s);  
        }
        
    }

    private void GetPlayerThemes()
    {
        List<Transform> playerL = gameDirector._playerList;
        for (int i = 0; i < playerL.Count; i++)
        {
            switch (playerL[i].GetComponent<KartReferenceObtainer>().Kart.GetComponent<CharacterClassManager>().characterClassEnum)
            {
                case CharacterClassENUM.METAL:
                    Slider s = FindSliderByTheme(CharacterClassENUM.METAL);
                    sliderThemeChosen.Add(s);
                    break;
                case CharacterClassENUM.SYNTH:
                    s = FindSliderByTheme(CharacterClassENUM.SYNTH);
                    sliderThemeChosen.Add(s);
                    break;
                case CharacterClassENUM.DISCO:
                    s = FindSliderByTheme(CharacterClassENUM.DISCO);
                    sliderThemeChosen.Add(s);
                    break;
            }
        }
    }
    private float getPlayerNameScore(Transform t, CharacterClassENUM ce)
    {
        if (t.gameObject.GetComponentInChildren<CharacterClassManager>().characterClassEnum == ce)
        {
            string s = t.GetComponent<KartReferenceObtainer>().Kart.GetComponent<KartController>().nickname;
            return  gameDirector.gameObject.GetComponent<PointEventLibrary>().map[s];
        }
        return  0;
    }
    public Slider FindSliderByTheme(CharacterClassENUM ce)
    {
        foreach (Slider s in sliderPrefabList)
        {
            if (s.GetComponent<CharacterClassManager>().characterClassEnum == ce)
            {
                return s;
            }
        }
        return null;
    }
    private void Update()
    {
        foreach (Slider s in sliderInstantiated)
        {
            var map = gameDirector.gameObject.GetComponent<PointEventLibrary>().map;
            //uncomment if you want to make dynamic max score eg: max score gets updated based on current max score
            //if (map[s.name] >= maxScore)
            //{
            //    maxScore = map[s.name];
            //}
            s.value = map[s.name];
            CheckLeader();

            if (s.value >= maxScore)
            {
                nameOfWinner = s.name;
                onWin.Invoke(nameOfWinner);
            }
            //s.maxValue = maxScore;

        }
    }

    public void CheckLeader()
    {
        List<Slider> SortedList = sliderInstantiated.OrderBy(s => s.value).ToList();
        SortedList.Reverse();
        for (int i = 0; i < SortedList.Count; i++)
        {
            SortedList[i].transform.localPosition = positions[i];
        }
    }
}
