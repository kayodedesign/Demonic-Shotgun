using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour {
	
	public Transform bar;
	public Transform border;
	public GameObject particles;
	public GameObject holder;
	
	public float process = 1f;
	
	bool shown = false;
	
	public static Bar instance;
	
	void Start () 
	{
		instance = this;
		//bar.gameObject.SetActive(false);
		//border.gameObject.SetActive(false);	
		
		//Vector3 screenPos = new Vector3(Screen.height * 0.05f, Screen.height * 0.05f, 5);			
		Camera camera = Camera.main;		
		Vector3 pos = camera.ScreenToWorldPoint(new Vector3(0, 0, 5));		
		
		this.transform.position = pos;
		
		Vector3 width = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
		
		barWidth = width.x * ratio;
	}
	
	float ratio = 1.3f;
	float barWidth;
	
	void Update () 
	{
		bar.transform.localScale = new Vector3(barWidth * process, 1, 1);		
		border.transform.localScale = new Vector3(barWidth, 1, 1);
	}
	
	public void SetBar(float p)
	{
		process = p;
		
		if(!shown)
		{
			bar.gameObject.SetActive(true);
			border.gameObject.SetActive(true);
		}
	}
	
	public void Explode()
	{
		Vector3 pos = this.transform.position;
		pos.x += 3f;
		pos.y += 1.2f;
		GameObject obj = Instantiate(particles, pos, this.transform.rotation) as GameObject;
		obj.transform.parent = this.transform;
		
		holder.SetActive(false);
	}
}
