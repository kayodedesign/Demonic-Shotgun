using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{	
	void Start () 
	{
	}
	
	void OnCollisionEnter(Collision c)
	{
		//Debris
		Debris debris = c.collider.gameObject.GetComponent<Debris>() as Debris;
		if(debris != null) 	debris.Hit();
		
		
		if(c.collider.attachedRigidbody)
		{
			//Enemies
			Enemy enemy = c.collider.attachedRigidbody.GetComponent<Enemy>() as Enemy;
			if(enemy) enemy.Hit();
			
			//Player
			Player player = c.collider.attachedRigidbody.GetComponent<Player>() as Player;
			if(player) player.Hit(10);
		}
		
		Destroy(this.gameObject);
	}	
}
