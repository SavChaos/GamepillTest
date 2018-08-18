using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : BasePickup
{
    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            base.OnTriggerEnter(collider);
            Main.GetInstance().currentPlayer.IncreaseSpeed();
        }
    }
}
