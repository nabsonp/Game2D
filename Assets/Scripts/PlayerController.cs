using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	
	private GameController _GameController;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
	private bool isAtack;
	private SpriteRenderer playerSR;

    public float speed, jumpForce;
    public bool isLookLeft;
	public GameObject hitBoxPrefab;

	public Transform mao;
	public Transform groundCheck;
	private bool isGroundCheck;
	public Color hitColor, noHitColor;
	public int maxHP;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();

		_GameController = FindObjectOfType(typeof(GameController)) as GameController;
		_GameController.playerTransform = this.transform;

		playerSR = GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
		playerAnimator.SetBool("isGrounded",isGroundCheck);
		if (_GameController.currentState != GameController.gameState.GAMEPLAY) {
			playerRb.velocity = new Vector2(0,playerRb.velocity.y);
			playerAnimator.SetInteger("h",0);
			return;
		}
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
			_GameController.playSFX(_GameController.sfxJump,0.5f);
			playerRb.AddForce(new Vector2(0,jumpForce));
		}

		if (Input.GetButtonDown("Fire1") && !isAtack) {
			_GameController.playSFX(_GameController.sfxAtack,0.5f);
			isAtack = true;
			playerAnimator.SetTrigger("atack");
		}

		playerRb.velocity = new Vector2(h * speed, speedY);

		playerAnimator.SetInteger("h",(int) h);
		playerAnimator.SetFloat("speedY",speedY);
		playerAnimator.SetBool("isAtack",isAtack);
    }

	void FixedUpdate() {
		isGroundCheck = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "coleta") {
			_GameController.playSFX(_GameController.sfxMoeda,0.5f);
			_GameController.getCoin();
			Destroy(col.gameObject);
		} else if (col.gameObject.tag == "enemy") {
			StartCoroutine("damageController");
			if (_GameController.vida > 0) StartCoroutine("damageController");
		} 
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
		Destroy(hitBoxTemp, 0.5f);
	}

	void footStep() {
        _GameController.playSFX(_GameController.sfxStep[Random.Range(0,_GameController.sfxStep.Length)],0.5f);
    }

	IEnumerator damageController() {
		this.gameObject.layer = LayerMask.NameToLayer("Invencible");
		if (--maxHP <= 0) {
			Debug.LogError("GameOver");
		}
		_GameController.playSFX(_GameController.sfxDano,0.5f);
		playerSR.color = hitColor;
		yield return new WaitForSeconds(0.3f);
		playerSR.color = noHitColor;
		for(int i=0; i<5; i++) {
			playerSR.enabled = false;
			yield return new WaitForSeconds(0.2f);
			playerSR.enabled = true;
			yield return new WaitForSeconds(0.2f);
		}
		playerSR.color = Color.white;
		this.gameObject.layer = LayerMask.NameToLayer("Player");
	}
}
