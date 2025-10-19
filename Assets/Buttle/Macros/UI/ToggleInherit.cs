using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

class ToggleInherit : Toggle
{
    public string value;
    public Creature creature;
    public SkillSetting skill;
    // public Item item;



    public void SetObject(Creature creature)
    {
        value = creature.displayName;
        this.creature = creature;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = value;
    }
    public void SetObject(SkillSetting skill)
    {
        value = skill.skillName;
        this.skill = skill;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = value;
    }
    // public void SetObject(Item item) {
    //     value = item.itemName;
    //     this.item = item;
    // }

    public T GetObject<T>() where T : class
    {
        if (typeof(T) == typeof(Creature))
        {
            return creature as T;
        }
        if (typeof(T) == typeof(SkillSetting))
        {
            return skill as T;
        }
        return null;
    }

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