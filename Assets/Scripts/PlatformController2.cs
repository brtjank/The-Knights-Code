using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController2 : MonoBehaviour
{
	public float moveSpeed = 2.5f;
	private bool up;
	private float jump_limit;
	private float jump;	
	
    // Start is called before the first frame update
    void Start()
    {
		up = true;
		jump_limit = 0;        
    }

    // Update is called once per frame
    void Update()
    {
		if (jump_limit > 3.0f || jump_limit < -3.0f)
		{
			if (up)
				up = false;
			else 
				up = true;
			jump_limit = 0;
		}
		else
		{
			jump = moveSpeed * Time.deltaTime;
			if (!up)
				transform.Translate(0.0f, jump, 0.0f, Space.World);
			else 
				transform.Translate(0.0f, -jump, 0.0f, Space.World);
			
			jump_limit += jump;
		}
    }
}
