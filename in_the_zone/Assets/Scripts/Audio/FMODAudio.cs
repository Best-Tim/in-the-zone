using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODAudio : MonoBehaviour
{
    FMOD.Studio.EventInstance metalMusicEventInstance;
    public EventReference metalMusicEventRef;

    FMOD.Studio.EventInstance synthMusicEventInsance;
    public EventReference synthMusicEventRef;

    FMOD.Studio.EventInstance drainSynthPaintEventInstance;
    public EventReference drainSynthPaintEventReference;

    FMOD.Studio.EventInstance drainMetalPaintEventInstance;
    public EventReference drainMetalPaintEventReference;


    FMOD.Studio.EventInstance checkpointEventInstance;
    public EventReference checkpointEventReference;

    // Start is called before the first frame update
    void Start()
    {
        

        
    }
    private void Awake()
    {
        metalMusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(metalMusicEventRef);
        synthMusicEventInsance = FMODUnity.RuntimeManager.CreateInstance(synthMusicEventRef);
        drainSynthPaintEventInstance = FMODUnity.RuntimeManager.CreateInstance(drainSynthPaintEventReference);
        drainMetalPaintEventInstance = FMODUnity.RuntimeManager.CreateInstance(drainMetalPaintEventReference);
        checkpointEventInstance = FMODUnity.RuntimeManager.CreateInstance(checkpointEventReference);
    }


    public void PlayMetalMusic()
    {
        metalMusicEventInstance.start();

    }

    public void PlaySynthMusic()
    {
        synthMusicEventInsance.start();
    }

    public void DrainSynthPaint()
    {
        drainSynthPaintEventInstance.start();

    }
    public void StopDrainingSynthPaint()
    {
        drainSynthPaintEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void DrainMetalPaint()
    {
        drainMetalPaintEventInstance.start();

    }
    public void StopDrainingMetalPaint()
    {
       drainMetalPaintEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public void PassCheckpoint()
    {
        checkpointEventInstance.start();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
