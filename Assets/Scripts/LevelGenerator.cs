using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
	public static LevelGenerator instance;
	public LevelPieceBasic startPlatformPiece;
	public LevelPieceBasic endPlatformPiece;
	
	public int maxGameTime = 30;
	public bool shouldFinish;
	public Transform levelStartPoint;
	public List<LevelPieceBasic> levelPrefabs = new List<LevelPieceBasic>();
	public List<LevelPieceBasic> pieces = new List<LevelPieceBasic>();

    // Start is called before the first frame update
    void Start()
    {
		shouldFinish = false;
        instance = this;
		ShowPiece((LevelPieceBasic)Instantiate(startPlatformPiece));		
		ShowPiece((LevelPieceBasic)Instantiate(startPlatformPiece));
		ShowPiece((LevelPieceBasic)Instantiate(levelPrefabs[1]));
		ShowPiece((LevelPieceBasic)Instantiate(levelPrefabs[2]));
    }
	
	public void ShowPiece(LevelPieceBasic piece)
	{
		piece.transform.SetParent (this.transform, false);
		if (pieces.Count < 1)
			piece.transform.position = new Vector2 (
				levelStartPoint.position.x - piece.startPoint.localPosition.x,
				levelStartPoint.position.y - piece.startPoint.localPosition.y );
		else
			piece.transform.position = new Vector2(
				pieces [pieces.Count - 1].exitPoint.position.x - pieces [pieces.Count-1].startPoint.localPosition.x,
				pieces [pieces.Count - 1].exitPoint.position.y - pieces [pieces.Count-1].startPoint.localPosition.y);
		pieces.Add( piece );
    }
	
	public void AddPiece()
	{	
		int randomIndex = Random.Range(1, levelPrefabs.Count - 1);				
		LevelPieceBasic piece = (LevelPieceBasic)Instantiate(levelPrefabs[randomIndex]);
		ShowPiece(piece);
	}
	
	public void Finish()
	{
		shouldFinish = true;
		ShowPiece((LevelPieceBasic)Instantiate(endPlatformPiece));
	}
	
	public void RemoveOldestPiece()
	{
		if (pieces.Count > 1)
		{
			LevelPieceBasic oldestPiece = pieces [0];
			pieces.RemoveAt (0);
			Destroy (oldestPiece.gameObject);
		}
	} 
}
