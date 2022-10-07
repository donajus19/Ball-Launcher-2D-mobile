using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball_Handler : MonoBehaviour
{
   
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float detachDelay;


    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSprintJoint;

    private Camera mainCamera;
    private bool isDragging;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        spawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBallRigidbody == null) {return;}

        if(!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if(isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            
            return;
        }
        // no longer acted on the physics system
        isDragging = true;
        currentBallRigidbody.isKinematic = true;
        
        
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        //when touching screen set rigidBody to be in that position
        currentBallRigidbody.position = worldPosition;
        
    }
    private void LaunchBall()
    {
        //make the ball react to physics
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;
        //launch ball
        Invoke(nameof(DetachBall), detachDelay);
        
    }
    private void DetachBall()
    {
        //wont try to pull the ball anymore
        //detach the ball
        currentBallSprintJoint.enabled = false;
        currentBallSprintJoint = null;

        //respawn new ball after some time
        Invoke(nameof(spawnNewBall), respawnDelay);
    }
    private void spawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSprintJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSprintJoint.connectedBody = pivot;
    }
}
