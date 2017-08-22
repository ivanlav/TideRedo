using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerScript : MonoBehaviour {

    public float speed = 0;
    public Boundary boundary;
    public float moveForce;
    public float maxSpeedX;
    public float maxSpeedY;
    public float swimForce;

    private bool isSurfing;

    private Animator animator;

    public bool facingRight = true;
    public float digRaydistance;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        GameObject background = GameObject.FindGameObjectWithTag("Background");

        boundary = new Boundary();

        boundary.xMin = background.GetComponent<SpriteRenderer>().bounds.min.x + 1;
        Debug.Log(boundary.xMin);
        boundary.xMax = background.GetComponent<SpriteRenderer>().bounds.max.x - 1;
        Debug.Log(boundary.xMax);
        boundary.yMin = background.GetComponent<SpriteRenderer>().bounds.min.y;
        Debug.Log(boundary.yMin);
        boundary.yMax = background.GetComponent<SpriteRenderer>().bounds.max.y;
        Debug.Log(boundary.yMax);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        //If colliding with Sand Above, allow wave to pass through
        if (gameObject.GetComponent<Rigidbody2D>().velocity.y >= 0 && collision.gameObject.transform.position.y > gameObject.transform.position.y)
        {
            GameObject wave = GameObject.FindGameObjectWithTag("Wave");
            wave.GetComponent<EdgeCollider2D>().isTrigger = true;
        }

        //If colliding with Wave, can swim under it
        if (collision.gameObject.tag == "Wave" && Input.GetAxis("Vertical") < 0)
        {
            collision.gameObject.GetComponent<EdgeCollider2D>().isTrigger = true;
        }

        if(collision.gameObject.tag == "Starfish")
        {
            collision.collider.isTrigger = true;
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("starup");
        }


    }
    

    private void FixedUpdate()
    {

        MoveHorizontal(Input.GetAxis("Horizontal"));

        WaveCheck(Input.GetAxis("Vertical"));

        //RayCast to Dig
        DigRay();

        //Clamp position to Boundary
        GetComponent<Rigidbody2D>().position = new Vector2
        (Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax), GetComponent<Rigidbody2D>().position.y);
    }

    private void Dig(GameObject digObject)
    {
        digObject.tag = "Dirt";
        digObject.GetComponent<TileScript>().SetSprite();

        digObject.GetComponent<BoxCollider2D>().enabled = false;

        float scale = 1.1f;
        digObject.GetComponent<TileScript>().ScaleSprite(scale);
    }

    private void DigRay()
    {

        int layer = LayerMask.NameToLayer("Tile");

        int layermask = 1 << layer;

        if (Input.GetButton("Fire2"))
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, 0), Mathf.Infinity, layermask);

            float distance = 1000;

            //fire BoxCast based on direction 
            Vector2 boxSize = new Vector2(1.7f, 1.7f);

            if (Input.GetAxis("Horizontal") > 0)
            {
                hit = Physics2D.BoxCast(transform.position, boxSize, 0.0f, Vector2.right, digRaydistance, layermask);
                distance = Mathf.Abs(hit.point.x - transform.position.x);
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                hit = Physics2D.BoxCast(transform.position, boxSize, 0.0f, Vector2.left, digRaydistance, layermask);
                distance = Mathf.Abs(hit.point.x - transform.position.x);
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                hit = Physics2D.BoxCast(transform.position, boxSize, 0.0f, Vector2.up, digRaydistance, layermask);
                distance = Mathf.Abs(hit.point.y - transform.position.y);
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                hit = Physics2D.BoxCast(transform.position, boxSize, 0.0f, Vector2.down, digRaydistance, layermask);
                distance = Mathf.Abs(hit.point.y - transform.position.y);
            }

            

            //If BoxCast hits a Sand Tile, Dig
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject);

                if (hit.collider.gameObject.CompareTag("Sand"))
                {
                    //Debug.Log(distance);
                    //Debug.Log(digRaydistance);

                    if (distance < digRaydistance)
                    {
                        Dig(hit.collider.gameObject);
                    }

                }
            }
        }


    }

    private void MoveHorizontal(float moveHorizontal)
    {
        //Movement code, Clamp to max/min speeds
        if (moveHorizontal * GetComponent<Rigidbody2D>().velocity.x < maxSpeedX)
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveHorizontal * moveForce, ForceMode2D.Impulse);

        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX)
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);

        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeedY)
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, Mathf.Sign(GetComponent<Rigidbody2D>().velocity.y) * maxSpeedY);

        //Flip 
        if (moveHorizontal > 0 && !facingRight)
            Flip();
        else if (moveHorizontal < 0 && facingRight)
            Flip();

        //Animate, Set animation speed equal to current speed  
        float horizVel = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x);
        if (horizVel> .1)
        {
            GameObject wave = GameObject.FindGameObjectWithTag("Wave");

            if (!GetComponent<CircleCollider2D>().IsTouching(wave.GetComponent<EdgeCollider2D>()))
            {
                animator.SetTrigger("isMoving");
                animator.speed = horizVel / maxSpeedX;
            }
            
            
        }



    }

    private void WaveCheck(float moveVertical)
    {
        GameObject wave = GameObject.FindGameObjectWithTag("Wave");

        //Swim if under wave and pressing up
        if(moveVertical > 0)
        {
            if (wave.GetComponent<EdgeCollider2D>().bounds.center.y > gameObject.transform.position.y)

            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, swimForce), ForceMode2D.Impulse);
            }
        }
        
        //Recreate Wave Collider when above wave
        if(IsAboveClear() && moveVertical >= 0.0f && wave.GetComponent<Rigidbody2D>().velocity.y >= 0 && wave.GetComponent<EdgeCollider2D>().bounds.center.y < GetComponent<CircleCollider2D>().bounds.min.y)
        {
            wave.GetComponent<EdgeCollider2D>().isTrigger = false;
        }
    }

    private bool IsAboveClear()
    {
        //Returns true if no object above player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, 1f));
        if(hit.collider == null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        GetComponent<SpriteRenderer>().transform.localScale = theScale;
    }
}


