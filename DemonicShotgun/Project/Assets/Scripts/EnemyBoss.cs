using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBoss : Enemy 
{
	
	List<Enemy> enemies;
	
	override public void Setup() 
	{
		state = new State(BossWaitState);
		
		Enemy[] findEnemies = FindObjectsOfType(typeof(Enemy)) as Enemy[];
		
		enemies = new List<Enemy>();
		foreach(Enemy e in findEnemies)
		{
			enemies.Add(e);
		}
	}
		
	void BossWaitState()
	{
		int enemyCount = enemies.FindAll(x => !x.dead).Count;
		
		if(enemyCount <= 1)
		{
			SetBossFrenzyState();
		}
	}
	
	int bossBulletAmount = 10;
	int bossBulletCounter;
	
	float bossWalkDelay = 4f;
	float bossWalkTimer;
	
	void SetBossFrenzyState()
	{
		bossBulletCounter = bossBulletAmount;
		bossWalkTimer = bossWalkDelay;
		
		Music.instance.PlayBossMusic();
		
		state = new State(BossFrenzyState);
	}
	
	void BossFrenzyState()
	{
		if(!player) return;
		
		this.transform.LookAt(player.transform.position, Vector3.forward);
		
		if(bossWalkTimer > 0)
		{
			bossWalkTimer -= Time.deltaTime;
			
			this.GetComponent<Rigidbody>().velocity = this.transform.forward * moveSpeed;
		}
		else
		{
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			
			shootTimer -= Time.deltaTime;
			
			if(shootTimer <= 0)
			{
				bossBulletCounter--;
				
				Vector3 pos = aim.transform.position;
				pos += aim.transform.right * Random.Range(-0.2f, 0.2f);
				GameObject newObj = Instantiate(bulletPrefab, pos, aim.transform.rotation) as GameObject;
				newObj.transform.parent = bulletHolder.transform;	
				newObj.GetComponent<Rigidbody>().velocity = newObj.transform.forward * 7f;
				
				CameraController.instance.CameraShake(0.1f, 0.1f, 0.25f);
				if(audioShoot) audioPlayer.PlayOneShot(audioShoot);
				
				if(bossBulletCounter <= 0)
				{
					bossWalkTimer = bossWalkDelay;
					bossBulletCounter = bossBulletAmount;
				}
				else
				{
					shootTimer = shootDelay * Random.Range(0.7f, 1.3f);
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider c)
	{
		//Debris
		Debris d = c.gameObject.GetComponent<Debris>();
		if(d) d.Hit(100);
		
		if(c.GetComponent<Collider>().attachedRigidbody)
		{
			//Player
			Player p = c.GetComponent<Collider>().attachedRigidbody.GetComponent<Player>();
			if(p) p.Hit(100); 
		}
	}
}