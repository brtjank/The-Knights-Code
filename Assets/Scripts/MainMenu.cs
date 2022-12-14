using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
	public Text highscoreLevel1Text;
	public Text highscoreLevel2Text;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		highscoreLevel1Text.text = "Highscore: " + PlayerPrefs.GetInt( "HighscoreLevel1" );
		highscoreLevel2Text.text = "Highscore: " + PlayerPrefs.GetInt( "HighscoreLevel2" );
    }
	
	void Awake()
	{
		if ( !PlayerPrefs.HasKey("HighscoreLevel1" ) )
			PlayerPrefs.SetInt("HighscoreLevel1", 0);
		if ( !PlayerPrefs.HasKey("HighscoreLevel2" ) )
			PlayerPrefs.SetInt("HighscoreLevel2", 0);
	}
	
	private IEnumerator StartGame( string levelName )
	{
		yield return new WaitForSeconds( 0.1f );
		SceneManager.LoadScene( levelName );
	}

	public void onLevel1ButtonPressed()
	{
		StartCoroutine( StartGame( "Poziom1" ) );
	}
	
	public void onLevel2ButtonPressed()
	{
		StartCoroutine( StartGame( "Poziom2" ) );
	}
		
	public void onQuitButtonPressed()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}	
		
}
