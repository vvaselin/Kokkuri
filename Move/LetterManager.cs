using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    public string character = default;
    public string GetCharacter()
    {
        return character;
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

}
