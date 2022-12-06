using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class PointEventLibrary : MonoBehaviour
{
    public Dictionary<string, float> map;


    private void Start()
    {
        map = new Dictionary<string, float>();
        EventCallPointsAdd += AddPoints;
        EventCallPointsRemove += RemovePoints;

        if (TryGetComponent(out GameDirector gd))
        {
            foreach (Transform t in gd._playerList)
            {
                map.Add(t.gameObject.name, 0);
            }
        }
    }

    private delegate void PointsEventType(float i, string s);

    private event PointsEventType EventCallPointsAdd;
    private event PointsEventType EventCallPointsRemove;

    //main logic
    private void AddPoints(float i, string s)
    {
        if (map.ContainsKey(s))
        {
            map[s] += i;
            //Debug.Log(map[s]);
        }
    }

    private void RemovePoints(float i, string s)
    {
        if (map.ContainsKey(s))
        {
            map[s] -= i;
        }
    }


    //public events to be called externally
    //on paint
    public void PublicEventAddPointsPaint( string s)
    {
        if (EventCallPointsAdd != null)
        {
            EventCallPointsAdd(1, s);
        }
    }
    //on using power up
    public void PublicEventAddPointsPowerUps( string s)
    {
        if (EventCallPointsAdd != null)
        {
            EventCallPointsAdd(50, s);
        }
    }

    public void PublicEventAddPaintBar(string s)
    {
        if (EventCallPointsAdd != null)
        {
            EventCallPointsAdd(1, s);
        }
    }
    //custom
    public void PublicEventAddPoints(float i, string s)
    {
        if (EventCallPointsAdd != null)
        {
            EventCallPointsAdd(i, s);
        }
    }

    public void PublicEventRemPoints(float i, string s)
    {
        if (EventCallPointsRemove != null)
        {
            EventCallPointsRemove(i, s);
        }
    }
    
    public void RemovePaintPoints(string s)
    {
        if (EventCallPointsRemove != null)
        {
            EventCallPointsRemove(0.5f, s);
        }
    }
}
