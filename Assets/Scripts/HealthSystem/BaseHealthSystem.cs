using UnityEngine;

namespace HealthSystem
{
    public class BaseHealthSystem : HealthSystem
    {
        [SerializeField] private float hpRegenPercentage;
        public static BaseHealthSystem Instance;

        protected override void Start()
        {
            base.Start();
            Instance = this;
        }

        private void Update()
        {
            // regen hp when day time.
            if(TimeSystem.Instance.timeState != TimeState.Day) return;
            Heal((hpRegenPercentage / 100) * maxHp * Time.deltaTime);
        }
    }
}
