using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClassManager : MonoBehaviour
{
    public CharacterClassENUM characterClassEnum;

    public CharacterClassENUM GetClass()
    {
        return characterClassEnum;
    }
}
