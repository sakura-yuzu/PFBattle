class Action
{
	private string skill;
	private string item;
	private string[] enemy;
	private string[] character;

	public Action(string skill, string item, string[] enemy, string[] character)
	{
		this.skill = skill;
		this.item = item;
		this.enemy = enemy;
		this.character = character;
	}
}