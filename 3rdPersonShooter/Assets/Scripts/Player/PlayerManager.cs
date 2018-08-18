using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseEntity
{
    public Camera _camera;
    public BaseWeapon weapon;
    public InputManager input;
    public HUDManager hud;

    public float speed = 10f;
    public float speedMult = 1f;
    public float speedMultIncr = 0.5f;
    public float horizTurnSpeed = 3f;
    public float verticalTurnSpeed = 3f;

    public float m_GroundCheckDistance = 2f;

    Vector3 m_GroundNormal;
    public bool m_IsGrounded;
        

    private void Awake()
    {
        AIAttackModule.OnPlayerAttacked += OnPlayerAttacked;
        hud.UpdateChevron();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AIAttackModule.OnPlayerAttacked -= OnPlayerAttacked;
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
        if (IsDead)
            return;

        ChangeHealth(-damage);
    }

    public override void ChangeHealth(float change)
    {
        base.ChangeHealth(change);
        hud.UpdateHealthBar(CurrentHealth, TotalHealth);
    }

    const int MaxSpeed = 3;
    float speedReductionInterval = 5;   //reduce speed each interval
    Coroutine speedReturnCoroutine = null;

    public void IncreaseSpeed()
    {
        speedMult += speedMultIncr;
        speedMult = Mathf.Clamp(speedMult, 1, 2.5f);

        SpeedPickup();

        if (speedReturnCoroutine != null)
        {
            StopCoroutine(DoSpeedReduction());
        }

        speedReturnCoroutine = StartCoroutine(DoSpeedReduction());
    }

    //gradually reduce speed multiplier, falling back to 1
    IEnumerator DoSpeedReduction()
    {
        yield return new WaitForSeconds(speedReductionInterval);

        if (speedMult > 1)
        {
            speedMult -= speedMultIncr;
        }

        //update ui
        hud.UpdateChevron();

        if (speedMult > 1)
        {
            speedReturnCoroutine = StartCoroutine(DoSpeedReduction());
        }
                
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        input.enabled = false;
        hud.GameOver();
    }

    void ShootPositionReached()
    {
        weapon.ShootWeapon();
    }

    
    public void SpeedPickup()
    {
        hud.UpdateChevron();
    }
}
