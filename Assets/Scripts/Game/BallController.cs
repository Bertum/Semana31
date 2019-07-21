using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rigidbody;
    private GameObject pointer;
    private GameController gameController;
    private LineRenderer lineRenderer;
    private float force;
    private bool pinCollision, directionSelected;
    public Slider powerSlider;
    private bool powerSelected;
    public bool canMove;
    public bool increase;
    public float maxForce = 4000;
    public float minForce = 1000;

    private void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        rigidbody = this.GetComponent<Rigidbody>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pointer = GameObject.FindGameObjectWithTag("Pointer");
        increase = true;
        powerSelected = false;
        directionSelected = false;
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        force = minForce;
        pinCollision = false;
        canMove = true;
        powerSlider.value = minForce;
    }

    void Update()
    {
        if (!powerSelected && directionSelected)
        {
            if (powerSlider.value >= maxForce)
            {
                increase = false;
            }
            else if (powerSlider.value <= minForce)
            {
                increase = true;
            }
            powerSlider.value += increase ? 10 : -10;
        }
        if (canMove && directionSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                powerSelected = true;
                canMove = false;
                force = powerSlider.value;
                PushBall(force);
            }
        }
        CheckBallOutOfSCreen();
        if (!directionSelected)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, pointer.transform.position);
            transform.LookAt(pointer.transform);
            if (Input.GetMouseButtonDown(0))
            {
                directionSelected = true;
            }
        }
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
        powerSelected = false;
        canMove = true;
        powerSlider.value = minForce;
        pinCollision = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.Sleep();
        this.transform.position = initialPosition;
        this.transform.rotation = initialRotation;
        directionSelected = false;
    }
}
