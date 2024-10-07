using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characters;  
    private GameObject selectedCharacter;
    public GameObject selectionIndicatorPrefab;
    private GameObject currentIndicator;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (IsCharacter(clickedObject))
                {
                    SelectCharacter(clickedObject);
                }
            }
        }
    }

    
    bool IsCharacter(GameObject obj)
    {
        foreach (var character in characters)
        {
            if (obj == character)
            {
                return true;
            }
        }
        return false;
    }

    
    void SelectCharacter(GameObject character)
    {
        
        if (selectedCharacter != null && selectedCharacter != character)
        {
            var mover = selectedCharacter.GetComponent<CharacterMover>();

            
            if (!mover.isAttachedToTreasure)
            {
                Destroy(currentIndicator);
            }
        }

        // Select the new character
        selectedCharacter = character;


        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }

        Vector3 indicatorPosition = selectedCharacter.transform.position + new Vector3(0, 1.5f, 0);
        currentIndicator = Instantiate(selectionIndicatorPrefab, indicatorPosition, Quaternion.identity);

        currentIndicator.transform.SetParent(selectedCharacter.transform);
    }

    
    public GameObject GetSelectedCharacter()
    {
        return selectedCharacter;
    }
}
