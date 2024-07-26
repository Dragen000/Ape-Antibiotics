using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    private DoubleJump dj;
    private Rigidbody rb;
    public Transform cam;
    public Transform shootPoint;
    public LayerMask whatIsGrappable;
    public LineRenderer lr;
    public Animator playerAnimator;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] GameObject grappleGun;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public Animator grappleAnimator;

    private Vector3 grapplePoint;
    private SpringJoint joint;

    [SerializeField] float maxJointDistance = 0.8f;
    [SerializeField] float minJointDistance = 0.25f;
    [SerializeField] float jointSpringValue = 4.5f;
    [SerializeField] float jointDamperValue = 7f;
    [SerializeField] float jointMassScaleValue = 4.5f;


    [Header("Cooldown")]
    [Tooltip("The cooldown time of this ability.")]
    public float GrapplingCd;
    
    [SerializeField] [Tooltip("DEBUG - TIME LEFT")]
    private float grapplingCdTimeLeft;

    [Tooltip("Reference to the CooldownManager.")]
    public CooldownManager cooldownManager; // Reference to the CooldownManager
    
    [Tooltip("Index of the cooldown icon in CooldownManager.")]
    public int cooldownIconIndex = 0; // Index of the cooldown icon in CooldownManager

    //storing private variable to hold animator. 
    private Animator animator;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.R;

    public bool grappling;
    private bool canGrapple = true;

    private RaycastHit savedHit;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        dj = GetComponent<DoubleJump>();
        rb = GetComponent<Rigidbody>();
        grappleGun.SetActive(false);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && canGrapple)
        {
            StartGrapple();
        }
        if (Input.GetMouseButtonUp(1) && canGrapple && lr.enabled)
        {
            StopGrapple();
        }
    }

    void LateUpdate()
    {
        if (grappling)
        {
            setGrappleRope(savedHit);
        }
    }

    private void StartGrapple()
    {
        Debug.Log("Grapple");

        grappling = true;

        //grapple animation triggered. 
        grappleGun.SetActive(true);
        grappleAnimator.SetTrigger("GrappleTriggerPull");
        playerAnimator.SetTrigger("Grapple");

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, maxGrappleDistance, whatIsGrappable))
        {
            Debug.Log($"We hit {hit.transform.name}");
            savedHit = hit;
            ExecuteGrapplePhysics(savedHit);

            //starting holding animation for grapple. 
            playerAnimator.SetBool("Grappling", true);
            grappleAnimator.SetBool("IsGrappleHolding", true);

        }
        else
        {
            playerAnimator.SetBool("Grappling", false);
            grappleAnimator.SetBool("IsGrappleHolding", false);
            return;
        }

        lr.enabled = true;
        // gun needs either linked or matching animation. TODO

    }

    private IEnumerator GrappleCooldown()
    {
        cooldownManager.TriggerCooldown(cooldownIconIndex,GrapplingCd);
        while (grapplingCdTimeLeft > 0) // Perform the cooldown
        {
            grapplingCdTimeLeft -= Time.deltaTime;
            yield return null;
        }
        canGrapple = true;
    }

    // change to fixed update
    private void ExecuteGrapplePhysics(RaycastHit hit)
    {

        Vector3 grapplePoint = hit.point;

        joint = pm.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;
        //joint.connectedBody = rb;

        float distanceFromPoint = Vector3.Distance(a: pm.gameObject.transform.position, b: grapplePoint);

        // The distance grapple will try to keep from grapple point. mess with this
        joint.maxDistance = distanceFromPoint * maxJointDistance;
        joint.minDistance = distanceFromPoint * minJointDistance;

        // Change these values until good fit
        joint.spring = jointSpringValue; // higher spring = more pull/push
        joint.damper = jointDamperValue;
        joint.massScale = jointMassScaleValue;

        // Let player double jump again
        dj.groundCheck.gameObject.SetActive(true);
    }

    private void StopGrapple()
    {
        grappling = false;
        grappleAnimator.SetBool("IsGrappleHolding", false);
        grappleGun.SetActive(false);

        //ending animation hold for grapple. 
        animator.SetBool("Grappling", false);

        lr.enabled = false;
        Destroy(joint);

        canGrapple = false;

        grapplingCdTimeLeft = GrapplingCd;

        StartCoroutine(GrappleCooldown());
    }

    private void setGrappleRope(RaycastHit hit)
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = shootPoint.position;
        positions[1] = hit.point;
        lr.SetPositions(positions);
    }
}
