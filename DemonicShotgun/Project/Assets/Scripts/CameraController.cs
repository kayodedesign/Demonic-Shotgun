using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	float depth = 0;
	float followSpeed = 2f;
	
	public Transform target;
	public GameObject shakeHelper;
		
	float shakeTimer;
	float totalShakeTime;
	Vector3 shakeAmount;
	
	public static CameraController instance;
	
	void Start () 
	{
		Vector3 pos = target.transform.position;
		pos.z = depth;
		this.transform.position = pos;
		
		instance = this;
	}
	
	void Update () 
	{		
		//Camera shake
		if(shakeTimer > 0)
		{
			shakeTimer -= Time.deltaTime;
			float per = shakeTimer / totalShakeTime;
			Vector3 shakePos = new Vector3(Random.Range(-shakeAmount.x * per, shakeAmount.x * per), Random.Range(-shakeAmount.y * per, shakeAmount.y * per), 0);
			
			if(shakeTimer <= 0) shakePos = Vector3.zero;
			
			shakeHelper.transform.localPosition = shakePos;
		}
		
		//Camera follow	
		if(target != null)
		{
			Vector3	pos = this.transform.position;
			pos += (target.transform.position - this.transform.position) * Time.deltaTime * followSpeed;
			pos.z = depth;
			
			this.transform.position = pos;
		}
	}
	
	public void CameraShake(float x, float y, float time)
	{
		shakeAmount = new Vector3(x, y, 0);		
		totalShakeTime = time;
		shakeTimer = time;		
	}
}
