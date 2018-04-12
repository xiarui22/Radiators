using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public enum CharacterType { CIVILIAN, RADIATED_PERSON, GUARD, RADIATED_PRISONER, TECHNICIAN, OTHER };

public enum BigRadState { INACTIVE, MOVING_TO_HELP_OTHERS, HELPING_OTHERS , ESCAPING, ATTACKING };

public enum TechnicianState { IDLE, TASK, RUN };

public enum CivilianState { IDLE, RUN };

public class Brain : MonoBehaviour {

    public CharacterType currentCharacter;

    // Target Locations
    public Transform GeneratorRoom;
    public Transform Entrance;
    public Transform Exit;
    public Transform Prison;
    public Transform RadiationRoom;
    public Transform MedicalRoom;

    public LayerMask RadiatedLayerMask;
    public LayerMask NormalPeopleLayerMask;

    public float BigRadDelay = 20.0f;

    public float bigRadSpeed;


    [Space(20)]
    [Header("Set these variables as the game goes on..")]
    public bool noPower = false;
    public bool isFree = true;


    public bool escaped = false;

    NavMeshAgent navMeshAgent;
    bool isMoving;

    [SerializeField]
    float guardSensorRadius = 150.0f;

    [SerializeField]
    Vector3 target;

    private Characters c;

    [SerializeField]
    GameObject rayHitFirstObj;

    // private List<GameObject> gameObjects = new List<GameObject>();
    
    GuardAttack gAttack;
    RadiatorAttack rAttack;
    Collider[] tempColliders;
    Collider[] newTempColliders;


    /* Variables for the Guard shooting the RadGuy */
    Vector3 tempGuardPos;
    Vector3 shootDirection;
    private GameObject targetForGuard;
    float distance;
    RaycastHit[] hits;


    /* Variables for the Rad Shooting */
    private GameObject primaryTargetForRadiator;
    private GameObject secondaryTargetForRadiator;
    [SerializeField]
    private bool isRadShooting;

    // Variables for Big Rad 20 seconds Delay.
    public BigRadState bigRadState;
    [SerializeField]
    float timeElapsed = 0.0f;

    // Variables for the Technician State
    public TechnicianState techState;
    public float CivilianRadius = 150.0f;
    private GameObject runFromObj;

    // Variables for the Civilian State
    public CivilianState civState;

    // Variables for freeing the prisoners.
    public LayerMask PrisonFreeTriggerLayer;

    [SerializeField]
    private Vector3 navMeshDestination;

