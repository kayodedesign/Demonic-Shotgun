using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {

	public GameObject explosionPrefab;
	
	void Start () 
	{
	
	}
	
	void OnCollisionEnter(Collision c)
	{
		Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
		
		Destroy(this.gameObject);
	}
}