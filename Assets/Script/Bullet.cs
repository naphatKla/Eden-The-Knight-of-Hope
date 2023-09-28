using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float damage;
    public Tower tower;
    private Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        if(target == null || tower == null) Destroy(gameObject);
        Vector2 direction = target.transform.position - transform.position;
        transform.up = direction;
        rb2d.velocity = direction.normalized * speed;
    }

    public void Init(GameObject target, float speed, float damage, Tower tower)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.tower = tower;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject != target.gameObject) return;
        
        target.GetComponent<EnemyHealthSystem>().TakeDamage(damage,tower.gameObject);
        Destroy(gameObject);
    }
}
