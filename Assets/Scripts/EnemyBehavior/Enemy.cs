using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HealthSystem;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace EnemyBehavior
{
    public enum EnemyState
    {
        Idle,
        FollowTarget,
        ReturnToSpawn,
        WaitToReturn,
    }

    public enum PriorityTag
    {
        Player,
        NCP,
        Tower,
        Base
    }
    
    public class Enemy : MonoBehaviour
    {
        #region Declare Variable
        [SerializeField] private float viewDistance;
        [SerializeField] private float roamDuration; 
        [SerializeField] private float roamCooldown; 
        [SerializeField] private Vector2 roamArea; 
        [SerializeField] private bool nightMode;
        [SerializeField] private LayerMask playerLayerMask;  
        [SerializeField] private List<PriorityTag> priorityTags;
        [SerializeField] private EnemyState enemyActionState;
        private List<Collider2D> _targetInDistances;
        private Coroutine _waitToReturnCoroutine;
        private Coroutine _roamAroundCoroutine;
        private Vector2 _spawnPoint;
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private float _lastTimeTurn;

        public bool NightMode { get => nightMode; set => nightMode = value; }
        public bool IsStun { get; set; }
        public GameObject Target { get; set; }
        public NavMeshAgent Agent { get; private set; }
        private EnemyHealthSystem _enemyHealthSystem;
        #endregion
    
        protected void Start()
        {
            _enemyHealthSystem = GetComponent<EnemyHealthSystem>();
            _animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            GetComponent<SpriteRenderer>();
            Agent.updateUpAxis = false;
            Agent.updateRotation = false;
            _spawnPoint = transform.position;
            enemyActionState = EnemyState.Idle;
        }

        protected void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttack") || IsStun || _enemyHealthSystem.isDead)
            {
                Agent.velocity = Vector2.zero;
                return;
            }
            
            BehaviorHandle();
            AnimationUpdate();
        }
    
        #region Methods
        /// <summary>
        /// Handle enemy behavior.
        /// </summary>
        private void BehaviorHandle()
        {
            _targetInDistances = Physics2D.OverlapCircleAll(transform.position, viewDistance, playerLayerMask).ToList();

            if (_targetInDistances.Count > 0)
                FollowTargetHandle();
        
            else if (nightMode)
                Agent.SetDestination(GameManager.Instance.playerBase.position);
        
            else if (!CheckState(EnemyState.Idle))
                ReturnToSpawnHandle();
        
            else
                IdleHandle();
        }
    
    
        /// <summary>
        /// Select target with the first priority tag found in the target list.
        /// </summary>
        private void SelectTarget()
        {
            if (Target) return;
        
            // Detect and set target only on the first priority tag found 
            foreach (PriorityTag priorityTag in priorityTags)
            {
                Collider2D target = _targetInDistances.FirstOrDefault(target => target.CompareTag(priorityTag.ToString()));
                if(!target) continue;
                Target = target.gameObject;
                break;
            }
        }
    
    
        /// <summary>
        /// Follow target.
        /// </summary>
        private void FollowTargetHandle()
        {
            SelectTarget();
            if (!Target || !Target.activeSelf)
            {
                ReturnToSpawnHandle();
                Target = null;
                return;
            }
            SetEnemyState(EnemyState.FollowTarget);
            Agent.SetDestination(Target.transform.position);
        }

    
        /// <summary>
        /// Return to spawn point.
        /// </summary>
        private void ReturnToSpawnHandle()
        {
            if(Target && Target.activeSelf)
                Agent.SetDestination(Target.transform.position);
        
            if (_waitToReturnCoroutine != null) return;
            _waitToReturnCoroutine = StartCoroutine(WaitAndReturn(3));
        }
    
    
        /// <summary>
        /// Idle and roam around spawn point.
        /// </summary>
        private void IdleHandle()
        {
            SetEnemyState(EnemyState.Idle);
            if (_roamAroundCoroutine != null) return;
            _roamAroundCoroutine = StartCoroutine(RoamAround(roamDuration));
        }
    
    
        /// <summary>
        /// Update enemy animation.
        /// </summary>
        private void AnimationUpdate()
        {
            _animator.SetFloat(Speed, Agent.velocity.magnitude);

            // flip horizontal direction relate with enemy direction
            if (Mathf.Abs(Agent.velocity.x) > 0.25f && Time.time > _lastTimeTurn + 0.25f)
            {
                transform.right = Agent.velocity.x < 0 ? Vector2.left : Vector2.right;
                _lastTimeTurn = Time.time;
            }
        }

    
        /// <summary>
        /// Set enemy state.
        /// </summary>
        /// <param name="state">State that you want to set.</param>
        private void SetEnemyState(EnemyState state)
        {
            enemyActionState = state;
        }
    
    
        /// <summary>
        /// Check enemy state.
        /// </summary>
        /// <param name="state">State that you want to check.</param>
        /// <returns></returns>
        private bool CheckState(EnemyState state)
        {
            return state == enemyActionState;
        }
    
    
        /// <summary>
        /// Wait for a while and return to spawn point.
        /// </summary>
        /// <param name="time">Time to wait.</param>
        /// <returns></returns>
        private IEnumerator WaitAndReturn(float time = 0)
        {
            SetEnemyState(EnemyState.WaitToReturn);
            yield return new WaitForSeconds(time);
        
            SetEnemyState(EnemyState.ReturnToSpawn);
            Target = null;
            while (CheckState(EnemyState.ReturnToSpawn))
            {
                Agent.SetDestination(_spawnPoint);
                if ((Vector2)transform.position == _spawnPoint)
                {
                    SetEnemyState(EnemyState.Idle);
                    break;
                }
                yield return null;
            }
            _waitToReturnCoroutine = null;
        }
    
    
        /// <summary>
        /// Roam around spawn point.
        /// </summary>
        /// <param name="time">Roam duration.</param>
        /// <returns></returns>
        private IEnumerator RoamAround(float time)
        {
            float timeCount = 0;
            Vector2 randomPos = new Vector2(_spawnPoint.x + Random.Range(roamArea.x, roamArea.y),
                _spawnPoint.y + Random.Range(roamArea.x, roamArea.y));

            yield return new WaitForSeconds(roamCooldown);

            while (timeCount < time)
            {
                if (!CheckState(EnemyState.Idle)) break;
                timeCount += Time.deltaTime;
                Agent.SetDestination(randomPos);
                yield return null;
            }
            _roamAroundCoroutine = null;
        }
    
    
        /// <summary>
        ///  Draw gizmos on inspector
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, viewDistance);
        }
        #endregion
    }
}
