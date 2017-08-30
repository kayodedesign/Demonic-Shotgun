using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour {
	
	public GameObject bullet;
	public GameObject aim;
	public GameObject bulletHolder;
	
	int ammountOfBullets = 20;
	float bulletSpread = 0.25f;
	float bulletSpeed = 15f;
	float bulletSpeedVariation = 0.4f;
	
	void Update () 
	{
	}
	
	void Shoot()
	{
		for(int i=0; i < ammountOfBullets; i++)
		{
			GameObject obj = Instantiate(bullet) as GameObject;
			
			//Position
			obj.transform.parent = bulletHolder.transform;
			Vector3 pos = aim.transform.position;
			
			pos.x += Random.Range(-bulletSpread, bulletSpread);
			pos.y += Random.Range(-bulletSpread, bulletSpread);
				
			obj.transform.position = pos;
			
			//Add force
			Vector3 dir = obj.transform.position - this.transform.position;
			dir.Normalize();	
			
			Vector3 force = dir * bulletSpeed * (1 + Random.Range(-bulletSpeedVariation, bulletSpeedVariation));
			obj.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		}
	}
}
