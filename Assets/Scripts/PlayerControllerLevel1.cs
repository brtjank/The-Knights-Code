using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevel1 : MonoBehaviour
{
	public AudioClip coinSound;
	public float moveSpeed = 0.1f;
	public float jumpForce = 6f;
	public LayerMask groundLayer;
	private Rigidbody2D rigidBody;
	public Animator animator;
	private bool isWalking = false;
	private float killOffset = 0.6f;
	private Vector2 startPosition;
	
	private AudioSource source;
	private bool isFacingRight = true;
    private string finishTime;
	private Vector3 vectorRight = new Vector3(0.4f, 0.0f, 0.0f);
	private Vector3 vectorLeft = new Vector3(-0.4f, 0.0f, 0.0f);
	private float killedTimer = 1.0f;
	private bool ifKilled = false;
	
	// Start is called before the first frame update
    void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
		if ( GameManager.instance.currentGameState == GameState.GS_GAME )
		{
			if (this.transform.position.y < -22.0f)
			{
				Death();
				return;
			}
			
			isWalking = false;
			
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				if ( !isFacingRight )
					Flip();
				isWalking = true;
				transform.Translate(moveSpeed*Time.deltaTime,0.0f,0.0f,Space.World );
			}
			else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
			{
				if ( isFacingRight )
					Flip();
				transform.Translate(-moveSpeed*Time.deltaTime,0.0f,0.0f,Space.World );
				isWalking = true;
			}
			if ( Input.GetMouseButtonDown( 0 ) || Input.GetKeyDown( KeyCode.Space ) )
				Jump ();
			
			if(ifKilled == true)
				killedTimer += Time.deltaTime;
			
			animator.SetBool( "isGrounded", isGrounded() );
			animator.SetBool( "isWalking", isWalking );
		}
	}
	
	void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		startPosition = rigidBody.position;
		source = GetComponent<AudioSource>();
	}

	bool isGrounded()
	{
		return (Physics2D.Raycast(this.transform.position, Vector2.down, 0.6f, groundLayer.value) || 
				Physics2D.Raycast(this.transform.position + vectorRight, Vector2.down, 0.6f, groundLayer.value) ||
				Physics2D.Raycast(this.transform.position + vectorLeft, Vector2.down, 0.6f, groundLayer.value));
	}
	
	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Jump()
	{ 
		if ( isGrounded() )
		{
			rigidBody.AddForce( Vector2.up * jumpForce, ForceMode2D.Impulse );
		}
	}
	
	void Death()
	{
		GameManager.instance.RemoveLife();
		this.transform.position = startPosition;
		if ( GameManager.instance.GetLives() <= 0 )
			GameManager.instance.GameOver();
	}
	
	private void OnTriggerEnter2D( Collider2D other )
	{ 
		if ( other.CompareTag ( "FallLevel" ) )
		{
			GameManager.instance.GameOver();
		}
		if ( other.CompareTag ( "Coin" ) )
		{
			GameManager.instance.AddCoins();
			other.gameObject.SetActive ( false );
			source.PlayOneShot(coinSound, AudioListener.volume);
		}
		if ( other.CompareTag ( "finish" ) )
		{
			if (GameManager.instance.doorOpen == true)
				GameManager.instance.LevelCompleted();
			else
				Debug.Log( "Door closed! Collect all the keys first!");
		}
		if ( other.CompareTag( "Enemy2" ) || other.CompareTag( "Enemy" ))
		{
			if ( other.gameObject.transform.position.y + killOffset < this.transform.position.y )
			{
				ifKilled = true;
				killedTimer = 0.0f;
				Debug.Log( "Killed an enemy! ");
				if ( other.CompareTag( "Enemy" ))
					GameManager.instance.AddEnemies1();
				else
					GameManager.instance.AddEnemies2();
			}
			else
			{
				if (killedTimer >= 1.0f)
				{
					Death();
					killedTimer = 1.0f;
					ifKilled = false;
				}
			}
		}
		if ( other.CompareTag( "Key" ) )
		{
			GameManager.instance.AddKeys(other.gameObject.name);
			other.gameObject.SetActive(false);
		}
		if ( other.CompareTag( "Heart" ) )
		{
			GameManager.instance.AddLife();
			other.gameObject.SetActive(false);
		}
	}

	private void OnCollisionEnter2D( Collision2D other )
	{ 
		if ( other.gameObject.CompareTag ( "MovingPlatform" ) )
		{
			this.transform.parent = other.transform;
		}
	}
	
	private void OnCollisionExit2D( Collision2D other )
	{ 
		if ( other.gameObject.CompareTag ( "MovingPlatform" ) )
		{
			this.transform.parent = null;
		}
	}
}
