class Action
{
	public Character actioner;
	public Item item;
	public Skill skill;
	public Character targetAlly;
	public Enemy targetEnemy;

	public enum Types
	{
		Attack,
		Defence,
		Skill,
		Item
	}
}