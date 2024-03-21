using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CharacterManager
{
    protected CharacterDatabase characterDatabase;

    public CharacterManager(CharacterDatabase _characterDatabase){
        characterDatabase = _characterDatabase;
    }

    public void AddCharacterData(Character character)
    {
        characterDatabase.characterList.Add(character);
    }

    public List<Character> getAll()
    {
        return characterDatabase.characterList;
    }
}
