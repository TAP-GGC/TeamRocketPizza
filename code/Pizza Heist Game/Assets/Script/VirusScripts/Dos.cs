using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dos : Virus
{
    public float stunRange;
    public float stunDuration;
    public float stunCooldown;
    private float nextStunTime;
    private ParticleSystem partSys;

    // Dictionary to track currently stunned towers
    private Dictionary<Defense, Coroutine> stunnedTowers = new Dictionary<Defense, Coroutine>();

    public override void Start()
    {
        base.Start();
        partSys = GetComponentInChildren<ParticleSystem>();
        nextStunTime = Time.time;
    }

    public override void UseAbilties()
    {
        if (Time.time >= nextStunTime)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, stunRange);
            foreach (Collider2D collider in colliders)
            {
                Defense tower = collider.GetComponent<Defense>();
                if (tower != null)
                {
                    // Only start a stun if the tower is not already stunned
                    if (!stunnedTowers.ContainsKey(tower))
                    {
                        Coroutine stunCoroutine = StartCoroutine(StunTower(tower, stunDuration));
                        stunnedTowers.Add(tower, stunCoroutine);
                        partSys.Play();
                    }
                }
            }

            nextStunTime = Time.time + stunCooldown;
        }
    }

    private IEnumerator StunTower(Defense tower, float duration)
    {
        // Disable the tower script
        tower.enabled = false;
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Re-enable the tower script and remove from the dictionary
        tower.enabled = true;
        stunnedTowers.Remove(tower);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stunRange);
    }

    new void Update()
    {
        base.Update();
        UseAbilties();
    }
}
