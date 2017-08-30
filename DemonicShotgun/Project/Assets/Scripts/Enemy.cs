using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public enum EnemyType{Patrol, Static}
	public EnemyType type = EnemyType.Static;
	
	protected delegate void State();
	protected State state;
	
	public GameObject particles;
	public Transform aim;
	public GameObject bulletPrefab;
	public GameObject sight;
	
	public GameObject[] nodes;
	public float nodeRange = 1f;
	protected int currNode = 0;	
	
	public float sightDistance = 10f;
	public float moveSpeed = 10f;
	public float health = 3f;
	
	protected float shootTimer;
	public float shootDelay = 0.05f;
	
	public AudioClip audioShoot;
	
	[HideInInspector]
	public bool dead;
	
	protected Player player;
	protected GameObject bulletHolder;
	protected AudioSource audioPlayer;
	
	void Start () 
	{
		if(audioShoot) audioPlayer = this.gameObject.AddComponent<AudioSource>();
		
		player = FindObjectOfType(typeof(Player)) as Player;
		bulletHolder = new GameObject(this.gameObject.name+"BulletHolder");
		
		Setup();	
	}
	
	public virtual void Setup()
	{
		if(type == EnemyType.Patrol)
		{
			state = new State(PatrolState);
		}		
		else
		{
			SetStaticState();
		}
	}
	
	void Update ()
	{
		if(state != null)
		{
			state();
		}	
	}
	
	public void Hit()
	{
		if(dead) return;
		
		health--;		
		if(health <= 0) dead = true;
		
		if(!player) player = FindObjectOfType(typeof(Player)) as Player; //***Find this bug
		if(player) player.EnemeyShot(this);
		
		if(dead)
		{
			if(particles != null)
			{
				GameObject p = Instantiate(particles) as GameObject;
				p.transform.position = this.transform.position;
				
				p.transform.Rotate(Vector3.forward, Random.Range(0, 180));
			}
			
			Destroy(this.gameObject);
		}
	}
	
	void PatrolState()
	{	
		float dist = Vector3.Distance(transform.position, nodes[currNode].transform.position);
				
		Vector3 targetLocation = nodes[currNode].transform.position -transform.position;
		targetLocation.Normalize();
		
		transform.LookAt(nodes[currNode].transform, Vector3.forward);
		
		GetComponent<Rigidbody>().velocity = targetLocation * moveSpeed * Time.deltaTime;
		
		if(dist < nodeRange)
		{
			if(currNode + 1 < nodes.Length)
			{
				currNode++;
			}
			else
			{
				currNode = 0;
			}
		}
		
		bool spotCheck = Spot();
		
		if(spotCheck)
		{
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			SetAttackState();
		}
	}
	
	bool Spot()
	{		
		if(player == null) return false;
		
		if(playerInSight)
		{
			RaycastHit hit;		
			Ray ray = new Ray(this.transform.position, player.transform.position - this.transform.position);
			
			if(Physics.Raycast(ray, out hit, 100f))
			{	
				if(hit.collider.attachedRigidbody && hit.collider.attachedRigidbody.GetComponent<Player>()) 
				{	
					return true;
				}
			}
		}
		
		return false;
	}
	
	bool playerInSight;
	
	void OnTriggerEnter(Collider c)
	{
		if(c.GetComponent<Collider>().attachedRigidbody && c.GetComponent<Collider>().attachedRigidbody.GetComponent<Player>())
		{
			playerInSight = true;
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if(c.GetComponent<Collider>().attachedRigidbody && c.GetComponent<Collider>().attachedRigidbody.GetComponent<Player>())
		{
			playerInSight = false;
		}
	}
	
	float staticWaveTimer;
	Vector3 staticStartRot;
	
	void SetStaticState()
	{
		staticStartRot = this.transform.rotation.eulerAngles;
		state = new State(StaticState);
	}
	
	void StaticState()
	{
		bool spotCheck = Spot();
		
		staticWaveTimer += Time.deltaTime * 1f;
		
		Vector3 r = staticStartRot;
		r.x += Mathf.Sin(staticWaveTimer) * 25f; 
		this.transform.rotation = Quaternion.Euler(r);
		
		if(spotCheck)
		{
			SetAttackState();
		}
	}
	
	void SetAttackState()
	{
		shootTimer = 0.2f;	
		
		player.BeginRampage();
		
		state = new State(AttackState);
	}
	
	void AttackState()
	{
		if(!player) return;
		
		this.transform.LookAt(player.transform.position, Vector3.forward);
		
		shootTimer -= Time.deltaTime;
		
		if(shootTimer <= 0)
		{
			GameObject newObj = Instantiate(bulletPrefab, aim.transform.position, aim.transform.rotation) as GameObject;
			newObj.transform.parent = bulletHolder.transform;	
			newObj.GetComponent<Rigidbody>().velocity = newObj.transform.forward * 6f;
			
			CameraController.instance.CameraShake(0.05f, 0.05f, 0.25f);
			
			if(audioShoot) audioPlayer.PlayOneShot(audioShoot);
			
			shootTimer = shootDelay * Random.Range(0.9f, 1.1f);
		}
	}
}
