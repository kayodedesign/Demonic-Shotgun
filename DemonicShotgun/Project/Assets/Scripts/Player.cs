using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	public float walkSpeed = 2f;
	public float rotationSpeed = 5;
	public bool mouseAim = true;
	
	public GameObject bullet;
	public GameObject shellPrefab;
	public GameObject aim;
	public GameObject holder;
	GameObject bulletHolder;
	
	public GameObject killParticles;
	public Transform shellEjector;
	
	int ammountOfBullets = 20;
	float bulletSpread = 0.25f;
	float bulletSpeed = 15f;
	float bulletSpeedVariation = 0.4f;
	
	//float blowBackAmmount = 4f;
	
	Bar shootBar;
	
	float demonicTimer;
	float totalDemonicTime = 12f;
	bool rampage = false;
	
	float reloadTimer;
	float reloadDelay = 1f;
	
	AudioSource[] audioSources;
	
	List<EnemyBoss> bosses;
	
	bool won;

    Rigidbody playerRigidbody;

	
	void Start () 
	{
        playerRigidbody = GetComponent<Rigidbody>();

		EnemyBoss[] findBosses = FindObjectsOfType(typeof(EnemyBoss)) as EnemyBoss[];
		
		bosses = new List<EnemyBoss>();
		
		foreach(EnemyBoss eb in findBosses)
		{
			bosses.Add(eb);		
		}
		
		audioSources = this.gameObject.GetComponents<AudioSource>();
		
		bulletHolder = new GameObject("BulletHolder");
		
		demonicTimer = totalDemonicTime;		

		shootBar = FindObjectOfType(typeof(Bar)) as Bar;
	}
	
	void Update () 
	{
		ControlInput();

		
		if(rampage && !won)
		{
           /* if ((playerRigidbody.velocity.x != 0) || (playerRigidbody.velocity.y != 0))
            {
                Rampage();
            }*/

            Rampage();
			
		}
	}
	
	void ControlInput() {
		
		if(Input.GetJoystickNames().Length > 0){
			mouseAim = false;
		} else {
			mouseAim = true;
		}
		
		//Movement
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
        playerRigidbody.velocity = new Vector3(h * walkSpeed, v * walkSpeed, 0);
		
		
		//Rotation
		Vector3 aim = Vector3.zero;
		
		if(mouseAim)
		{
			aim = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			aim.z = 0;
		}
		else
		{
			aim = this.transform.position + new Vector3(Input.GetAxis ("RightHorizontal"), -Input.GetAxis("RightVertical"), 0);		
		}
		
		transform.LookAt(aim, Vector3.forward);
		
		//Shoot
		if(Input.GetAxis("Fire1")!= 0)
		{
			if(reloadTimer <= 0)
			{
				Shoot();
			}
		}
		
		if(reloadTimer > 0)
		{
			reloadTimer -= Time.deltaTime;
			
			if(reloadTimer <= 0)
			{
				holder.GetComponent<Animation>().Play();
				GameObject shellObj = Instantiate(shellPrefab, shellEjector.transform.position, shellEjector.transform.rotation) as GameObject;
				shellObj.transform.parent = bulletHolder.transform;
				
				shellObj.GetComponent<Rigidbody>().AddForce(shellObj.transform.forward * 3f, ForceMode.Impulse);
				shellObj.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, 4f), ForceMode.Impulse);
				
				audioSources[1].Play();
			}
		}
	}
	
	void Shoot()
	{
		if(!rampage)
		{
			BeginRampage();
		}
		
		CameraController.instance.CameraShake(0.1f, 0.1f, 0.5f);
		
		audioSources[0].Play();
		
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
		
		reloadTimer = reloadDelay;
	}
	
	public void Hit(int amount)
	{
		demonicTimer -= (totalDemonicTime / 100) * amount;
	}
	
	public void BeginRampage()
	{
		if(!rampage)
		{
			rampage = true;
			Music.instance.PlayCalmMusic();
		}
	}
	
	void Rampage()
	{
		if(demonicTimer > 0)
		{
			demonicTimer -= Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.A)) 
            {
                demonicTimer += 1;
            }
		}
		
		//Update bar
		float per = demonicTimer / totalDemonicTime;
		per = Mathf.Clamp(per, 0, 1);		
		shootBar.SetBar(per);
		
		if(demonicTimer <= 0)
		{
			KillPlayer();
		}
	}
	
	public void EnemeyShot(Enemy enemy)
	{
		if(enemy.dead)
		{
			demonicTimer += totalDemonicTime / 3f;	
		}
		else if(enemy is EnemyBoss)
		{
			demonicTimer += (totalDemonicTime * 0.01f);	
		}
		
		demonicTimer = Mathf.Clamp(demonicTimer, 0, totalDemonicTime);
		
		WinCheck();
	}	
	
	void WinCheck()
	{
		int bossCount = bosses.FindAll(x => !x.dead).Count;
		
		if(bossCount == 0)
		{
			GameManager.Instance.SetWinState();
			
			won = true;
		}
	}
	
	public void KillPlayer()
	{
		GameObject p = Instantiate(killParticles) as GameObject;
		p.transform.position = this.transform.position;	
		
		GameManager.Instance.SetDeathState();
		
		CameraController.instance.CameraShake(0.3f, 0.3f, 1f);
		
		Bar.instance.Explode();
		
		Destroy(this.gameObject);
	}
}
