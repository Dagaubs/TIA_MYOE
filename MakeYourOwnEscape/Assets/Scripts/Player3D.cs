using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller3D))]
public class Player3D : MonoBehaviour {

	private Controller3D controller;

	private Vector3 velocity;

	private Animator animator;

	[SerializeField]
	private float maxSpeed = 18f;
	[SerializeField]
	private float VelocityAcceleration = 8f, rotationAcceleration = 3f;

	[SerializeField]
	private float rotateSpeed = 0.1f;

	[SerializeField]
    private float gravity, maxJumpVelocity, minJumpVelocity;

	[SerializeField]
	private bool useKeyBoard = false;

	private Vector3 targetOrientation;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller3D>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		bool jumpPressed = Input.GetKeyDown(KeyCode.Space), jumpUnPressed = Input.GetKeyDown(KeyCode.Space);
		if(!useKeyBoard){
			float VerticalInput = Input.GetAxis("Vertical");
			float HorizontalInput = Input.GetAxis("Horizontal");
			//TargetOrientation = Orientation of Joystick
			//Lerp transform.Rotation to TargetOrientation

			// Speed += Acceleration * Float (threshhold joystick)

		}else{
			if(Input.GetKey(KeyCode.DownArrow)){
				velocity.z = velocity.z - VelocityAcceleration > -maxSpeed ? velocity.z - VelocityAcceleration : -maxSpeed;
			}
			if(Input.GetKey(KeyCode.UpArrow)){
				velocity.z = velocity.z + VelocityAcceleration < maxSpeed ? velocity.z + VelocityAcceleration : maxSpeed;
			}
			float targetOrientation_Y = transform.localEulerAngles.y;
			if(Input.GetKey(KeyCode.RightArrow)){
				//velocity.z = velocity.z + acceleration < maxSpeed ? velocity.z + acceleration : maxSpeed;
				
				targetOrientation_Y -= rotationAcceleration;
			}
			if(Input.GetKey(KeyCode.LeftArrow)){
				//velocity.z = velocity.z - acceleration > -maxSpeed ? velocity.z - acceleration : -maxSpeed;
				targetOrientation_Y += rotationAcceleration;
			}
			targetOrientation = new Vector3(transform.localEulerAngles.x,targetOrientation_Y, transform.localEulerAngles.z);
			
			if(jumpPressed && controller.collisions.below){
				animator.SetTrigger("Jump");
				velocity.y = maxJumpVelocity;
			}

			if(jumpUnPressed){
				if (velocity.y > minJumpVelocity)
                {
                    velocity.y = minJumpVelocity;
                }
			}

			//Debug.Log("Actual TargetOrientation : " + targetOrientation);
			//Quaternion RotationTo = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(targetOrientation.x, targetOrientation.y, targetOrientation.z), Time.deltaTime * rotateSpeed);
			//Debug.Log("Actual rotation : " + transform.localEulerAngles + " | to : " + RotationTo * Vector3.forward);
			transform.localRotation = Quaternion.Euler(targetOrientation.x, targetOrientation.y, targetOrientation.z);
			//targetOrientation = transform.localEulerAngles;
		}

		if(!controller.collisions.below) velocity.y += gravity * Time.deltaTime;
		else if(!jumpPressed) velocity.y = 0;
		
		animator.SetFloat("speed", velocity.z);
		controller.Move(velocity * Time.deltaTime);
 
		velocity = velocity / 3;
	}
}
