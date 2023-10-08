using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        [Header("Bullet")] 
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private float bulletDamage;
        [SerializeField] private float bulletSpeed;

        private Collider2D[] _targetInAttackAreas;
        private float _nextAttackTime;
        private GameObject _currentTarget;
        public GameObject CurrentTarget => _currentTarget;

        #endregion

        private void Update()
        {
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

