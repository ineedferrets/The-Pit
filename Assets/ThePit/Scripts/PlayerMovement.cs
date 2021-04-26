using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float acceleration;
	[SerializeField] private float maxXZVelocity;
	[SerializeField] private float angularSpeed;
	[SerializeField] private float jumpForce;

	[SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerInput playerInput;
	[SerializeField] private PickaxeBehaviour pickaxe;
	[SerializeField] private Rigidbody rb;

	private bool isJumping = false;

	public bool holdingTreasure = false;

	private void Awake()
	{
		if (playerInput == null)
			playerInput = GetComponent<PlayerInput>();
		if (rb == null)
			rb = GetComponent<Rigidbody>();
		if (playerCamera == null)
			playerCamera = GetComponentInChildren<Camera>();
		if (pickaxe == null)
			pickaxe = GetComponentInChildren<PickaxeBehaviour>();

		// Attempt again to get the camera from the scene if it didn't work at first from children
		if (playerCamera == null)
		{
			playerCamera = Camera.main;
            // If we finally found the camera...
            if (playerCamera != null)
            {
				// Assign this object's transform as the target to follow from the camera
				var cameraScript = playerCamera.GetComponent<CameraFollow>();
				if (cameraScript != null)
				{
					if (cameraScript.targetTransform == null)
						cameraScript.targetTransform = this.transform;
					
				}

				// also assign it to target cutout
				var cutoutScript = playerCamera.GetComponent<Cutout>();
				if (cutoutScript != null)
				{
					if (cutoutScript.targetObject == null)
						cutoutScript.targetObject = this.transform;

				}

			}
		}

		playerCamera.transform.parent = null;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			isJumping = false;
		}
	}
	void Update()
    {
		if (holdingTreasure)
        {
			Debug.Log("Holding it");
        }

		Vector3 direction = calculateMovementDirection(playerInput, playerCamera);

		if (direction != Vector3.zero)
		{
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, angularSpeed, 0.0f);

			transform.rotation = Quaternion.LookRotation(newDirection);

			Vector3 movement = transform.forward * acceleration;

			Debug.Log("Change in acceleration: " + transform.forward);

			Vector3 nextXZVelocity = new Vector3(
				movement.x + rb.velocity.x,
				0,
				movement.z + rb.velocity.z);

			if (nextXZVelocity.magnitude < maxXZVelocity)
			{
				rb.velocity += movement;
				Debug.Log("My new velocity: " + rb.velocity);
			}
		}

		if (playerInput.jumpInputDown && !isJumping)
		{
			rb.AddForce(Vector3.up * jumpForce);
			isJumping = true;
		}

		if (playerInput.mineInputDown)
        {
			pickaxe.Mine();
        }
		else
        {
			pickaxe.StopMining();
        }
    }

	private Vector3 calculateMovementDirection(PlayerInput input, Camera camera)
	{
		Vector3 direction = new Vector3(
			playerInput.movementInput.x,
			0,
			playerInput.movementInput.y);
		direction.Normalize();

		Vector3 cameraCorrectedDirection = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.x, Vector3.up) * direction;
		return cameraCorrectedDirection;
	}

}
