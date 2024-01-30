using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Sprite;
// using WeaponTypeEnum;

[CreateAssetMenu]
[SerializeField]
public class Character : ScriptableObject
{
    public int id;                                      //ID
    public string characterName;
    public Sprite image;
    public int hp;
    public int mp;
    public int attackPower;
    public int defensePower;
    public int speed;

	public Sprite selectedImage;
	public Sprite deselectedImage;

    public List<Skill> skills;

}