    [SerializeField]
    float stopDist = 5.0f;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stopDist;
        bigRadSpeed = navMeshAgent.speed;
    }

    // Use this for initialization
    void Start ()
    {
        foreach (Transform t in GameObject.FindGameObjectWithTag("targetPositions").transform)
        {
            if (t.name == "GeneratorRoom")
            {
                GeneratorRoom = t;
            }
            else if (t.name == "PrisonRoom")
            {
                Prison = t;
            }
            else if (t.name == "ExitRoom")
            {
                Exit = t;
            }
            else if (t.name == "RadiationRoom")
            {
                RadiationRoom = t;
            }
            else if (t.name == "Entrance")
            {
                Entrance = t;
            }
            else if (t.name == "HospitalRoom")
            {
                MedicalRoom = t;
            }
        }

        switch (currentCharacter)
        {
            case CharacterType.CIVILIAN:
                civState = CivilianState.IDLE;
                break;

            case CharacterType.RADIATED_PRISONER:
                rAttack = GetComponent<RadiatorAttack>();
                break;

            case CharacterType.RADIATED_PERSON:
                rAttack = GetComponent<RadiatorAttack>();
                break;

            case CharacterType.TECHNICIAN:
                break;
            case CharacterType.GUARD:
                gAttack = GetComponent<GuardAttack>();
                break;
        }

    }

    // Update is called once per frame
    void Update () {
        //temp for test
        navMeshAgent.stoppingDistance = stopDist;
        //---------------------------------------

        primaryTargetForRadiator = null;
        secondaryTargetForRadiator = null;
        runFromObj = null;
        targetForGuard = null;

        if ( rAttack != null)
        {
            rAttack.mainTarget = null;
            rAttack.sideTarget = null;
        }

        if (CharacterType.RADIATED_PERSON == currentCharacter )
        {
            if ( timeElapsed >= BigRadDelay && bigRadState == BigRadState.INACTIVE)
            {
                bigRadState = BigRadState.MOVING_TO_HELP_OTHERS;
                GetComponent<Brain>().isFree = true;
                target = Prison.transform.position;
            } else if ( timeElapsed < BigRadDelay && bigRadState == BigRadState.INACTIVE)
            {
                timeElapsed += Time.deltaTime;
            }

            if (bigRadState == BigRadState.MOVING_TO_HELP_OTHERS && target != Prison.transform.position)
            {
                target = Prison.transform.position;
            }

            // Free the prisoners.
            if ( bigRadState == BigRadState.HELPING_OTHERS)
            {
                // Get the prison free triggers in his vicinity.
                tempColliders = Physics.OverlapSphere(transform.position, rAttack.attackRange*4, PrisonFreeTriggerLayer);
                if ( tempColliders.Length > 0 )
                {
                    if ((target != tempColliders[0].transform.position))
                        target = tempColliders[0].gameObject.transform.position;
                }
                else
                {
                    bigRadState = BigRadState.ESCAPING;
                }

            }

            if ( bigRadState == BigRadState.ESCAPING)
            {
                target = Exit.position;
            }

            if ( escaped && (bigRadState == BigRadState.HELPING_OTHERS))
            {
                target = Exit.position;
                bigRadState = BigRadState.ESCAPING;
            }
        }

        if (CharacterType.RADIATED_PRISONER == currentCharacter && isFree)
        {
            if (bigRadState == BigRadState.ESCAPING)
            {
                target = Exit.position;
            }
        }

        // Logic for running away.
        if ( (CharacterType.CIVILIAN == currentCharacter && civState == CivilianState.IDLE) || (CharacterType.TECHNICIAN == currentCharacter && techState == TechnicianState.IDLE))
        {
            tempColliders = Physics.OverlapSphere(transform.position, CivilianRadius, RadiatedLayerMask);
            if (tempColliders.Length >= 1)
            {
                foreach (Collider tempCollider in tempColliders)
                {
                    shootDirection = (tempCollider.transform.position - transform.position).normalized;
                    distance = Vector3.Distance(transform.position, tempCollider.transform.position);
                    hits = Physics.RaycastAll(transform.position, shootDirection, distance).OrderBy(h=>h.distance).ToArray();

                    // Loop until you find a rad with no obstruction in the middle.

                    if (hits.Length <= 0 || ((RadiatedLayerMask & 1 << hits[0].transform.gameObject.layer) == 0))
                    {
                        runFromObj = null;
                        continue;
                    }

                    // Target only free rads.
                    if (tempCollider.gameObject.GetComponent<Brain>().isFree)
                    {
                        runFromObj = tempCollider.gameObject;
                        break;
                    }
                }

                if (null != runFromObj && null != runFromObj.transform)
                {
                    target = transform.position + (-2 * shootDirection * distance);
                    techState = TechnicianState.RUN;
                    civState = CivilianState.RUN;
                }
            }
        }

        // Guard Attack
        if (CharacterType.GUARD == currentCharacter)
        {
            // Check for the surroundings to see any idiots radiated and running around.
            tempColliders = Physics.OverlapSphere(transform.position, gAttack.attackRange, RadiatedLayerMask);
            if (tempColliders.Length >= 1)
            {
                foreach (Collider tempCollider in tempColliders)
                {
                    shootDirection = (tempCollider.transform.position - transform.position).normalized;
                    distance = Vector3.Distance(transform.position, tempCollider.transform.position);
                    hits = Physics.RaycastAll(transform.position, shootDirection, distance).OrderBy(h => h.distance).ToArray();

                    // Loop until you find a rad with no obstruction in the middle.

                    if (hits.Length <= 0 || (hits[0].transform.gameObject != tempCollider.gameObject))
                    {
                        targetForGuard = null;
                        continue;
                    }

                    // Target only free rads.
                    if (tempCollider.gameObject.GetComponent<Brain>().isFree)
                    {
                        targetForGuard = tempCollider.gameObject;
                    }
                }

                if (null != targetForGuard && null != targetForGuard.GetComponent<GeneralRadiator>())
                {
                    // Check if the object still exists.
                    if (null != targetForGuard.transform)
                    {
                        gAttack.target = targetForGuard.GetComponent<GeneralRadiator>();
                        if (gAttack.IsReadyToAttack) {
                            gAttack.Attack();
                        }                        
                    }
                }
            }
        }

        // Radiators Attack.
        if ((CharacterType.RADIATED_PERSON == currentCharacter && bigRadState != BigRadState.INACTIVE) || ( CharacterType.RADIATED_PRISONER == currentCharacter && bigRadState != BigRadState.INACTIVE))
        {
            tempColliders = Physics.OverlapSphere(transform.position, rAttack.attackRange, NormalPeopleLayerMask);

            if (tempColliders.Length >= 1)
            {
                foreach (Collider tempCollider in tempColliders)
                {
                    shootDirection = (tempCollider.transform.position - transform.position).normalized;
                    distance = Vector3.Distance(transform.position, tempCollider.transform.position);
                    hits = Physics.RaycastAll(transform.position, shootDirection, distance).OrderBy(h => h.distance).ToArray();

                    // Loop until you find a rad with no obstruction in the middle.
                    if ( hits.Length <= 0 || ( hits[0].transform.gameObject != tempCollider.gameObject))
                    {
                        rayHitFirstObj = hits[0].transform.gameObject;
                        primaryTargetForRadiator = null;
                        continue;
                    }

                    rayHitFirstObj = hits[0].transform.gameObject;
                    primaryTargetForRadiator = tempCollider.gameObject;
                    isRadShooting = true;

                    // Banking on the fact that this doesn't happen usually when the player is playing.
                    break;
                }

                if (null != primaryTargetForRadiator && null != primaryTargetForRadiator.transform)
                {
                    newTempColliders = Physics.OverlapSphere(primaryTargetForRadiator.transform.position, rAttack.attackRange, NormalPeopleLayerMask);

                    if (newTempColliders.Length >= 1)
                    {
                        foreach (Collider newTempCollider in newTempColliders)
                        {
                            shootDirection = -1 * (primaryTargetForRadiator.transform.position - newTempCollider.transform.position).normalized;
                            distance = Vector3.Distance(primaryTargetForRadiator.transform.position, newTempCollider.transform.position);
                            hits = Physics.RaycastAll(primaryTargetForRadiator.transform.position, shootDirection, distance).OrderBy(h => h.distance).ToArray();

                            if (hits.Length <= 0 || (hits[0].transform.gameObject != newTempCollider.gameObject))
                            {
                                continue;
                            }

                            secondaryTargetForRadiator = newTempCollider.gameObject;
                            break;
                        }
                    }

                    if (null != primaryTargetForRadiator && null != primaryTargetForRadiator.GetComponent<GeneralPeople>())
                    {
                        rAttack.mainTarget = primaryTargetForRadiator.GetComponent<GeneralPeople>();
                        if (null != secondaryTargetForRadiator && null != secondaryTargetForRadiator.GetComponent<GeneralPeople>())
                        {
                            rAttack.sideTarget = secondaryTargetForRadiator.GetComponent<GeneralPeople>();
                        }
                        if ( rAttack.IsReadyToAttack)
                        {
                            rAttack.Attack();
                        }
                    }
                }
            }

            if ( null == primaryTargetForRadiator || null == primaryTargetForRadiator.transform)
            {
                isRadShooting = false;
            }

        }

        if ( (currentCharacter == CharacterType.RADIATED_PERSON || currentCharacter == CharacterType.RADIATED_PRISONER) && isRadShooting)
        {
            navMeshAgent.speed = 0;
        }
        else
        {
            navMeshAgent.speed = bigRadSpeed;
        }

        navMeshDestination = navMeshAgent.destination;

        distance = Vector3.Distance(target, new Vector3(0.0f, 0.0f, 0.0f));
        if (target != null && ( distance != 0 ) && (target.x != navMeshAgent.destination.x && target.z != navMeshAgent.destination.z))
        {
            navMeshAgent.SetDestination(target);
        }
    }

    bool nearTheTarget(Transform targetPosition)
    {
        if (Vector3.Distance(transform.position, targetPosition.position) <= 1.0f)
            return true;

        return false;
    }

    public void SetTarget(Vector3 targetPosition)
    {
        target = targetPosition;
    }
}
