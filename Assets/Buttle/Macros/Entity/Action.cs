class Action
{
	public Character actioner;
	// public Item item;
	public Skill skill;
	public Character targetAlly;
	public EnemyComponent targetEnemy;

	public Types actionType;

	public enum Types
	{
		Attack,
		Defence,
		Skill,
		Item
	}
}