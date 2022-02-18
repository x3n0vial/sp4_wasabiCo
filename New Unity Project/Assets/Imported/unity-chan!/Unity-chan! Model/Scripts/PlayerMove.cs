using UnityEngine;
using System.Collections;

namespace UnityChan
{
	// list of required components
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]

	public class PlayerMove : MonoBehaviour
	{
		public float turnSpeed = 20f;

		public float animSpeed = 1.5f;              // Animation playback speed setting
		public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
		public bool useCurves = true;               // Use or set curve adjustment in Mecanim
													// If this switch is not turned on, the curve will not be used
		public float useCurvesHeight = 0.5f;        // Effective height of curve correction (increase when it is easy to slip through the ground)

		// The following parameters for the character controller
		// speed
		public float speed = 2.0f;
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

        private float restTimer = 0.0f;

		// Animator Reference to each state
		static int idleState = Animator.StringToHash("Base Layer.Idle");
		static int runState = Animator.StringToHash("Base Layer.Run");
		static int jumpState = Animator.StringToHash("Base Layer.Jump");
		static int restState = Animator.StringToHash("Base Layer.Rest");

		Quaternion rotation = Quaternion.identity;

		// initialize
		void Start()
		{
			// Get the Animator component
			anim = GetComponent<Animator>();

			// Get the CapsuleCollider component (capsule type collision)
			col = GetComponent<CapsuleCollider>();
			rb = GetComponent<Rigidbody>();

			// Save the initial values ​​of Height and Center of CapsuleCollider component
			orgColHight = col.height;
			orgVectColCenter = col.center;
		}


		// Below, the main process. Since it is entwined with the rigid body, process it in Fixed Update..
		void FixedUpdate()
		{
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");  

			// play animation when move
			bool hasHorizontalInput = !Mathf.Approximately(h, 0f);
			bool hasVerticalInput = !Mathf.Approximately(v, 0f);
			bool isRun = hasHorizontalInput || hasVerticalInput;
			anim.SetBool("IsRun", isRun);

			anim.speed = animSpeed;									// Set animation speed
			currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // Set the reference state variable to the current state of Base Layer (0)
			rb.useGravity = true; //Gravity is cut during the jump, so be affected by gravity otherwise

            velocity.Set(h, 0f, v);
            velocity = Camera.main.transform.TransformDirection(velocity);
            velocity.y = 0.0f;
            velocity.Normalize();

			Vector3 desiredForward = Vector3.RotateTowards(transform.forward, velocity, turnSpeed * Time.deltaTime, 0f);
			rotation = Quaternion.LookRotation(desiredForward);

            rb.MovePosition(rb.position + velocity * Time.deltaTime * speed);
            rb.MoveRotation(rotation);

            if (Input.GetKeyDown(KeyCode.Space))
            {   //You can jump if you are not in a state transition
                Debug.Log("space pressed");
                if (currentBaseState.nameHash == idleState || currentBaseState.nameHash == runState || currentBaseState.nameHash == restState)
                {
                    if (!anim.IsInTransition(0))
                    {
                        rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                        anim.SetBool("IsJump", true);     // Send animator a flag to switch to jump
                    }
                    else
                    {
                        Debug.Log("Anim transition got issue");
                    }
                }
                else {
                    Debug.Log("current base state got issue");// + currentBaseState.nameHash.ToString());
                }
            }

            //// Below, the processing in each state of Animator
            //// During Locomotion
            //// When the current base layer is runState
            if (currentBaseState.nameHash == runState)
            {
                restTimer = 0.0f;
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
                restTimer = 0.0f;
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
                    anim.SetBool("IsJump", false);
                }
            }
            // Processing during IDLE
            // When the current base layer is idleState
            else if (currentBaseState.nameHash == idleState)
            {
                restTimer += Time.deltaTime;
                //If you are adjusting the collider on a curve, reset it just in case
                if (useCurves)
                {
                    resetCollider();
                }
                // rest state every 5 sec
                if (restTimer >= 5.0f)
                {
                    restTimer = 0.0f;
                    anim.SetBool("IsRest", true);
                }
            }
            // Processing during REST
            // When the current base layer is restState
            else if (currentBaseState.nameHash == restState)
            {
                // Reset the Rest bool value if the state is not transitioning (do not loop)
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("IsRest", false);
                }
            }
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