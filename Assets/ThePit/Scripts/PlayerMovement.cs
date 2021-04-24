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
	[SerializeField] private Rigidbody rb;

	private void Awake()
	{
		if (playerInput == null)
			playerInput = GetComponent<PlayerInput>();
		if (rb == null)
			rb = GetComponent<Rigidbody>();
		if (playerCamera == null)
			playerCamera = GetComponentInChildren<Camera>();
	}

	void Update()
    {

		Vector3 direction = calculateMovementDirection(playerInput, playerCamera);
		if (direction != Vector3.zero)
		{
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, angularSpeed, 0.0f);

			transform.rotation = Quaternion.LookRotation(newDirection);

			Vector3 movement = transform.forward * acceleration;

			Vector3 nextXZVelocity = new Vector3(
				movement.x + rb.velocity.x,
				0,
				movement.z + rb.velocity.z);

			if (nextXZVelocity.magnitude < maxXZVelocity)
				rb.velocity += movement;
		}

		if (playerInput.jumpInputDown)
		{
			rb.AddForce(Vector3.up * jumpForce);
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
