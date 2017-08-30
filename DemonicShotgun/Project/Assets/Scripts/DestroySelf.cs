using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {

    private float destroyTimer;

	// Use this for initialization
	void Start () {

        destroyTimer = 3;
	
	}
	
	// Update is called once per frame
	void Update () {

        
        destroyTimer -= Time.deltaTime;
        if (destroyTimer <= 0)
        {
            Destroy(this.gameObject);
        }
	
	}


}
