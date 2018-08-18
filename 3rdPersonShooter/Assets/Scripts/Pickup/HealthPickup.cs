using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : BasePickup
{
    public float HEALTH_GAIN = 20;

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            base.OnTriggerEnter(collider);
            Main.GetInstance().currentPlayer.ChangeHealth(HEALTH_GAIN);
        }
    }

}
