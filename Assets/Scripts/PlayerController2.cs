using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
	public AudioClip coinSound;
	public AudioClip fallSound;
	public AudioClip winSound;
	public AudioClip loseSound;
	public float moveSpeed = 7.0f;
	public float jumpForce = 6f;
	public LayerMask groundLayer;
	private Rigidbody2D rigidBody;
	public Animator animator;
	private bool isWalking = false;
	private float killOffset = 0.2f;
	private Vector2 startPosition;
	private int jumpCounter = 0;
	
	private Vector3 vectorRight = new Vector3(0.5f, 0.0f, 0.0f);
	private AudioSource source;
	private float killedTimer = 1.0f;
	private bool ifKilled = false;	
	private int ifFalledCounter = 52;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( GameManager.instance.currentGameState == GameState.GS_GAME )
		{
			isWalking = false;
			if ( rigidBody.velocity.x < moveSpeed )
				transform.Translate(moveSpeed*Time.deltaTime,0.0f,0.0f,Space.World );
			else if (rigidBody.velocity.x == 0)
				rigidBody.velocity = new Vector2( 0f, rigidBody.velocity.y );
				
			if ( Input.GetMouseButtonDown( 0 ) || Input.GetKeyDown( KeyCode.Space ))
				Jump();
			
			if ( isGrounded() )
			{
				jumpCounter = 0;				
				isWalking = true;
			}
			animator.SetBool( "isGrounded", isGrounded() );
			animator.SetBool( "isWalking", isWalking );
		}
		else
		{
			if ( rigidBody.velocity.x > 0.01f )
				rigidBody.velocity = new Vector2(
					0.95f * rigidBody.velocity.x, rigidBody.velocity.y );
			else
				if ( rigidBody.velocity.x < 0.01f )
					rigidBody.velocity = new Vector2(
						0.95f * rigidBody.velocity.x, rigidBody.velocity.y );
				else
					rigidBody.velocity = new Vector2( 0f, 0f );		
			animator.SetBool( "isGrounded", true );
			animator.SetBool( "isWalking", false );					
		}
		if(ifKilled == true)
			killedTimer += Time.deltaTime;	
		if (ifFalledCounter == 50)
		{
			transform.Translate(0.0f,10.0f,0.0f,Space.World );
			while (!(Physics2D.Raycast(this.transform.position, Vector2.down, 6.0f, groundLayer.value)
					&& Physics2D.Raycast(this.transform.position + vectorRight + vectorRight, Vector2.down, 6.0f, groundLayer.value)))
			{
				transform.Translate(-1.0f,0.0f,0.0f,Space.World );
			}
		}
		if (ifFalledCounter == 51)
			System.Threading.Thread.Sleep(1500);
		if (ifFalledCounter < 100)
			ifFalledCounter++;
		
    }
	
	void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		startPosition = rigidBody.position;
		source = GetComponent<AudioSource>();
	}

	bool isGrounded()
	{
		return (Physics2D.Raycast(this.transform.position, Vector2.down, 0.01f, groundLayer.value));
	}
	
	void Jump()
	{ 
		if ( isGrounded() || jumpCounter < 1 )
		{
			rigidBody.AddForce( Vector2.up * jumpForce, ForceMode2D.Impulse );
			jumpCounter++;
		}
	}	
	
	void Death()
	{
		GameManager.instance.RemoveLife();
		if ( GameManager.instance.GetLives() <= 0 )
			GameManager.instance.GameOver();
	}
	
	private void OnTriggerEnter2D( Collider2D other )
	{ 
		if ( other.CompareTag ( "FallLevel" ) && ifFalledCounter > 1)
		{			
			Death();
			source.PlayOneShot(fallSound, AudioListener.volume);
			ifFalledCounter = 0;
			
		}
		if ( other.CompareTag ( "Coin" ) )
		{
			GameManager.instance.AddCoins();
			other.gameObject.SetActive ( false );
			source.PlayOneShot(coinSound, AudioListener.volume);
		}
		if ( other.CompareTag ( "finish" ) )
		{
			GameManager.instance.LevelCompleted();
		}
		if (other.CompareTag( "Enemy" ))
		{
			if ( other.gameObject.transform.localPosition.y + killOffset < this.transform.localPosition.y )
			{
				ifKilled = true;
				killedTimer = 0.0f;
				Debug.Log( "Killed an enemy! ");
				GameManager.instance.AddEnemies1();
				source.PlayOneShot(winSound, AudioListener.volume);
			}
			else
			{
				if (killedTimer >= 1.0f)
				{
					Death();
					source.PlayOneShot(loseSound, AudioListener.volume);
					killedTimer = 1.0f;
					ifKilled = false;
				}
			}
		}
		if ( other.CompareTag( "Heart" ) )
		{
			GameManager.instance.AddLife();
			other.gameObject.SetActive(false);
		}
	}	
	
}
