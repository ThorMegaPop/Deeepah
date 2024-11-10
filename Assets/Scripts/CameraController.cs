using UnityEngine;

namespace Controller
{
	public class CameraController: MonoBehaviour {
		public float Sensitivity =  5f; 
		public float Smoothing = 2f; 
		public float ControllerSensitivityHorizontal = 1f;
		public float ControllerSensitivityVertical = .5f; 
		public float ControllerSmoothing = 2f;  
		public float DeadZone = .2f;
		private Vector2 _mouseLook; 
		private Vector2 _smoothV; 
		private GameObject _playerObject;
		private PlayerController controller;

		// private bool _cantMove => controller.CantMove;  

		private Vector2 _viewPos; 


        private void Awake() 
        {
			controller = GetComponentInParent<PlayerController>();
        }

        // Use this for initialization
		private void Start () 
		{
			_playerObject = this.transform.parent.gameObject; 
		}
		
		// Update is called once per frame
		private void Update () 
		{
			if (controller.CantMove) return; 
			
			_viewPos = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y")); 

			_viewPos = Vector2.Scale (_viewPos, new Vector2 (Sensitivity * Smoothing, Sensitivity * Smoothing)); 
			_smoothV.x = Mathf.Lerp (_smoothV.x, _viewPos.x, 1 / Smoothing); 
			_smoothV.y = Mathf.Lerp (_smoothV.y, _viewPos.y, 1 / Smoothing); 

			_mouseLook += _smoothV; 
			_mouseLook.y = Mathf.Clamp (_mouseLook.y, -90, 90); 

			transform.localRotation = Quaternion.AngleAxis (-_mouseLook.y, Vector3.right); 
			_playerObject.transform.localRotation = Quaternion.AngleAxis (_mouseLook.x, _playerObject.transform.up);
		}
	}
}