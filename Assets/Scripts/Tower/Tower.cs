using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tower
{
    public class Tower : MonoBehaviour
    {
        #region Declare Variables
        [Header("Tower Stats & Config")] 
        [SerializeField] private float attackRadius;
        [SerializeField] private float attackCooldown;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private LayerMask targetLayerMask;
        [SerializeField] private LayerMask alphaLayerMask;

        [Header("Bullet")] 
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private float bulletDamage;
        [SerializeField] private float bulletSpeed;

        private Collider2D[] _targetInAttackAreas;
        private float _nextAttackTime;
        private GameObject _currentTarget;
        [HideInInspector] public SpriteRenderer spriteRenderer;
        public GameObject CurrentTarget => _currentTarget;

        #endregion

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            AlphaDetect();
            _targetInAttackAreas = Physics2D.OverlapCircleAll(transform.position, attackRadius, targetLayerMask);   
            if (_targetInAttackAreas.Length <= 0) return;

            TargetHandler();
            ShootBulletHandler();
        }

        #region Methods
        /// <summary>
        /// Target the nearest enemy in the attack radius.
        /// </summary>
        private void TargetHandler()
        {
            var targetDistances = new List<float>();
            foreach (var target in _targetInAttackAreas)
                targetDistances.Add(Vector2.Distance(transform.position, target.transform.position));
            _currentTarget = _targetInAttackAreas[targetDistances.IndexOf(targetDistances.Min())].gameObject;
        }


        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Logic Shoot bullet to the current target.
        /// </summary>
        private void ShootBulletHandler()
        {
            if (Time.time < _nextAttackTime) return;
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity)
                .GetComponent<Bullet>();
            bullet.Init(bulletSpeed, bulletDamage, this);
            _nextAttackTime = Time.time + attackCooldown;
        }

        private void AlphaDetect()
        {
            Collider2D[] objInSprite = Physics2D.OverlapBoxAll(transform.position + spriteRenderer.sprite.bounds.center,
                spriteRenderer.sprite.bounds.size + new Vector3(-2,-1,0), 0, alphaLayerMask);
            if (objInSprite.Length > 0)
            {
                if (objInSprite.Any(obj => obj.transform.position.y > transform.position.y+0.5f))
                {
                    if (spriteRenderer.color.a > 0.5f)
                    {
                        Color color = spriteRenderer.color;
                        color.a -= 0.01f;
                        spriteRenderer.color = color;
                    }
                }
                else if (spriteRenderer.color.a < 1)
                {
                    Color color = spriteRenderer.color;
                    color.a += 0.01f;
                    spriteRenderer.color = color;
                }
            }
            else if (spriteRenderer.color.a < 1)
            {
                Color color = spriteRenderer.color;
                color.a += 0.01f;
                spriteRenderer.color = color;
            }
        }

        /// <summary>
        /// Draw attack radius in the inspector.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
        #endregion
    }
}

