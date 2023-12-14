using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

public class CharacterGenerator
{
   


    public CharacterController GenerateCharacter(GameObject characterPrefab, Vector3 characterPosition, GameObject characterUIPrefab, Vector3 characterUIdelta)
    {
        var character = GameObject.Instantiate(characterPrefab, characterPosition, characterPrefab.transform.rotation).GetComponent<CharacterController>();

        if (characterUIPrefab != null)
        {
            
            var characterUI = GameObject.Instantiate(characterUIPrefab, characterPosition + characterUIdelta, characterUIPrefab.transform.rotation).GetComponent<CharacterUI>();
            
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
    public void ConfigurePlayer(CharacterController character, GameManager gameManager)
    {
        character.m_death.AddListener(gameManager.GameOver);
    }

    public void ConfigureEnemy(EnemyController character, CharacterController player, float visibilityDistance, float visibilityAngle, float enemyDamage,float enemyDamageDelay)
    {
        character.Player = player.transform.gameObject;
        character.VisibilityDistance = visibilityDistance;
        character.VisibilityAngle = visibilityAngle;
        character.EnemyDamage = enemyDamage;
        character.EnemyDamageDelay = enemyDamageDelay;
    }
}
