using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
	public Vector2 movementInput { get; private set; }
	public bool jumpInputDown { get; private set; }
	public bool mineInputDown { get; private set; }

	/// <summary>
	/// Used to check network ownership
	/// </summary>
	private RealtimeView _realtimeView;
	/// <summary>
	/// Owning this transform over the network
	/// </summary>
	private RealtimeTransform _realtimeTransform;


	private void Awake()
	{
		// Get references
		_realtimeView = GetComponent<RealtimeView>();
		_realtimeTransform = GetComponent<RealtimeTransform>();
	}


	private void Update()
	{
		if (_realtimeView != null)
		{
			// If this Player prefab is not owned by this client, bail.
			if (_realtimeView.realtime != null && !_realtimeView.isOwnedLocallySelf)
				return;
		}

		if (_realtimeTransform != null)
			// Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
			_realtimeTransform.RequestOwnership();

		// Check if the mouse was clicked over a UI element
		if (EventSystem.current.IsPointerOverGameObject())
		{
			Debug.Log("Clicked on the UI");
		}
		else
		{
			// Do all input stuff
			movementInput = new Vector2(
				Input.GetAxis("Horizontal"),
				Input.GetAxis("Vertical"));
			jumpInputDown = Input.GetButtonDown("Jump");
			mineInputDown = Input.GetButton("Fire1");
		}
	}
}
