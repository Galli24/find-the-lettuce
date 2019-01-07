using UnityEngine;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (SpriteRenderer))]
public class Player : MonoBehaviour {

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .1f;
    float accelerationTimeGrounded = .05f;
    float moveSpeed = 3;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    public GameObject wave;
    Controller2D controller;

    SpriteRenderer spriteRenderer;
    Animator anim;

    float timeSinceLastWave = 0f;

	void Start () {
        controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        if (timeSinceLastWave < 1.5f)
            timeSinceLastWave += Time.deltaTime;
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                    timeToWallUnstick -= Time.deltaTime;
                else
                    timeToWallUnstick = wallStickTime;
            }
            else
                timeToWallUnstick = wallStickTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z))
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }

            if(controller.collisions.below)
                velocity.y = maxJumpVelocity;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Z))
        {
            if (velocity.y > minJumpVelocity)
                velocity.y = minJumpVelocity;
        }

        if (Input.GetKeyDown(KeyCode.E) && timeSinceLastWave >= 0.5f && PlayerVariables.instance.getPlayerCanMove())
        {
            GameObject instanciateWave = Instantiate(wave, new Vector3(transform.position.x, transform.position.y, -2), wave.transform.rotation);
            Wave waveScript = instanciateWave.GetComponent<Wave>();
            waveScript.timeLeft = timeSinceLastWave;
            timeSinceLastWave = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerVariables.instance.setPaused(!PlayerVariables.instance.getPaused());
        }

        if (Input.GetKeyDown(KeyCode.R) && PlayerVariables.instance.getPlayerCanMove())
        {
            PlayerVariables.instance.restartFromCheckpoint();
        }

        velocity.y += gravity * Time.deltaTime;

        if (PlayerVariables.instance.getPlayerCanMove())
        {
            if (velocity.x < 0)
                spriteRenderer.flipX = true;
            else if (velocity.x > 0)
                spriteRenderer.flipX = false;

            anim.SetFloat("xvelocity", velocity.x);
            anim.SetFloat("yvelocity", velocity.y);
            anim.SetBool("Jump", !controller.collisions.below);

            if (PlayerVariables.instance.getPlayerCanMove())
                controller.Move(velocity * Time.deltaTime, input);
        }
        else
        {
            velocity.x = 0;
            velocity.y = -0.8f;
            anim.SetFloat("xvelocity", velocity.x);
            anim.SetFloat("yvelocity", velocity.y);
        }

        if (controller.collisions.above || controller.collisions.below)
            velocity.y = 0;
    }
}
