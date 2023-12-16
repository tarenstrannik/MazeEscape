using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

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
    public void ConfigurePlayer(PlayerController character, InputSystemUIInputModule uiInputModule)
    {
        character.GetComponent<PlayerInput>().uiInputModule = uiInputModule;
    }

    public void ConfigureEnemy(EnemyController character, CharacterController target, float visibilityDistance, float visibilityAngle, float deltaAngle, float drawAndDamageDistance, float drawAndDamageAngle, float enemyDamage,float enemyDamageDelay, List<Vector3> enemyWaypoints, bool isFrontDamageLineFlat, bool isFrontViewLineFlat)
    {
        var enemyRaycasting = character.GetComponent<EnemyRaycasting>();
        enemyRaycasting.Target = target.transform.gameObject;
        enemyRaycasting.VisibilityDistance = visibilityDistance;
        enemyRaycasting.VisibilityAngle = visibilityAngle;
        enemyRaycasting.IsFrontViewLineFlat=isFrontViewLineFlat;

        enemyRaycasting.DrawAndDamageDistance = drawAndDamageDistance;
        enemyRaycasting.DeltaAngle = deltaAngle;
        enemyRaycasting.DrawAndDamageAngle = drawAndDamageAngle;
        enemyRaycasting.IsFrontDamageLineFlat = isFrontDamageLineFlat;

        character.EnemyDamage = enemyDamage;
        character.EnemyDamageDelay = enemyDamageDelay;
        character.GetComponent<EnemyMove>().PatrolingPoints = enemyWaypoints;
    }
}
