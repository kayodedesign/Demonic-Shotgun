using UnityEngine;
using System.Collections;

public class SetRotation : MonoBehaviour {

    public Vector3 setRotation;

	// Use this for initialization
	void Start () {

        transform.localEulerAngles = setRotation;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
