using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    private Vector3 originalScale;
    private GameObject player;
    public GameObject FireBallPrefab;
    private Animator animator;
    private float lastShoot = 0;
    private bool hitted;
    private float attackCoolDown;
    private float health = 150;
    private GameObject blood;
    private GameObject damageMessagePopUp;
    private GameObject tears;
    private int amountOfTearsToDrop = 144;

    // Start is called before the first frame update
    void Start()
    {
        tears = GetComponent<EnemyController>().getTears();
        originalScale = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        blood = GetComponent<EnemyController>().getBlood();
        damageMessagePopUp = GetComponent<EnemyController>().getDamageMessagePopUp();
    }

    // Update is called once per frame
    void Update()
    {

        
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
            if (!hitted && distanceX < 10 && distanceY < 10 && Time.time > lastShoot + 4.5f)
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

    public void Tracking(){
        Vector3 direction = player.transform.position - transform.position;
        if(direction.x >= 0){
            transform.localScale = new Vector3(originalScale.x,originalScale.y,1.5f);
        }
        else{
            transform.localScale = new Vector3(-originalScale.x,originalScale.y,1.5f);
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
        tears.GetComponent<TearsController>().currentValue = 0;
       tears.GetComponent<TearsController>().finalValue = amountOfTearsToDrop;
       Instantiate(tears, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Destroy(gameObject);
    }
}
