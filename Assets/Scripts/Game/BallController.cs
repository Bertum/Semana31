using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private float force;
    public float MaxForce = 50;
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
                force += Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0))
            {
                canMove = false;
                PushBall(Mathf.Clamp(force, 0, MaxForce));
                force = 0;
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
            //Activate timer and check if pins are moving??
        }
    }


}
