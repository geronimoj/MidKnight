using UnityEngine;

/// <summary>
/// every enemy has this script
/// </summary>
public class Enemy : Character
{
    /// <summary>
    /// a reference to this enemy's animation
    /// </summary>
    Animator enemyAnim;
    /// <summary>
    /// a reference to this enemy's hitbox
    /// </summary>
    EnemyHitbox enemyHitbox;
    /// <summary>
    /// a reference to this enemy's character controller
    /// </summary>
    CharacterController enemyCC;
    /// <summary>
    /// A reference to the skinned mesh renderer for models
    /// </summary>
    SkinnedMeshRenderer smr;
    /// <summary>
    /// the damage this enemy deals
    /// </summary>
    public int damage;
    /// <summary>
    /// checks if the enemy is in its second phase 
    /// </summary>
    [HideInInspector] public bool isPhase2 = false;
    /// <summary>
    /// the time until the gameobject is destroyed
    /// </summary>
    float timeTillDestroy = 5;
    /// <summary>
    /// the downward force
    /// </summary>
    public float gravity;
    /// <summary>
    /// the overall vertical speed
    /// </summary>
    public float vertSpeed;
    /// <summary>
    /// returns true if the enemy is dead
    /// </summary>
    bool isDead = false;
    //For saving, loading & not respawning
    private EntitiesManager EM;
    public bool isBoss = false;

    public float damagedFlashDuration = 1.5f;
    private float damagedTimer;
    public float damagedFlashSpeed = 0.25f;
    [Range(0,1)]
    public float alphaLimit = 1;

    private float flashTimer;
    private bool flashDirection = false;

    public AudioSource Audio;
    public AudioClip attackAudio;
    public AudioClip deathAudio;

    // Start is called before the first frame update
    public virtual void Start()
    {
        //initialise stuff
        enemyAnim = GetComponent<Animator>();
        enemyHitbox = GetComponentInChildren<EnemyHitbox>();
        enemyCC = GetComponent<CharacterController>();
        health = MaxHealth;
        EM = FindObjectOfType<EntitiesManager>();

        if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
            smr = GetComponentInChildren<SkinnedMeshRenderer>();

        Flash();
    }

    // Update is called once per frame
    protected override void ExtraUpdate()
    {
        damagedTimer -= Time.deltaTime;
        if (damagedTimer > 0)
            Flash();
        else if (smr != null)
            smr.materials[0].SetColor("_BaseColor", new Color(1, 1, 1, 0));

        if(isDead)
        {
            timeTillDestroy -= Time.deltaTime;
        }

        if(timeTillDestroy < 0)
        {
            LogEntity();
            Destroy(gameObject);
        }
    }

    private void Flash()
    {   //Check that we got a renderer
        if (smr == null)
            return;
        //Check that we have enough materials
        if (smr.materials.Length < 2)
        {
            Debug.LogWarning("Flash Matterial not assigned. Assign flash to element 0.");
            return;
        }
        Color col = smr.materials[0].color;

        if (flashDirection)
        {
            //We are brightening
            //Increment the flash timer
            flashTimer += Time.deltaTime;
            flashDirection = flashTimer < damagedFlashSpeed;

            col.a = (flashTimer / damagedFlashSpeed) * alphaLimit;
        }
        else
        {
            //We are darkening
            //Decrement the flash timer
            flashTimer -= Time.deltaTime;
            flashDirection = flashTimer <= 0;

            col.a = (flashTimer / damagedFlashSpeed) * alphaLimit;
        }

        smr.materials[0].SetColor("_BaseColor", col);
    }

    /// <summary>
    /// Call this when the enemy dies
    /// </summary>
    protected override void OnDeath()
    {
        enemyAnim.SetTrigger("death");
        enemyCC.enabled = false;
        Destroy(enemyHitbox);
        isDead = true;
        Audio.PlayOneShot(deathAudio);
    }

    private void LogEntity()
    {
        Entities e;
        Room R = GetComponentInParent<Room>();
        e.thisRoom = R.roomID;
        e.index = -1;

        if (R == null)
        {
            Debug.LogError("Could not find Room in parent. Searching entire scene");
            R = FindObjectOfType<Room>();

            if (R == null)
            {
                Debug.LogError("Could not find Room in entire scene. Destroying myself now");
                OnDeath();
            }
        }


        for (int i = 0; i < R.NonRespawningRoomObjects.Count; i++)
        {
            if (R.NonRespawningRoomObjects[i] == gameObject)
            {
                e.index = i;
                break;
            }
        }

        if (isBoss)
        {
            EM.EntitiesToNeverRespawn.Add(new Entities { thisRoom = R.roomID, index = e.index });
        }

        EM.EntitiesToNotRespawnUntillRest.Add(new Entities { thisRoom = R.roomID, index = e.index });
        gameObject.SetActive(false);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        damagedTimer = damagedFlashDuration;
        flashTimer = 0;
        flashDirection = true;
    }
}
