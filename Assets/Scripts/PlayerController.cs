using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	
	private GameController _GameController;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
	private bool isAtack;

    public float speed, jumpForce;
    public bool isLookLeft;
	public GameObject hitBoxPrefab;

	public Transform mao;
	public Transform groundCheck;
	private bool isGroundCheck;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();

		_GameController = FindObjectOfType(typeof(GameController)) as GameController;
		_GameController.playerTransform = this.transform;
	}

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
		// Esquerda = -1
		// Direita = 1
		// Canto nenhum = 0

		if (isAtack && isGroundCheck) {
			h = 0;
		}

		if ( h > 0 && isLookLeft) {
			Flip();
		} else if (h < 0 && !isLookLeft) {
			Flip();
		}

		float speedY = playerRb.velocity.y;

		if (Input.GetButtonDown("Jump") && isGroundCheck) {
			playerRb.AddForce(new Vector2(0,jumpForce));
		}

		if (Input.GetButtonDown("Fire1") && !isAtack) {
			isAtack = true;
			playerAnimator.SetTrigger("atack");
		}

		playerRb.velocity = new Vector2(h * speed, speedY);

		playerAnimator.SetInteger("h",(int) h);
		playerAnimator.SetBool("isGrounded",isGroundCheck);
		playerAnimator.SetFloat("speedY",speedY);
		playerAnimator.SetBool("isAtack",isAtack);
    }

	void FixedUpdate() {
		isGroundCheck = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
	}

    void Flip() {
		isLookLeft = !isLookLeft;
		float x = transform.localScale.x * -1;
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

	void onEndAtack() {
		isAtack = false;
	}

	void hitBoxAtack() {
		GameObject hitBoxTemp = Instantiate(hitBoxPrefab,mao.position, transform.localRotation);
		Destroy(hitBoxTemp, 0.2f);
	}
}
