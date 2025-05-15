using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerAbility : MonoBehaviour
{
    // We only need one instance of player health
    [SerializeField] public static int PlayerHP { get; private set; }
    [SerializeField] private float wallDrag;


    private Rigidbody2D rb;
    private bool iFrame = false;
    private PlayerMovement playerMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMovement>();
        PlayerHP = 100;
    }

    void Update()
    {
        AbilityCheck();
    }

    private void AbilityCheck()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RollPlayer();
            return;
        }
        // Add more abilities as necessary
    }

    private void RollPlayer()
    {
        playerMove.StateCheck("Rolling");
        if (!iFrame)
        {
            StartCoroutine(DamageTimer());
        }
    }

    public void Damage(int dmg)
    {
        // We don't want player to instaly perish on one collision
        if (!iFrame)
        {
            PlayerHP -= dmg;
            StartCoroutine(DamageTimer());
        }

        // Prevent player's health from exceeding the maximum.
        if (PlayerHP > 100)
        {
            PlayerHP = 100;
        }
    }

    // Allows invincibilty frames to trigger
    IEnumerator DamageTimer()
    {
        float cooldown = 2f;
        iFrame = true;
        while (cooldown > 0)
        {
            cooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }
        iFrame = false;
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        GameObject collider = collision.gameObject;
        // Allow player to slide vertically against when hitting a wall
        rb.drag = collider.CompareTag("Wall") ? wallDrag : 0;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Wall"))
            playerMove.IsOnWall(true);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Wall"))
            playerMove.IsOnWall(false);
    }

}
