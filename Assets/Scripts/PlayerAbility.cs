using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerAbility : MonoBehaviour
{
    public GameObject EnergyTemp;
    public static GameObject Energy;
    public static bool wall = false;
    // We only need one instance of player health
    [SerializeField] public static int PlayerHP { get;  private set; }
    [SerializeField] private bool isFacingRight;
    [SerializeField] private float wallSlideY;
    [SerializeField] private int dashForce;
    [SerializeField] private bool dashCD;
    public GameObject projectilePrefab;



    private Rigidbody2D rb;
    public bool iFrame = false;
    private PlayerMovement playerMove;

    void Start()
    {
        Energy=EnergyTemp;
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMovement>();
        PlayerHP = 40;
        wallSlideY = 1.75f;
    }

    void Update()
    {     
        FlipCheck();
        AbilityCheck();
    }

    private void FlipCheck()
    {
        // Set a threshold for flip detection so player doesn't flicker uncontrollably
        const float threshold = 0.1f;
        if (Mathf.Abs(rb.velocity.x) > threshold) {
            // negative velovity suggests moving left
            if (rb.velocity.x < 0 && isFacingRight)
                Flip();
            // positive velovity suggests moving right (0 is is idle movement)
            if (rb.velocity.x > 0 && !isFacingRight)
                Flip();
        }
    }

    private void AbilityCheck()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            RollPlayer();
            return;
        }

        // Conflicts with movement state velocity
        if (Input.GetKeyDown(KeyCode.W)) {
            DashPlayer();
            return;
        }

        // Check for projectile ability (left click)
        if (Input.GetMouseButtonDown(0)) {
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            FireProjectile(direction);
            return;
        }
        // Add more abilities as necessary
    }

    private void RollPlayer()
    {
       
        if (!iFrame)
        {
            playerMove.StateCheck("Roll");
            StartCoroutine(DamageTimer());
        }
    }

    private void DashPlayer()
    {
        if (!dashCD) {
            playerMove.StateCheck("Dash");
            Vector2 dash = new Vector2(dashForce, rb.velocity.y);
            rb.velocity = dash;
            StartCoroutine(DashCooldown());
        }
    }

    public void Damage(int dmg)
    {
        // We don't want player to instaly perish on one collision
        if (!iFrame)
        {
            PlayerHP -= dmg;
            CheckHealth();
            StartCoroutine(DamageTimer());
        }

        // Prevent player's health from exceeding the maximum.
        if (PlayerHP > 100)
        {
            PlayerHP = 100;
        }
    }

    private void CheckHealth()
    {
        if (PlayerHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Allows invincibilty frames to trigger
    IEnumerator DamageTimer()
    {
        float cooldown = 3f;
        iFrame = true;

        // Apply invincibility scale, preserving X direction
        transform.localScale = new Vector3(transform.localScale.x, 0.5f, 1f);

        while (cooldown > 0)
        {
            cooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }
        iFrame = false;

        // Restore scale, preserving X direction
        transform.localScale = new Vector3(transform.localScale.x, 1f, 1f);
    }

    IEnumerator DashCooldown()
    {
        float cooldown = 1f;
        dashCD = true;
        while(cooldown > 0)
        {
            cooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }
        dashCD = false; 
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        // Flip the player's local scale on the X axis
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        dashForce *= -1;
    }

    private void FireProjectile(Vector2 direction)
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, new Vector2(transform.position.x+.1f,transform.position.y), Quaternion.identity);
        Rigidbody2D projectileRb = projectileInstance.GetComponent<Rigidbody2D>();
        Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
        if (projectileRb != null && projectileScript != null)
        {
            projectileRb.velocity = direction * projectileScript.projectileSpeed;
        }
    }   

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        GameObject collider = collision.gameObject;
        // Allow player to slide vertically against when hitting a wall
        rb.drag = collider.CompareTag("Wall") ? wallSlideY: 0;
        if(collision.gameObject.tag=="Wall"){
            wall=true;
        }
    }

   void OnCollisionExit2D(Collision2D col){
     if(col.gameObject.tag=="Wall"){
        wall=true;
      }
   }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Wall")){
            playerMove.IsOnWall(true);
           
            
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Wall")){

            playerMove.IsOnWall(false);
             
        }
    }

}
