using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombatSystem;
using UnityEngine;

public class BossCombatSystem : EnemyCombatSystem
{
    [SerializeField] private GameObject bossAttackAreaIndicator;

    protected override IEnumerator Attack(float delay)
    {
        lastAttackTime = Time.time;
        animator.SetTrigger(attackState.ToString());
        bossAttackAreaIndicator.SetActive(true);
        Color color = bossAttackAreaIndicator.GetComponent<SpriteRenderer>().color;
        Color color2 = bossAttackAreaIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        
        float timeCount = 0;
        while (timeCount < delay)
        {
            if (color.a <= 0.25f && color2.a <= 0.25f)
            {
                color.a = timeCount;
                color2.a = timeCount;
            }
            
            bossAttackAreaIndicator.GetComponent<SpriteRenderer>().color = color;
            bossAttackAreaIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color2;
            
            timeCount += Time.deltaTime;
            Vector2 locaScale = bossAttackAreaIndicator.transform.GetChild(0).transform.localScale;
            locaScale.x = Mathf.Lerp(0, 1, timeCount / delay);
            bossAttackAreaIndicator.transform.GetChild(0).transform.localScale = locaScale;
            yield return null;
        }
        
        List<HealthSystem.HealthSystem> targetHealthSystems = TargetInAttackArea.Select(target => target.GetComponent<HealthSystem.HealthSystem>()).ToList();
        targetHealthSystems.ForEach(target => target.TakeDamage(currentAttackPattern.power * attackStat,gameObject));
        
        // Change attack stage to next stage, if attack state is the last state, change to the first state.
        attackState = (int)attackState >= attackPatterns.Count - 1 ? AttackState.AttackState0 : attackState + 1;
        attackCoroutine = null;
        
        timeCount = 0;
        while (timeCount < 0.5f)
        {
            color.a = 0.5f - (timeCount / 0.5f);
            color2.a = 0.5f - (timeCount / 0.5f);
            bossAttackAreaIndicator.GetComponent<SpriteRenderer>().color = color;
            bossAttackAreaIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color2;
            timeCount += Time.deltaTime;
            yield return null;
        }
        bossAttackAreaIndicator.SetActive(false);
    }
}
