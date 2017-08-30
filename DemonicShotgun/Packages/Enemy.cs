using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public bool patroller = false;
	
	public GameObject particles;
	Player player;
	
	public GameObject[] nodes;
	public int currNode = 0;
	public float nodeRange = 1;
	
	public float sightDistance = 10;
	public float moveSpeed = 10;
	
	bool dead;
	
	void Start () 
	{
		player = FindObjectOfType(typeof(Player)) as Player;
	}
	
	void Update ()
	{
		Spot();
		if(patroller) Patrol();
	}
	
	void OnCollisionEnter(Collision c)
	{
		if(dead) return;
		dead = true;
		
		player.EnemeyShot(this);
		
		if(particles != null)
		{
			GameObject p = Instantiate(particles) as GameObject;
			p.transform.position = this.transform.position;
			
			p.transform.RotateAround(Vector3.forward, Random.Range(0, 180));
		}
		
		Destroy(this.gameObject);
	}
	
	void Patrol()
	{
		float dist = Vector3.Distance(transform.position, nodes[currNode].transform.position);
				
		Vector3 targetLocation = nodes[currNode].transform.position -transform.position;
		targetLocation.Normalize();
		
		transform.LookAt(nodes[currNode].transform);
		
		rigidbody.velocity = targetLocation * moveSpeed * Time.deltaTime;
		
		if(dist < nodeRange){
			if(currNode + 1 < nodes.Length){
				currNode ++;
			} else {
				currNode = 0;
			}
		}
	}
	
	void Spot()
	{		
		RaycastHit hit;
		Vector3 dir = player.transform.position - transform.position;
		
		Ray ray = new Ray(transform.position, dir);
		
		if(Physics.Raycast(ray, out hit, sightDistance)){
			Debug.Log(hit.collider.gameObject.name);
			if(hit.collider.gameObject.tag == "Player"){
				Debug.Log ("PlayerShot");
			}
		}
		
		Debug.DrawLine(transform.position, player.transform.position, Color.red, sightDistance);
	}
}
