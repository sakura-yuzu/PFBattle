using UnityEngine;
using UnityEngine.EventSystems;

public delegate void OnClickDelegate(Creature character);
class MemberSelectButton : BackgroundButton
{
	private Creature character;
	public void setCharacter(Creature _character)
	{
		character = _character;
		tmp.text = character.displayName;
	}

	public override void OnSelect(BaseEventData e)
	{
		base.OnSelect(e);
		if(onClickDelegate != null) // FIXME: 大丈夫か？
		{
			onClickDelegate(character);
		}
	}

	public OnClickDelegate onClickDelegate;

	public void onClick(OnClickDelegate _delegate)
	{
		onClickDelegate = _delegate;
	}
}