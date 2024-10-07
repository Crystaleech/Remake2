using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Treasure : MonoBehaviour
{
    public int requiredCharacters = 2;
    private HashSet<GameObject> attachedCharacters = new HashSet<GameObject>();
    public bool isBeingCarried = false;

    private NavMeshAgent navMeshAgent;
    public Transform homeBase;

    private GoalArea goalArea;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
        goalArea = FindObjectOfType<GoalArea>();
    }

    void Update()
    {
        attachedCharacters.RemoveWhere(character => character == null);

        if (attachedCharacters.Count >= requiredCharacters)
        {
            if (AreBothPlayersSameColor())
            {
                if (!navMeshAgent.enabled)
                {
                    navMeshAgent.enabled = true;
                    SetDestination(homeBase.position);
                }

                AdjustAreaMaskBasedOnPlayerColor();
                isBeingCarried = true;
            }
            else
            {
                StopTreasureMovement();
            }
        }
        else
        {
            isBeingCarried = false;
        }

        UpdateCharacterState();
    }

    public void AttachCharacter(GameObject character)
    {
        if (character != null && attachedCharacters.Add(character))
        {
            character.transform.SetParent(this.transform);
        }
    }

    public void DetachCharacter(GameObject character)
    {
        if (character != null && attachedCharacters.Remove(character))
        {
            character.transform.SetParent(null);
        }

        if (attachedCharacters.Count < requiredCharacters)
        {
            navMeshAgent.enabled = false;
        }
    }

    public void DetachAllPlayers()
    {
        List<GameObject> playersToDetach = new List<GameObject>(attachedCharacters);

        foreach (GameObject character in playersToDetach)
        {
            if (character != null)
            {
                character.GetComponent<CharacterMover>().DetachFromTreasure();
            }
        }

        attachedCharacters.Clear();
        navMeshAgent.enabled = false;
    }

    public HashSet<GameObject> GetAttachedCharacters()
    {
        return attachedCharacters;
    }

    void SetDestination(Vector3 destination)
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(destination);
        }
    }

    void UpdateCharacterState()
    {
        foreach (GameObject character in attachedCharacters)
        {
            if (character != null)
            {
                var mover = character.GetComponent<CharacterMover>();
                if (attachedCharacters.Count >= requiredCharacters)
                {
                    mover.ChangeState(CharacterMover.CharacterState.Carrying);
                }
                else
                {
                    mover.ChangeState(CharacterMover.CharacterState.TryingToCarry);
                }
            }
        }
    }

    // Check if both attached players have the same color
    bool AreBothPlayersSameColor()
    {
        if (attachedCharacters.Count == requiredCharacters)
        {
            GameObject[] characters = new GameObject[attachedCharacters.Count];
            attachedCharacters.CopyTo(characters);

            var color1 = characters[0].GetComponent<CharacterMover>().characterType;
            var color2 = characters[1].GetComponent<CharacterMover>().characterType;

            return color1 == color2;
        }
        return false;
    }

    // Adjust the NavMesh area mask based on the color of the attached players
    void AdjustAreaMaskBasedOnPlayerColor()
    {
        GameObject[] characters = new GameObject[attachedCharacters.Count];
        attachedCharacters.CopyTo(characters);

        var characterType = characters[0].GetComponent<CharacterMover>().characterType;

        if (characterType == "Fire")
        {
            navMeshAgent.areaMask = (1 << NavMesh.GetAreaFromName("Fire Area")) | (1 << NavMesh.GetAreaFromName("Walkable"));
        }
        else if (characterType == "Water")
        {
            navMeshAgent.areaMask = (1 << NavMesh.GetAreaFromName("Water Area")) | (1 << NavMesh.GetAreaFromName("Walkable"));
        }
        else if (characterType == "Electric")
        {
            navMeshAgent.areaMask = (1 << NavMesh.GetAreaFromName("Electric Area")) | (1 << NavMesh.GetAreaFromName("Walkable"));
        }
    }

    // Stops treasure movement if conditions aren't met (e.g., players are not the same color)
    void StopTreasureMovement()
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.isStopped = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (navMeshAgent.enabled && other.gameObject.CompareTag("HomeBase") && goalArea != null)
        {
            goalArea.CollectTreasure();
            DetachAllPlayers();
            Destroy(gameObject);
        }
    }
}
