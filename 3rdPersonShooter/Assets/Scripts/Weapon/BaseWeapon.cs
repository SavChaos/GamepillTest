using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public PlayerManager player;
    public GameObject Ammo_Prefab;
    public List<Ammo> ammoList = new List<Ammo>();
    public Transform nozzleMarker;
    const float SHOOT_FORCE = 25;
    public Vector3 shootVector;

    private void Awake()
    {
        shootVector = nozzleMarker.forward; //default
    }

    public void ShootWeapon()
    {        
        //return ammo from pool
        foreach (Ammo ammo in ammoList)
        {
            if(ammo.IsDead)
            {
                InitAmmo(ammo, shootVector);
                return;
            }
        }

        //if there is no object to use from pool, we instantiate a new one and store in the pool
        Ammo _ammo = Instantiate(Ammo_Prefab).GetComponent<Ammo>();
        ammoList.Add(_ammo);
        InitAmmo(_ammo, shootVector);
    }

    void InitAmmo(Ammo _ammo, Vector3 shootvector)
    {
        _ammo.Refresh(nozzleMarker.position);
        Ray shootRay = new Ray(player.playerCorePos, shootVector * 100);
        _ammo.Fire(shootRay, SHOOT_FORCE);
    }

	
}
