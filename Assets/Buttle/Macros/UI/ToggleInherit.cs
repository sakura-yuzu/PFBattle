using UnityEngine.EventSystems;
using UnityEngine.UI;
class ToggleInherit : Toggle
{
    public string value;
    public Creature creature;
    public SkillSetting skill;
    // public Item item;

    public void SetObject(Creature creature) {
        value = creature.displayName;
        this.creature = creature;
    }
    public void SetObject(SkillSetting skill) {
        value = skill.skillName;
        this.skill = skill;
    }
    // public void SetObject(Item item) {
    //     value = item.itemName;
    //     this.item = item;
    // }

    public void SetValue(string newValue)
    {
        value = newValue;
    }

    public string GetValue()
    {
        return value;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        // Additional logic can be added here if needed
    }
}