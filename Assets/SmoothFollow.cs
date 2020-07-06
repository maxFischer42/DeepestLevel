using UnityEngine;
using System.Collections;
using Prime31;


public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private CharacterController2D _playerController;
	private Vector3 _smoothDampVelocity;
	
	
	void Awake()
	{
		transform = gameObject.transform;
		_playerController = target.GetComponent<CharacterController2D>();
	}
	
	
	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}
//float nearestTenth = (int)(floatValue/0.1f)*0.1f;
	public float v;

	void updateCameraPosition()
	{
		if( _playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			transform.position = new Vector3((int)(transform.position.x/v)*v,(int)(transform.position.y/v)*v, cameraOffset.z);
			return;
		}
		
		if( _playerController.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			transform.position = new Vector3((int)(transform.position.x/v)*v,(int)(transform.position.y/v)*v, cameraOffset.z);
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
			transform.position = new Vector3((int)(transform.position.x/v)*v,(int)(transform.position.y/v)*v, cameraOffset.z);
		}
	}
	
}