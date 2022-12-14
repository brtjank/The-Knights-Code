using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D( Collider2D other )
	{ 
		if (other.CompareTag("Player") && LevelGenerator.instance.shouldFinish == false)
		{
			LevelGenerator.instance.AddPiece();
			LevelGenerator.instance.RemoveOldestPiece();
		}
	}
}
