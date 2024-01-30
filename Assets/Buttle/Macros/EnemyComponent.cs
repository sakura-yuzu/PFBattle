using UnityEngine;
using Cysharp.Threading.Tasks;
class EnemyComponent : MonoBehaviour
{
	public ScriptableObject enemyData;
	public string enemyName;
	public int hp;
	public int mp;
	public int attackPower;
	public int defensePower;

	public Animator anim;

	public void setEnemyData(Enemy _enemyData){
		enemyName = _enemyData.enemyName;
		hp = _enemyData.hp;
		mp = _enemyData.mp;
		attackPower = _enemyData.attackPower;
		defensePower = _enemyData.defensePower;
		// anim = gameObject.GetComponent<Animator>();
	}

	public async UniTask Damaged(){
		anim.SetBool("Damaged", true);
		await UniTask.DelayFrame(25);
		anim.SetBool("Damaged", false);
	}
}