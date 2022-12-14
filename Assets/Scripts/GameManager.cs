using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Drawing;

public enum GameState
{
 GS_PAUSEMENU,
 GS_GAME,
 GS_LEVELCOMPLETED,
 GS_GAME_OVER
}

public class GameManager : MonoBehaviour
{	
	public GameState currentGameState = GameState.GS_PAUSEMENU;
	public static GameManager instance;
	public Canvas inGameCanvas;
	public Canvas pauseMenuCanvas;
	public Canvas levelCompletedCanvas;
	public Canvas gameOverCanvas;
	
	public Text coinsText;
	private int coins = 0;
	public Image[] keysTab;
	private int keys;	
	public Image[] livesTab;
	private int lives = 3;
	private string timerStr;
	private float timer;
	public Text timeText;
	public Text enemies1killedText;
	public Text enemies2killedText;
	private int enemies1killed = 0;
	private int enemies2killed = 0;
	public int maxKeyNumber = 3;
	public bool doorOpen = false;
	public Text score_UIText;
	public Text highscoreText;
	private int maxSecsToHighscore = 180;
	
    // Start is called before the first frame update
    void Start()
    {
		InGame();
    }

    // Update is called once per frame
    void Update()
    {
        if ( currentGameState == GameState.GS_PAUSEMENU )
		{
			if (Input.GetKeyDown( KeyCode.Escape ))
				Invoke("InGame", 0.1f);
		}
		else if ( currentGameState == GameState.GS_GAME )
		{ 
			timer += Time.deltaTime;
			float minutes = timer / 60;
			float seconds = timer % 60;
			timerStr = string.Format("{0:00}:{1:00}", minutes, seconds);
			timeText.text = timerStr;
			if ( Input.GetKey( KeyCode.Escape ) )
				Invoke("PauseMenu", 0.1f);
		}
		
		if (LevelGenerator.instance != null)
			if (timer > LevelGenerator.instance.maxGameTime && LevelGenerator.instance.shouldFinish == false)
				LevelGenerator.instance.Finish();

    }
	
	void Awake()
	{
		if ( !PlayerPrefs.HasKey("HighscoreLevel1" ) )
			PlayerPrefs.SetInt("HighscoreLevel1", 0);
		if ( !PlayerPrefs.HasKey("HighscoreLevel2" ) )
			PlayerPrefs.SetInt("HighscoreLevel2", 0);

		instance = this;
		coinsText.text = coins.ToString();
		if (keysTab[0] != null)
			for (int i = 0; i < keysTab.Length; i++)
				keysTab[i].color = UnityEngine.Color.grey;
		for (int i = 0; i < livesTab.Length; i++)
		{
			if (i < lives)
				livesTab [i].enabled = true;
			else
				livesTab [i].enabled = false;
		}
		timer = 0.0f;
	}
	
	void SetGameState ( GameState newGameState )
	{
		currentGameState = newGameState;
		if ( newGameState == GameState.GS_LEVELCOMPLETED )
		{
			Scene currentScene = SceneManager.GetActiveScene();
			if ( currentScene.name == "Poziom1" )
			{
				int score = lives * 20 + coins * 10 + enemies1killed * 50 + enemies2killed * 50 + (maxSecsToHighscore - (int)timer) * 20;
				if ( PlayerPrefs.GetInt ( "HighscoreLevel1" ) < score )
					PlayerPrefs.SetInt ("HighscoreLevel1", score);
				highscoreText.text = "Highscore: " + PlayerPrefs.GetInt( "HighscoreLevel1" );
				score_UIText.text = "score: " + score;
			}
			else if ( currentScene.name == "Poziom2" )
			{
				int score = lives * 20 + coins * 10 + enemies1killed * 50;
				if ( PlayerPrefs.GetInt( "HighscoreLevel2" ) < score )
					PlayerPrefs.SetInt( "HighscoreLevel2", score );
				highscoreText.text = "Highscore: " + PlayerPrefs.GetInt( "HighscoreLevel2" );
				score_UIText.text = "score: " + score;
			}
		}
		inGameCanvas.enabled = (currentGameState == GameState.GS_GAME);
		pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
		levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
		gameOverCanvas.enabled = (currentGameState == GameState.GS_GAME_OVER);		
		
	}	
	
	public void OnResumeButtonClicked()
	{
		InGame ();
	}
	public void OnRestartButtonClicked()
	{
		SceneManager.LoadScene( SceneManager.GetActiveScene().name );
	}
	public void OnNextLevelButtonClicked()
	{
		SceneManager.LoadScene( "Poziom2" );
	}
	public void OnExitButtonClicked()
	{
		SceneManager.LoadScene ( "MainMenu" );
	}
	
	
	
	public void AddKeys(string name)
	{
		if (name == "Key1")
			keysTab [ 0 ].color = UnityEngine.Color.white;
		else if (name == "Key2")
			keysTab [ 1 ].color = UnityEngine.Color.green;
		else
			keysTab [ 2 ].color = UnityEngine.Color.magenta;
		keys++;
		if ( GetKeysNr() >= maxKeyNumber )
			doorOpen = true;		
	}
	
	public int GetKeysNr()
	{
		return keys;
	}
	
	public void AddCoins()
	{
		coins++;
		coinsText.text = coins.ToString();
	}
	
	public void AddEnemies1()
	{
		enemies1killed++;
		enemies1killedText.text = enemies1killed.ToString();
	}
	
	public void AddEnemies2()
	{
		enemies2killed++;
		enemies2killedText.text = enemies2killed.ToString();
	}

	public void AddLife()
	{
		lives++;
		livesTab[lives-1].enabled = true;
	}
	
	public void RemoveLife()
	{
		lives--;
		livesTab[lives].enabled = false;
		Debug.Log(lives);
	}
	
	public int GetLives()
	{
		return lives;
	}
	
	public void InGame()
	{
		SetGameState( GameState.GS_GAME );
	}
	public void GameOver()
	{
		SetGameState( GameState.GS_GAME_OVER );
	}
	public void PauseMenu()
	{
		SetGameState( GameState.GS_PAUSEMENU );
	}
	public void LevelCompleted()
	{
		SetGameState( GameState.GS_LEVELCOMPLETED );
	}

}
