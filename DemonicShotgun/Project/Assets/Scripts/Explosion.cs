using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour {
	
	float activeTimer;
	float activeDelay = 0.2f;
	
	List<GameObject> damaged;
		
	void Start () 
	{
		damaged = new List<GameObject>();
		activeTimer = activeDelay;
		
		CameraController.instance.CameraShake(0.15f, 0.15f, 0.4f);
	}
	
	void Update () 
	{
		activeTimer -= Time.deltaTime;
		
		if(activeTimer <= 0)
		{
			this.transform.localScale -= this.transform.localScale * Time.deltaTime * 4f;
			
			if(this.transform.localScale.x < 0.1f)
			{
				Destroy(this.gameObject);
			}
		}
	}
	
	void OnTriggerEnter(Collider c)
	{
		if(activeTimer > 0)
		{
			//Debris
			Debris d = c.gameObject.GetComponent<Debris>();
			if(d && !damaged.Contains(d.gameObject))
			{
				d.Hit(7);
				damaged.Add(d.gameObject);
			}
			
			if(c.GetComponent<Collider>().attachedRigidbody)
			{
				//Player
				Player p = c.GetComponent<Collider>().attachedRigidbody.GetComponent<Player>();
				if(p && !damaged.Contains(p.gameObject))
				{
					p.Hit(10); 
					damaged.Add(p.gameObject);
				}
			}
		}
	}
}
