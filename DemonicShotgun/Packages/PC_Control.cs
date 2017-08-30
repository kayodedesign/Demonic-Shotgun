using UnityEngine;
using System.Collections;

public class PC_Control : MonoBehaviour {
	
	float horiMove;
	float vertMove;
	float horiRot;
	public float forceMultiplier = 5;
	public float rotationSpeed = 5;
	public bool mouseAim = true;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ControlInput();
	}
	
	void ControlInput() {
		//Movement
		horiMove = Input.GetAxis("Horizontal");
		vertMove = Input.GetAxis("Vertical");		
		rigidbody.velocity = new Vector3(horiMove*forceMultiplier,vertMove*forceMultiplier,0);
		
		
		//Rotation
		Vector3 aim = this.transform.position + new Vector3(Input.GetAxis ("RightHorizontal"), -Input.GetAxis("RightVertical"), 0);		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(mouseAim){
			aim = ray.origin;
			aim.z = 0;
		}
		transform.LookAt(aim);
	}
}
