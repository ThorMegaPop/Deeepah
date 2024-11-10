using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour 
{
	public float WalkSpeed = 6f;
    public float RunSpeed = 11f; 
	public float JumpSpeed = 8f;
	public float Gravity = 20f;
	public float SlideSpeed = 12f;
	public float AntiBumperFactor = .75f; 
	public int AntiBunnyHopFactor = 1; 
	public bool LimitDiagonalSpeed = true; 
	public bool ToggleRun; 
	public bool SlideWhenOverSlopeLimit; 
	public bool SlideOnTaggedObjects; 
	public bool CantMove; 
	[SerializeField] private bool _airControl; 

	//Private vars
	private Vector3 _moveDirection = Vector3.zero; 
	private bool _grounded; 
	private float _speed; 
	//private float _slideLimit; 
	//private float _rayDistance; 
	private int _jumpTimer; 
	private Transform _myTransform; 
	private CharacterController _controller; 
	private RaycastHit _hit; 
	
	[SerializeField] private GameObject _camObject; 
	[SerializeField] private SkinnedMeshRenderer[] renders; 
	void Start () 
	{

		/*	foreach(var r in renders)
				r.enabled = false; 
		*/

		_camObject.SetActive(true); 

		_controller = GetComponent<CharacterController> ();	
		_myTransform = transform; 
		_speed = WalkSpeed; 
		//_rayDistance = _controller.height * .5f + _controller.radius; 
		//_slideLimit = _controller.slopeLimit - .1f; 
		_jumpTimer = 0; 
	}


	private void ToggleRunning()
	{
		if (Math.Abs(_speed - WalkSpeed) < .01f)
			_speed = RunSpeed;
		else 
			_speed = WalkSpeed; 
	}
	
	private void FixedUpdate () 
	{
		if (CantMove) return; 

		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		
		float inputModifyFactor = (inputX != 0f && inputY != 0f && LimitDiagonalSpeed)? .7071f : 1f;

		if (inputY == 0 && inputX == 0)
		{
			_speed = WalkSpeed;  
		}

		if (_grounded)
		{
			_moveDirection = new Vector3(inputX * inputModifyFactor, -AntiBumperFactor, inputY * inputModifyFactor);
			_moveDirection = _myTransform.TransformDirection(_moveDirection) * _speed;
			
			/*if (Input.GetKeyDown(KeyCode.Space))
			{
				Jump();
			}*/
		}
		if (!_grounded)
		{
			Vector3 temp = new Vector3(_moveDirection.x = inputX * inputModifyFactor, 0, _moveDirection.z = inputY * inputModifyFactor);
			//Vector3 temp = _myTransform.TransformDirection(_moveDirection) * _speed;

			_moveDirection.x = temp.x; 
			_moveDirection.z = temp.z; 
		}

		_moveDirection.y -= Gravity * Time.deltaTime; 
		_grounded = (_controller.Move(_moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
	}

	private void Jump()
	{
		if (!_grounded)
			return; 

		_moveDirection.y = JumpSpeed;
	}

	private void Update()
	{
		if (ToggleRun && _grounded && Input.GetKeyDown(KeyCode.LeftShift))
		{
			ToggleRunning();
		}

	}

	public void TeleportPlayer(Vector3 destination)
	{
		_controller.enabled = false; 
		this.transform.position = destination; 
		_controller.enabled = true; 
	}

}