using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    private Material skybox;
    
    public Color StartColor;
    public Color MetalColor;
    public Color SynthColor;
    public Color DiscoColor;
    private Color currentColor;
    public float transitionTime = 2f;
    bool isCycling;

    private PointEventLibrary pel;
    private GameDirector gd;
    public float transitionInterval = 10;

    private float transitionIntervalTimer;
    // Start is called before the first frame update
    void Start()
    {
        skybox = RenderSettings.skybox;
        currentColor = StartColor;
        skybox.SetColor("_Tint", currentColor);
        pel = FindObjectOfType<PointEventLibrary>();
        gd = FindObjectOfType<GameDirector>();
        transitionIntervalTimer = transitionInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCycling)
        {
            transitionIntervalTimer -= Time.deltaTime;
            if (transitionIntervalTimer <= 0)
            {
                transitionIntervalTimer = transitionInterval;
                var map = pel.map;
                var sortedDict = from s in map orderby s.Value descending select s;
                foreach (Transform t in gd._playerList)
                {
                    if (t.GetComponentInChildren<KartController>().nickname == sortedDict.First().Key)
                    {
                        if (t.GetComponentInChildren<CharacterClassManager>().GetClass() == CharacterClassENUM.METAL)
                        {
                           StartCoroutine(CycleMaterial(MetalColor, transitionTime, skybox));
                        }
                        else if (t.GetComponentInChildren<CharacterClassManager>().GetClass() ==
                                 CharacterClassENUM.SYNTH)
                        {
                            StartCoroutine(CycleMaterial(SynthColor, transitionTime, skybox));
                        }
                        else if (t.GetComponentInChildren<CharacterClassManager>().GetClass() ==
                                 CharacterClassENUM.DISCO)
                        {
                            StartCoroutine(CycleMaterial(DiscoColor, transitionTime, skybox));
                        }
                    }
                }
            }
        }
        
    }
    
    IEnumerator CycleMaterial(Color endColor, float cycleTime, Material mat)
    {
        isCycling = true;
        float currentTime = 0;
        while (currentTime < cycleTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / cycleTime;
            currentColor = Color.Lerp(currentColor, endColor, t);
            mat.SetColor("_Tint", currentColor);
            yield return null;
        }
        isCycling = false;
    }
}
