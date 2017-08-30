using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	
	public delegate void GameState();
	public GameState gameState;
	
	string[] levels = {"Level01", "Level03", "Level02", "Level05", "Level06", "Level04"};
	
	public int currentLevel;
	
	GameObject winText;
	
	public static GameManager Instance
	{
		get
		{
			if (instance == null) instance = new GameObject("GameManager").AddComponent<GameManager> ();			
			
			DontDestroyOnLoad(instance);
			
			return instance;
		}
	}
 
	public void OnApplicationQuit ()
	{
		instance = null;
	}
	
	//Start menu
	public void SetStartMenuState()
	{
		gameState = new GameState(StartMenuState);
	}
	
	void StartMenuState()
	{
		if(Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			SetLevelState(levels[currentLevel]);
            
		}
	}
	
	//Playing
	void SetLevelState(string level)
	{
		SceneManager.LoadScene(level);
		gameState = new GameState(LevelState);
	}
	
	void LevelState()
	{
		//Cheat to next level
		if(Input.GetKeyDown(KeyCode.P))
		{
			PlayNextLevel();
		}
	}
	
	//Win
	public void SetWinState()
	{
		gameState = new GameState(WinState);
	}
	
	void WinState()
	{
		winText = GameObject.Find("CameraHolder/WIN");
		
		winText.GetComponent<Renderer>().enabled = true;
		
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			PlayNextLevel();	
		}
	}
	
	void PlayNextLevel()
	{
		if(currentLevel < levels.Length-1)
		{
			currentLevel++;
		}
		
		SetLevelState(levels[currentLevel]);
	}
	
	//Retry
	public void SetDeathState()
	{
        gameState = new GameState(DeathState);
	}
	
	void DeathState()
	{		
		winText = GameObject.Find("CameraHolder/DIE");
		
		winText.GetComponent<Renderer>().enabled = true;
		
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
            SceneManager.LoadScene(levels[currentLevel]);
			gameState = null;
		}
	}
	
	void Update () {
		
		if(gameState != null)
		{
			gameState();
		}		
	}
}