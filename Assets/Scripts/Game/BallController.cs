using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rigidbody;
    private GameController gameController;
    private float force;
    private bool pinCollision;
    public Slider powerSlider;
    private bool powerSelected;
    public bool canMove;
    public bool increase;
    public float maxForce = 4000;
    public float minForce = 1000;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        increase = true;
        powerSelected = false;
    }

    private void Start()
    {
        force = minForce;
        pinCollision = false;
        canMove = true;
        initialPosition = transform.position;
        powerSlider.value = minForce;
    }

    void Update()
    {
        if (!powerSelected)
        {
            //force += 10;
            //force = Mathf.Clamp(force, minForce, maxForce);
            Debug.Log("slider: " + powerSlider.value);
            Debug.Log("maxForce: " + maxForce);
            if (powerSlider.value >= maxForce) {
                Debug.Log("entro a superior");
                increase = false;
            } else if (powerSlider.value <= minForce)
            {
                Debug.Log("entro a inferior");
                increase = true;
            }
            powerSlider.value += increase ? 10 : -10 ;
            Debug.Log("slider: " + powerSlider.value);
        }
        if (canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                powerSelected = true;
                force = powerSlider.value;
                Debug.Log(force);
                PushBall(force);
            }
        }
        CheckBallOutOfSCreen();
    }

    private void PushBall(float forceToApply)
    {
        //this.transform.Rotate(transform.up, 10f);
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
