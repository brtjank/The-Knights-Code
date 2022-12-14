using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class ArrowController : MonoBehaviour
{
	private bool up;
	private int counter;
	
    // Start is called before the first frame update
    void Start()
    {
		up = true;
		counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == 40)
		{
			if (up == true)
			{
				transform.Translate(0.0f, 0.2f, 0.0f, Space.World);
				up = false;
			}
			else 
			{
				transform.Translate(0.0f, -0.2f, 0.0f, Space.World);
				up = true;
			}
			counter = 0;
		}
		else counter++;
    }
}
