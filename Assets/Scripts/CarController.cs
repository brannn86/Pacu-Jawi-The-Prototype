using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float moveInput;
    private float turnInput;
    private bool isCarGrounded;

    public float airDrag;
    public float groundDrag;

    public Rigidbody sphere;
    public float forwardSpeed;
    public float reverseSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        sphere.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;

        transform.position = sphere.transform.position;
        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        transform.Rotate(0, newRotation, 0, Space.World);

        RaycastHit hit;
        isCarGrounded =  Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        if (isCarGrounded) {
            sphere.drag = groundDrag;
        } else {
            sphere.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        if (isCarGrounded) {
            sphere.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        } else {
            sphere.AddForce(transform.up * -30f);
        }
        
    }
}
