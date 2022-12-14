using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
	public float moveSpeed = 2.5f;
	private bool left;
	private float jump_limit;
	private float jump;	
	
    // Start is called before the first frame update
    void Start()
    {
		left = true;
		jump_limit = 0;        
    }

    // Update is called once per frame
    void Update()
    {
		if (jump_limit > 2.6f || jump_limit < -2.6f)
		{
			if (left)
				left = false;
			else 
				left = true;
			jump_limit = 0;
		}
		else
		{
			jump = moveSpeed * Time.deltaTime;
			if (!left)
				transform.Translate(jump, 0.0f, 0.0f, Space.World);
			else 
				transform.Translate(-jump, 0.0f, 0.0f, Space.World);
			
			jump_limit += jump;
		}
    }
}
