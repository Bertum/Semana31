using UnityEngine;

public class BallController : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rigidbody;
    private GameController gameController;
    private float force;
    private bool pinCollision;
    public bool canMove;
    public float maxForce = 5000;
    public float minForce = 1000;

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
                force += 10;
            }

            if (Input.GetMouseButtonUp(0))
            {
                canMove = false;
                PushBall(Mathf.Clamp(force, minForce, maxForce));
                force = 0;
            }
        }
        CheckBallOutOfSCreen();
    }

    private void PushBall(float forceToApply)
    {
        this.transform.Rotate(transform.up, 10f);
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
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.Sleep();
        transform.position = initialPosition;
    }
}
