using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState{
    idle,
    walk,
    attack,
    stagger
}


public class Enemy : MonoBehaviour
{
    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Enemy Stats")]
    public EnemyInfo enemyInfo;
    public EnemyInfo enemyInfo2;
    public FloatValue maxHealth;
    public float health;
    public float moveSpeed;
    //public string enemyName;
    public int baseAttack;
    public Vector2 homePos;

    [Header("Death Effects")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f;
    public LootTable thisLoot;
    public LootTableItem thisLoot2;

    [Header("Death Signals")]
    public SignalSender roomSignal;

    private void Awake()
    {
        health = maxHealth.initialValue;
        homePos = transform.position;
    }

    private void OnEnable()
    {
        transform.position = homePos;
        currentState = EnemyState.idle;
        health = maxHealth.initialValue;
        //if(!this.enemyinfo.isAlive || !this.enemyInfo2.isAlive)
        //{
        //DeathEffect();
        //MakeLoot2();
        //}
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            DeathEffect();
            MakeLoot();
            //MakeLoot2();
            if(roomSignal != null)
            {
                roomSignal.Raise();
            }
            this.gameObject.SetActive(false);
        }
    }

    private void MakeLoot()
    {
        if(thisLoot != null)
        {
            PowerUp current = thisLoot.LootPowerUp();
            if(current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
    /*
        private void MakeLoot2()
    {
        if(thisLoot2 != null)
        {
            InventoryItem current = thisLoot2.LootInventoryItem();
            if(current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
    */
    private void DeathEffect()
    {
        if(deathEffect != null){
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }

    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if(myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}
