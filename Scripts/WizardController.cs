using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    private GameObject player;
    public GameObject FireBallPrefab;
    private Animator animator;
    private float lastShoot = 0;
    private bool hitted;
    private float attackCoolDown;
    private float health = 20;
    private GameObject blood;
    private GameObject damageMessagePopUp;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = player.transform.position - transform.position;
        if(direction.x >= 0){
            transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        }
        else{
            transform.localScale = new Vector3(-1.5f,1.5f,1.5f);
        }
        float distanceX = Mathf.Abs(player.transform.position.x - transform.position.x);
        float distanceY = Mathf.Abs(player.transform.position.y - transform.position.y);

        hitted = GetComponent<EnemyController>().getHitted();
        if(hitted)
        {
            GetComponent<EnemyController>().cancelHitted();
            Hitted(GetComponent<EnemyController>().getDamageReceived());
        }

        if(health > 0)
        {
            if (!hitted && distanceX < 5 && distanceY < 5 && Time.time > lastShoot + 2f)
            {
                lastShoot = Time.time;
                animator.SetTrigger("Shoot");
            }
        }
        else
        {
            animator.SetTrigger("Dead");
        }
    }

    public void Shoot(){
        Vector3 direction;
        if(transform.localScale.x > 0){
            direction = Vector2.right;
        }
        else{
            direction = Vector2.left;
        }
        GameObject fireBall = Instantiate(FireBallPrefab, transform.position + direction*0.5f, Quaternion.identity);
        fireBall.GetComponent<FireBallScript>().setDirection(direction);
    }

    private void Hitted(float damage){
        health -= damage;
        hitted = true;
        damageMessagePopUp.GetComponent<TextMeshPro>().text = (damage) + " ";
        Instantiate(damageMessagePopUp, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Instantiate(blood, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddForce(Vector2.left, ForceMode2D.Impulse);
    }

    void FinishDeath()
    {
        Destroy(gameObject);
    }
}
