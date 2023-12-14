using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] protected Transform m_characterToFollow;

    [SerializeField] protected Vector3 m_deltaPosition;

    protected virtual void Update()
    {
        transform.position = m_characterToFollow.position+ m_deltaPosition;
    }
}
