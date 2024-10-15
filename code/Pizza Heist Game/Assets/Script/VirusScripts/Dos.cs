using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dos : Virus
{
    public float stunRange; // Range within which towers are stunned
    public float stunDuration; // Duration of the stun effect
    public float stunCooldown; // Time between stun effects
    private float nextStunTime; // Keeps track of when the next stun can occur // Duration for which the towers are disabled


    public override void Start()
    {
        base.Start();
        nextStunTime = Time.time;
    }
    public override void UseAbilties()
    {
        if (Time.time >= nextStunTime)
            {
                // Find towers within the stun range
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, stunRange);
                foreach (Collider2D collider in colliders)
                {
                    Defense tower = collider.GetComponent<Defense>(); // Assuming Tower is the base class for your towers
                    if (tower != null)
                    {
                    StartCoroutine(StunTower(tower, stunDuration));
                    // play animation
                }
                }

                nextStunTime = Time.time + stunCooldown; // Reset the stun timer
            }
    }

    private IEnumerator StunTower(Defense tower, float duration)
    {
        // Disable the tower script
        tower.enabled = false;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Re-enable the tower script
        tower.enabled = true;
    }

    new void Update(){
        base.Update();
        UseAbilties();
    }
}
