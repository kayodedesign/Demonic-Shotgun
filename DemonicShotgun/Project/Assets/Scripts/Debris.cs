using UnityEngine;
using System.Collections;

public class Debris : MonoBehaviour 
{
	public bool indestructible = false;
	public GameObject particles;
	public float health = 1;
	
	bool destroyed;
	
	public void Hit(int damage = 1)
	{
		if(!indestructible)
		{
			if(destroyed) return;
			
			health -= damage;
			
			if(health <= 0)
			{
				destroyed = true;
				
				if(particles != null)
				{
					GameObject p = Instantiate(particles) as GameObject;
					p.transform.position = this.transform.position;
				}
				
				Destroy(this.gameObject);
			}		
		}
	}
}
