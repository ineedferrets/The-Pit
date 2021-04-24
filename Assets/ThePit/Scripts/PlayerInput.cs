using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public Vector2 movementInput { get; private set; }
	public bool jumpInputDown { get; private set; }
	public bool mineInputDown { get; private set; }

	private void Update()
	{
		movementInput = new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"));
		jumpInputDown = Input.GetButtonDown("Jump");
		mineInputDown = Input.GetButton("Fire1");
	}
}
