using UnityEngine;
using UnityEngine.AI;

public class CharacterMover : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Collider playerCollider;
    public GameObject moveIndicatorPrefab;
    public GameObject treasureIndicatorPrefab;
    private GameObject moveIndicator;
    private GameObject treasureIndicator;

    public string characterType;
    public bool isAttachedToTreasure = false;
    private Treasure currentTreasure;

    private Vector3 originalScale;
    public float stopDistance = 1.5f;

    public enum CharacterState
    {
        Idle,
        Active,
        Carrying,
        TryingToCarry
    }

    public CharacterState currentState = CharacterState.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        originalScale = transform.localScale;

        if (characterType == "Fire")
        {
            agent.areaMask = (1 << NavMesh.GetAreaFromName("Fire Area")) | (1 << NavMesh.GetAreaFromName("Walkable"));
        }
        else if (characterType == "Water")
        {
            agent.areaMask = (1 << NavMesh.GetAreaFromName("Water Area")) | (1 << NavMesh.GetAreaFromName("Walkable"));
        }
        else if (characterType == "Electric")
        {
            agent.areaMask = (1 << NavMesh.GetAreaFromName("Electric Area")) | (1 << NavMesh.GetAreaFromName("Walkable"));
        }
    }

    void Update()
    {
        GameObject selectedCharacter = FindObjectOfType<CharacterSelector>().GetSelectedCharacter();

        if (selectedCharacter == null) return;

        if (selectedCharacter == this.gameObject && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                Treasure clickedTreasure = clickedObject.GetComponent<Treasure>();
                if (clickedTreasure != null)
                {
                    MoveToTreasure(clickedTreasure);
                }
                else
                {
                    MoveToGround(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                Treasure clickedTreasure = clickedObject.GetComponent<Treasure>();
                if (clickedTreasure != null)
                {
                    clickedTreasure.DetachAllPlayers();
                }
            }
        }

        if (currentTreasure != null && !isAttachedToTreasure)
        {
            float distanceToTreasure = Vector3.Distance(transform.position, currentTreasure.transform.position);
            if (distanceToTreasure <= stopDistance)
            {
                AttachToTreasure();
            }
        }

        if (agent.enabled && agent.isOnNavMesh && !agent.pathPending)
        {
            if (moveIndicator != null && agent.remainingDistance <= agent.stoppingDistance)
            {
                Destroy(moveIndicator);
            }
        }
    }

    void MoveToTreasure(Treasure treasure)
    {
        currentTreasure = treasure;

        if (moveIndicator != null)
        {
            Destroy(moveIndicator);
        }

        ShowTreasureIndicator(currentTreasure.transform.position);

        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(currentTreasure.transform.position);
            agent.isStopped = false;
        }
    }

    void MoveToGround(Vector3 position)
    {
        if (moveIndicator != null)
        {
            Destroy(moveIndicator);
        }

        moveIndicator = Instantiate(moveIndicatorPrefab, position, Quaternion.identity);

        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(position);
            agent.isStopped = false;
        }
    }

    void ShowTreasureIndicator(Vector3 position)
    {
        Vector3 indicatorPosition = position + new Vector3(0, 2.0f, 0);
        if (treasureIndicator != null)
        {
            Destroy(treasureIndicator);
        }
        treasureIndicator = Instantiate(treasureIndicatorPrefab, indicatorPosition, Quaternion.identity);
    }

    public void AttachToTreasure()
    {
        isAttachedToTreasure = true;

        agent.isStopped = true;
        agent.enabled = false;
        rb.isKinematic = true;

        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        currentTreasure.AttachCharacter(this.gameObject);

        transform.SetParent(currentTreasure.transform, true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = originalScale;

        if (treasureIndicator != null)
        {
            Destroy(treasureIndicator);
        }

        ChangeState(CharacterState.Carrying);
    }

    public void DetachFromTreasure()
    {
        isAttachedToTreasure = false;

        agent.enabled = true;
        rb.isKinematic = false;

        currentTreasure.DetachCharacter(this.gameObject);
        transform.SetParent(null);

        transform.localScale = originalScale;
        transform.localRotation = Quaternion.identity;

        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }

        currentTreasure = null;

        ChangeState(CharacterState.Idle);
    }

    public void ChangeState(CharacterState newState)
    {
        currentState = newState;
    }
}
