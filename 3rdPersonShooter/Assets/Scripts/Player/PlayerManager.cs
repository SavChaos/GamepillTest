using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseEntity
{
    public Camera _camera;
    public BaseWeapon weapon;

    public float speed = 10f;

    public float horizTurnSpeed = 3f;
    public float verticalTurnSpeed = 3f;

    public float m_GroundCheckDistance = 2f;

    Vector3 m_GroundNormal;
    public bool m_IsGrounded;

    

    private void Awake()
    {
        AIAttackModule.OnPlayerAttacked += OnPlayerAttacked;
    }

    public Vector3 playerCorePos
    {
        get
        {
            return transform.position + playerHeightOffset;
        }
    }

    public Vector3 playerHeightOffset
    {
        get
        {
            CapsuleCollider capasuleCol = _collider as CapsuleCollider; //offset the height of the raycast from center of player
            return new Vector3(0, capasuleCol.height / 2f);
        }
    }

    public void ShootWeapon()
    {
        _animator.SetTrigger("Shoot");
    }

    void OnPlayerAttacked(float damage)
    {
        CurrentHealth -= damage;
    }

    void ShootPositionReached()
    {
        weapon.ShootWeapon();
    }
}
