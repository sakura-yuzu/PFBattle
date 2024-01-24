using UnityEngine;
class EnemyClass : MonoBehaviour
{
	public ScriptableObject enemyData;

	public string name;
	public int hp;
	public int mp;
	public int attackPower;
	public int defensePower;

	public EnemyClass(Enemy _enemyData){
		name = _enemyData.name;
		hp = _enemyData.hp;
		mp = _enemyData.mp;
		attackPower = _enemyData.attackPower;
		defensePower = _enemyData.defensePower;
	}
}