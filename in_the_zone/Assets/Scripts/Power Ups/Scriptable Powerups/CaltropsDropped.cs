using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/SpeedDebuff")]
public class CaltropsDropped : PowerupEffect
{
    public int duration;
    private KartController kartController;

    // public string player ;       playerName from caltropsDrop
    public override void Apply(GameObject target) //OnTrigger(Collision)
    {
        // if (target.CompareTag("Player"))
        // {
        kartController = target.GetComponent<KartReferenceObtainer>().Kart.GetComponent<KartController>();
            Debuff();
        Debuff();
        // }
    }

    private void Speed(float x)
    {
        kartController.currentSpeed = x;
    }
    public void Debuff()
    {
        if (kartController != null)
        {
            DOVirtual.Float(kartController.speed, kartController.speed /= 2, duration, Speed);
        }
    }
}
