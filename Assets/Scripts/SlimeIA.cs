using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIA : MonoBehaviour
{

    private GameController _GameController;
    private Rigidbody2D slimeRb;
    private Animator slimeAnimator;

    public GameObject hitbox;

    public float speed;
    public float timeToWalk;
    private int h;
    public bool isLookLeft;
    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        slimeRb = GetComponent<Rigidbody2D>();
        slimeAnimator = GetComponent<Animator>();

        StartCoroutine("slimeWalk");
    }

    // Update is called once per frame
    void Update()
    {
        if (_GameController.currentState != GameController.gameState.GAMEPLAY) return;

		if ( h > 0 && isLookLeft) {
			Flip();
		} else if (h < 0 && !isLookLeft) {
			Flip();
		}
        slimeRb.velocity = new Vector2(h*speed, slimeRb.velocity.y);

        if (h != 0) {
            slimeAnimator.SetBool("isWalk",true);
        } else {
            slimeAnimator.SetBool("isWalk",false);
        }
    }

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "hitBox") {
            Destroy(hitbox);
		    this.gameObject.layer = LayerMask.NameToLayer("Invencible");
            h=0;
            StopCoroutine("slimeWalk");
			_GameController.playSFX(_GameController.sfxMorteInimigo,0.2f);
			slimeAnimator.SetTrigger("dead");
		}
	}

    IEnumerator slimeWalk() {
        int rand = Random.Range(0,100);

        if (rand<33) h = 1;
        else if (rand > 33 && rand < 66) h = 0;
        else h = -1;

        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("slimeWalk");
    }

    void onDead() {
        Destroy(this.gameObject);
    }

    void Flip() {
		isLookLeft = !isLookLeft;
		float x = transform.localScale.x * -1;
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

}
