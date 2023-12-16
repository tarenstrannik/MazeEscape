using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

public class CharacterGenerator
{


    public CharacterController GenerateCharacter(GameObject characterPrefab, Vector3 characterPosition, GameObject characterUIPrefab, Vector3 characterUIdelta,Transform parent)
    {
        var character = GameObject.Instantiate(characterPrefab, characterPosition, characterPrefab.transform.rotation).GetComponent<CharacterController>();
        character.transform.SetParent(parent);
        if (characterUIPrefab != null)
        {
            
            var characterUI = GameObject.Instantiate(characterUIPrefab, characterPosition + characterUIdelta, characterUIPrefab.transform.rotation).GetComponent<CharacterUI>();
            characterUI.transform.SetParent(parent);
            characterUI.CharacterToFollow = character.transform;
            characterUI.DelatPosition = characterUIdelta;
            character.m_damageRecieved.AddListener(characterUI.UpdateHealth);
        }
        

       
        return character;
    }
    public void ConfigureCharacter(CharacterController character, float characterMaxHealth, float chracterSpeed, float characterRotationSpeed)
    {
        character.MaxPersonHealth = characterMaxHealth;
        var moveController = character.GetComponent<MoveController>();
        if (moveController != null)
        {
            moveController.Speed = chracterSpeed;
            moveController.RotationSpeed = characterRotationSpeed;

        }
    }
    

    public void ConfigureEnemy(EnemyController character, CharacterController target, float visibilityDistance, float visibilityAngle, float deltaAngle, float drawAndDamageDistance, float drawAndDamageAngle, float enemyDamage,float enemyDamageDelay, List<Vector3> enemyWaypoints)
    {
        var enemyRaycasting = character.GetComponent<EnemyRaycasting>();
        enemyRaycasting.Target = target.transform.gameObject;
        enemyRaycasting.VisibilityDistance = visibilityDistance;
        enemyRaycasting.VisibilityAngle = visibilityAngle;

        enemyRaycasting.DrawAndDamageDistance = drawAndDamageDistance;
        enemyRaycasting.DeltaAngle = deltaAngle;
        enemyRaycasting.DrawAndDamageAngle = drawAndDamageAngle;

        character.EnemyDamage = enemyDamage;
        character.EnemyDamageDelay = enemyDamageDelay;
        character.GetComponent<EnemyMove>().PatrolingPoints = enemyWaypoints;
    }
}
