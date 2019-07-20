using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private float force;
    public float maxForce = 5000;
    public float minForce = 1000;
    public bool canMove = false;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetMouseButton(0))
            {
                force+=10;
            }

            if (Input.GetMouseButtonUp(0))
            {
                canMove = false;
                PushBall(Mathf.Clamp(force, minForce, maxForce));
                force = 0;
            }
        }
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
            //Activate timer and check if pins are moving??
        }
    }


}
