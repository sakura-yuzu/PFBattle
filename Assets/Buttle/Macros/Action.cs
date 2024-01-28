class Action
{
	public Character actioner;
	public Item item;
	public Skill skill;
	public Character targetAlly;
	public EnemyClass targetEnemy;

	public Types actionType;

	public enum Types
	{
		Attack,
		Defence,
		Skill,
		Item
	}
}