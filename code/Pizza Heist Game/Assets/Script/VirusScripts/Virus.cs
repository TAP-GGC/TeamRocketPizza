using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Virus : MonoBehaviour
{

    [Header("References")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    [Header("Virus Stats")]
    public int hitPoints;
    public int coinsWorth;
    public int damage;
    public float speed;
    public int waypointIndex = 0;
    public Transform target;
    // Update is called once per frame

    private float origSpeed;
    private Color originalColor;
    private Coroutine slowEffectCoroutine; 

    public virtual void Start()
    {
        target = LevelManager.main.waypoints[waypointIndex];
        //myTran = transform;
        originalColor = spriteRenderer.color;
        origSpeed = speed;
    }

    protected virtual void Update()
    {
        if(Vector2.Distance(target.position, transform.position) < 0.1f){
            waypointIndex++;
            
        }


        if(waypointIndex == LevelManager.main.waypoints.Length){
            EnemySpawner.enemyDestroy.Invoke();
            Destroy(gameObject);
            dealDamage();
            return;
        } else {
            target = LevelManager.main.waypoints[waypointIndex];
        }

         
    }

    protected void FixedUpdate() {
        
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        
    }
    public void TakeDamage(int dmg){
        hitPoints -= dmg;
        Debug.Log($"{gameObject.name} took {dmg} damage. Remaining HP: {hitPoints}");
        if(hitPoints <= 0){
            OnDeath();
        }
    }
    
    public virtual void OnDeath(){
            EnemySpawner.enemyDestroy.Invoke();
            LevelManager.main.IncreaseCoin(coinsWorth);
            Destroy(gameObject);
    }
    public void dealDamage(){
        LevelManager.main.decreaseHealth(damage);
    }

    public void ApplySlow(float slowAmount, float duration)
    {
        if (slowEffectCoroutine != null)
        {
            StopCoroutine(slowEffectCoroutine);
        }
        speed = origSpeed * (1f - slowAmount);
        ChangeColor();
        slowEffectCoroutine = StartCoroutine(RemoveSlowAfterDuration(duration)); 
    }

    private IEnumerator RemoveSlowAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = origSpeed;
        spriteRenderer.color = originalColor;
        slowEffectCoroutine = null;
    }

    private void ChangeColor()
    {
        if (spriteRenderer != null)
        {
            // Blend the original color with an orange tint (RGB: 1, 0.65, 0)
            
            Color orangeTint = new Color(1f, 0.65f, 0f);
            
            // Interpolate between the original color and the orange tint to create a highlighted effect
            spriteRenderer.color = Color.Lerp(originalColor, orangeTint, 0.2f); // 0.5f to blend 50%
        }
    }


    public abstract void UseAbilties();

    
    
}
