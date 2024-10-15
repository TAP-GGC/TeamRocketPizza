using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trojan : Virus
{

    public Sprite happySprite; // The sprite for the happy state
    public Sprite angrySprite; // The sprite for the berserk state
    public int berserkSpeed = 3;
    public int berserkHealth = 10; // Speed multiplier for berserk mode
    private bool isBerserk = false; // Tracks whether the Trojan is in berserk mode

    
    private Animator animator;

    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Set initial happy state
        spriteRenderer.sprite = happySprite;
        spriteRenderer.color = Color.yellow; // Set the color to yellow for happy
        // animator.Play("HappyWalking");
    }
    public override void UseAbilties()
    {
        
        if (!isBerserk)
        {
            isBerserk = true;
            
            
            // Transform to berserk state
            spriteRenderer.sprite = angrySprite; // Change to the angry sprite
            spriteRenderer.color = Color.red; // Change the color to red for angry
            speed = berserkSpeed;
            hitPoints = berserkHealth; // Increase speed
            // animator.Play("AngryWalking");
            Debug.Log("Trojan is now berserk!");
            
            // Optionally, add visual or sound effects here
        }
        else{
            base.OnDeath();
        }
        
    }

    public override void OnDeath()
    {
        UseAbilties();
        
    }

    protected override void Update()
    {
        base.Update();
    }
}
