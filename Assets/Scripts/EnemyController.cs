using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{	
	public float moveSpeed = 2f;
	public float XMin;
	public float XMax;
	private bool isFacingRight = true;
	private bool isMovingRight = true;
	public Animator animator;
	private Rigidbody2D rigidBody;
	private float startPositionX;
	private float killOffset = 0.6f;	
	
    // Start is called before the first frame update
    void Start()
    {
		startPositionX = rigidBody.position.x;
    }

    // Update is called once per frame
    void Update()
    {
		if(isMovingRight)
		{
			if ( this.transform.position.x < startPositionX + XMax )
				MoveRight ();
			else
			{
				isMovingRight = false;
				MoveLeft();
				Flip();
			}
		}
		else
		{
			if (this.transform.position.x > startPositionX - XMin)
				MoveLeft ();
			else
			{
				isMovingRight = true;
				MoveRight ();
				Flip();
			}
		}				
    }
	
	void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	
	void MoveRight()
	{
		if ( rigidBody.velocity.x < moveSpeed )
		{
			rigidBody.velocity = new Vector2( moveSpeed * 0.5f, rigidBody.velocity.y );
			rigidBody.AddForce( Vector2.right * 0.6f, ForceMode2D.Impulse );
		}
	}

	void MoveLeft()
	{
		if ( -rigidBody.velocity.x < moveSpeed )
		{
			rigidBody.velocity = new Vector2( -moveSpeed, rigidBody.velocity.y );
			rigidBody.AddForce( Vector2.right * 0.6f, ForceMode2D.Impulse );
		}
	}
	
	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	private void OnTriggerEnter2D( Collider2D other )
	{
		if ( other.CompareTag( "Player" ) )
		{
			if ( other.gameObject.transform.position.y > this.transform.position.y + killOffset )
			{
				Debug.Log("Enemy is dead");
				animator.SetBool( "isDead", true );
				StartCoroutine( KillOnAnimationEnd() );
			}
		}
	}
	
	private IEnumerator KillOnAnimationEnd()
	{
		yield return new WaitForSeconds( 1.0f );
		this.gameObject.SetActive( false );
	}

}
