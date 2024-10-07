using UnityEngine;
using UnityEngine.AI;

public class TreasureAreaRestriction : MonoBehaviour
{
    private NavMeshAgent agent;
    private Treasure treasure;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        treasure = GetComponent<Treasure>();
    }

    void Update()
    {
        if (treasure.isBeingCarried)
        {
            SetAreaMask();
        }
    }

    void SetAreaMask()
    {
        int allowedAreas = 0;

        // Use the public getter method to access the attached characters from the Treasure script
        foreach (GameObject character in treasure.GetAttachedCharacters())
        {
            CharacterMover mover = character.GetComponent<CharacterMover>();
            if (mover.characterType == "Fire")
            {
                allowedAreas |= 1 << NavMesh.GetAreaFromName("Fire Area");
            }
            else if (mover.characterType == "Water")
            {
                allowedAreas |= 1 << NavMesh.GetAreaFromName("Water Area");
            }
            else if (mover.characterType == "Electric")
            {
                allowedAreas |= 1 << NavMesh.GetAreaFromName("Electric Area");
            }
        }

        // Always allow movement on the "Walkable" areas
        allowedAreas |= 1 << NavMesh.GetAreaFromName("Walkable");
        agent.areaMask = allowedAreas;
    }
}
