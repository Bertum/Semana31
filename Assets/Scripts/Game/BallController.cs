using UnityEngine;

public class BallController : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rigidbody;
    private GameController gameController;
    private float force;
    private bool pinCollision;
    public float MaxForce = 50;
    public bool canMove;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        pinCollision = false;
        canMove = true;
        initialPosition = transform.position;
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetMouseButton(0))
            {
                force += Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0))
            {
                canMove = false;
                PushBall(Mathf.Clamp(force, 0, MaxForce));
                force = 0;
            }
        }
        CheckBallOutOfSCreen();
    }

    private void PushBall(float forceToApply)
    {
        rigidbody.AddForce(transform.forward * forceToApply);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pin")
        {
            pinCollision = true;
            gameController.checkPins = true;
        }
    }

    private void CheckBallOutOfSCreen()
    {
        if (transform.position.y < -3 && !pinCollision)
        {
            gameController.checkPins = true;
        }
    }

    public void ResetBall()
    {
        pinCollision = false;
        transform.position = initialPosition;
    }
}
