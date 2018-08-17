using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerManager player;
    const float playerRotationOffset = 0f; //in degrees

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {
        Vector3 pos = gameObject.transform.position;

        float motionSpeed = Mathf.Max(Mathf.Abs(Input.GetAxis("Forward")), Mathf.Abs(Input.GetAxis("Horizontal")));
        player._animator.SetFloat("IdleToWalk", motionSpeed);

        //we take the worlds forward and right axis to drive forward and side movement
        Vector3 Velocity = Vector3.forward * Input.GetAxis("Forward") * player.speed;
        Velocity += Vector3.right * Input.GetAxis("Horizontal") * player.speed;    
                
        player._rigidBody.velocity = Velocity;
        
        //make top down camera follow player from above
        player._camera.transform.position = new Vector3(player.transform.position.x, player._camera.transform.position.y, player.transform.position.z);

        RotatePlayerToMousePos();
        CheckShooting();

    }

    void RotatePlayerToMousePos()
    {
        Vector3 mousePos = Input.mousePosition;

        //convert both player and mouse position to clip space
        Vector3 mouseViewportPos = player._camera.ScreenToViewportPoint(mousePos);
        Vector3 playerViewportPos = player._camera.WorldToViewportPoint(player.transform.position);

        Vector3 mouseRelativePos = mouseViewportPos - playerViewportPos;

        //Debug.Log("x: " + mouseRelativePos.x + " y: " + mouseRelativePos.y + "  player x: "+ playerViewportPos.x + " y: " + playerViewportPos.y);

        float rotationAngle = Mathf.Atan2(mouseRelativePos.x, mouseRelativePos.y);


        player.transform.rotation = Quaternion.Euler(0, (Mathf.Rad2Deg * rotationAngle) + playerRotationOffset, 0);
    
    }

    void CheckShooting()
    {
        if(Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.Space))
        {           
            //update the shoot vector
            Plane plane = new Plane(Vector3.up, player.transform.position);
            Ray ray = player._camera.ScreenPointToRay(Input.mousePosition);
            float dist;


            if (plane.Raycast(ray, out dist))
            {

                Vector3 mousePointerOnPlane = ray.GetPoint(dist) + player.playerHeightOffset;
                player.weapon.shootVector = (mousePointerOnPlane - player.playerCorePos).normalized;

#if UNITY_EDITOR
                // visual aid for the shoot ray
                Debug.DrawLine(player.playerCorePos, player.playerCorePos + (player.weapon.shootVector * 100), Color.red, 100);
#endif

            }

            player.ShootWeapon();

        }

    }

}
