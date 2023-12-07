using UnityEngine;

namespace Tower
{
    public class Bullet : MonoBehaviour
    {
        #region Declare Variables
        private float _speed;
        private float _damage;
        private Tower _parentTower;
        private Rigidbody2D _rb2d;
        private Transform _target;
        private Transform _thisTransform;
        #endregion

        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _thisTransform = transform;
        }
    
        private void Update()
        {
            if (!_target || !_parentTower)
            {
                Destroy(gameObject);
                return;
            }
            
            Vector2 direction = _target.position - _thisTransform.position;
            _thisTransform.right = direction;
            _rb2d.velocity = direction.normalized * _speed;
        }

        #region Methods
        /// <summary>
        /// Initialize the bullet when it spawned.
        /// </summary>
        /// <param name="speed">Bullet speed.</param>
        /// <param name="damage">Bullet damage.</param>
        /// <param name="tower">Parent tower.</param>
        public void Init(float speed, float damage,Tower tower)
        {
            _speed = speed;
            _damage = damage;
            _parentTower = tower;
            _target = tower.CurrentTarget.transform;
        }

        
        /// <summary>
        /// When the bullet hit the target, the target will take damage.
        /// </summary>
        /// <param name="other">Target hit.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject != _target.gameObject) return;


            if (_target.GetComponent<HealthSystem.HealthSystem>() as BossGolemHeathSystem)
            {
                _target.GetComponent<HealthSystem.HealthSystem>().TakeDamage(_damage / 10,_parentTower.gameObject);
                Destroy(gameObject);
                return;
            }
                
            _target.GetComponent<HealthSystem.HealthSystem>().TakeDamage(_damage,_parentTower.gameObject);
            Destroy(gameObject);
        }
        #endregion
    }
}