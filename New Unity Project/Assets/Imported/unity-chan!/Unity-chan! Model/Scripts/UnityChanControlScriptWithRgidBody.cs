//
// Controller with Rigidbody when Mecanim animation data does not move at origin
// sample
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
	// list of required components
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]

	public class UnityChanControlScriptWithRgidBody : MonoBehaviour
	{

		public float animSpeed = 1.5f;              // Animation playback speed setting
		public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
		public bool useCurves = true;               // Use or set curve adjustment in Mecanim
													// If this switch is not turned on, the curve will not be used
		public float useCurvesHeight = 0.5f;        // Effective height of curve correction (increase when it is easy to slip through the ground)

		// The following parameters for the character controller
		// front speed
		public float forwardSpeed = 7.0f;
		// Back speed
		public float backwardSpeed = 2.0f;
		// rotation speed
		public float rotateSpeed = 2.0f;
		// Jump power
		public float jumpPower = 3.0f;
		//Reference of character controller (capsule collider)
		private CapsuleCollider col;
		private Rigidbody rb;
		// Amount of movement of the character controller(capsule collider)
		private Vector3 velocity;
		// Variable that stores the initial values ​​of Collider Height and Center set in CapsuleCollider
		private float orgColHight;
		private Vector3 orgVectColCenter;
		private Animator anim;                          // Reference to the animator attached to the character
		private AnimatorStateInfo currentBaseState;         // A reference to the animator's current state used in the base layer

		private GameObject cameraObject;    // Reference to main camera

		// Animator Reference to each state
		static int idleState = Animator.StringToHash("Base Layer.Idle");
		static int locoState = Animator.StringToHash("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash("Base Layer.Jump");
		static int restState = Animator.StringToHash("Base Layer.Rest");

		// initialize
		void Start()
		{
			// Get the Animator component
			anim = GetComponent<Animator>();
			// Get the CapsuleCollider component (capsule type collision)
			col = GetComponent<CapsuleCollider>();
			rb = GetComponent<Rigidbody>();
			// Get the main camera
			cameraObject = GameObject.FindWithTag("MainCamera");
			// Save the initial values ​​of Height and Center of CapsuleCollider component
			orgColHight = col.height;
			orgVectColCenter = col.center;
		}


		// Below, the main process. Since it is entwined with the rigid body, process it in Fixed Update..
		void FixedUpdate()
		{
			float h = Input.GetAxis("Horizontal");                  // Define the horizontal axis of the input device with h
			float v = Input.GetAxis("Vertical");                    // Define the vertical axis of the input device with v
			anim.SetFloat("Speed", v);                              // Pass v to the "Speed" parameter set on the Animator side
			anim.SetFloat("Direction", h);                          // Pass h to the "Direction" parameter set on the Animator side
			anim.speed = animSpeed;                                 // Set animSpeed ​​to the motion playback speed of Animator
			currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // Set the reference state variable to the current state of Base Layer (0)
			rb.useGravity = true; //Gravity is cut during the jump, so be affected by gravity otherwise



			// Below, the movement process of the character
			velocity = new Vector3(0, 0, v);        // Get the amount of movement in the Z-axis direction from the up and down key inputs
													// Convert to the direction of the character in local space
			velocity = transform.TransformDirection(velocity);
			//The following v thresholds are adjusted with the transition on the Mecanim side.
			if (v > 0.1)
			{
				velocity *= forwardSpeed;       // Multiply the movement speed
			}
			else if (v < -0.1)
			{
				velocity *= backwardSpeed;  // Multiply the movement speed
			}

			if (Input.GetButtonDown("Jump"))
			{   // After entering the space key

				// Animation states can only jump during Locomotion
				if (currentBaseState.nameHash == locoState)
				{
					//You can jump if you are not in a state transition
					if (!anim.IsInTransition(0))
					{
						rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
						anim.SetBool("Jump", true);     // Send animator a flag to switch to jump
					}
				}
			}


			// Move the character by pressing the up and down keys
			transform.localPosition += velocity * Time.fixedDeltaTime;

			// Swing the character on the Y axis by pressing the left and right keys
			transform.Rotate(0, h * rotateSpeed, 0);


			// Below, the processing in each state of Animator
			// During Locomotion
			// When the current base layer is locoState
			if (currentBaseState.nameHash == locoState)
			{
				//If you are adjusting the collider on a curve, reset it just in case
				if (useCurves)
				{
					resetCollider();
				}
			}
			// Processing during JUMP
			// When the current base layer is jumpState
			else if (currentBaseState.nameHash == jumpState)
			{
				cameraObject.SendMessage("setCameraPositionJumpView");  // Change to a jumping camera
																		// If the state is not in transition
				if (!anim.IsInTransition(0))
				{

					// Below, the process when adjusting the curve
					if (useCurves)
					{
						// Below is the curve Jump Height and Gravity Control attached to the JUMP00 animation.
						// JumpHeight: Jump height at JUMP00 (0 to 1)
						// GravityControl: 1 -> Jumping (gravity disabled), 0 -> Gravity enabled
						float jumpHeight = anim.GetFloat("JumpHeight");
						float gravityControl = anim.GetFloat("GravityControl");
						if (gravityControl > 0)
							rb.useGravity = false;  //Cut off the effects of gravity during jumps

						// Drop Raycast from the character's center
						Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
						RaycastHit hitInfo = new RaycastHit();
						// Adjust the height and center of the collider with the curve attached to the JUMP00 animation only when the height is above useCurvesHeight.
						if (Physics.Raycast(ray, out hitInfo))
						{
							if (hitInfo.distance > useCurvesHeight)
							{
								col.height = orgColHight - jumpHeight;          // Adjusted collider height
								float adjCenterY = orgVectColCenter.y + jumpHeight;
								col.center = new Vector3(0, adjCenterY, 0); // adjusted collider center
							}
							else
							{
								// When it is lower than the threshold value, it returns to the initial value (just in case)				
								resetCollider();
							}
						}
					}
					// Reset the Jump bool value (do not loop)			
					anim.SetBool("Jump", false);
				}
			}
			// Processing during IDLE
			// When the current base layer is idleState
			else if (currentBaseState.nameHash == idleState)
			{
				//If you are adjusting the collider on a curve, reset it just in case
				if (useCurves)
				{
					resetCollider();
				}
				// Enter the space key and it will be in Rest state
				if (Input.GetButtonDown("Jump"))
				{
					anim.SetBool("Rest", true);
				}
			}
			// Processing during REST
			// When the current base layer is restState
			else if (currentBaseState.nameHash == restState)
			{
				// cameraObject.SendMessage("setCameraPositionFrontView");		// Switch the camera to the front
				// Reset the Rest bool value if the state is not transitioning (do not loop)
				if (!anim.IsInTransition(0))
				{
					anim.SetBool("Rest", false);
				}
			}
		}

		void OnGUI()
		{
			GUI.Box(new Rect(Screen.width - 260, 10, 250, 150), "Interaction");
			GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
			GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
			GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
			GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "Hit Space key while Stopping : Rest");
			GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
			GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
		}


		// Character collider size reset function
		void resetCollider()
		{
			// Returns the initial values ​​of component Height and Center
			col.height = orgColHight;
			col.center = orgVectColCenter;
		}
	}
}