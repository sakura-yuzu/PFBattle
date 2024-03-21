using UnityEngine;
using UnityEngine.EventSystems;

public delegate void OnClickDelegate(Character character);
class MemberSelectButton : BaseButton
{
	private Character character;
	public void setCharacter(Character _character)
	{
		character = _character;
		backgroundManager.setData(character.selectedImage, character.deselectedImage, mountedButton);
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