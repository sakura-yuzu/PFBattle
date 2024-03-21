using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Cysharp.Threading.Tasks;

class SettingSceneController : MonoBehaviour
{
	public Transform characterListArea;
	public CharacterDatabase characterDatabase;
	private CharacterManager characterManager;
	public Button gameStartButton;
	public List<MemberSelectButton> memberSelectButtons;
	public ToggleGroup memberSelectToggleGroup;
	public GameObject characterDetailPanel;
	public CharacterDetailPanelComponent characterDetailPanelComponent;
	public Button characterAssignButton;

	public SelectedAllyList selectedAllyListScriptableObject;

	public GameObject[] selectedMemberImageList;
	public List<Character> selectedMemberList;

	void Awake()
	{
		Prepare();
		characterAssignButton.onClick.AddListener(() =>
		{
			Character character = characterDetailPanelComponent.getCharacter();
			selectedMemberImageList[selectedMemberList.Count].GetComponent<MemberSelectButton>().setCharacter(character);
			selectedMemberList.Add(character);
		});
	}

	public async void Prepare()
	{
		characterDetailPanelComponent = characterDetailPanel.GetComponent<CharacterDetailPanelComponent>();

		characterManager = new CharacterManager(characterDatabase);
		await PrepareCharacterList(characterManager, characterListArea);

		gameStartButton.onClick.AddListener(() =>
		{
			// 命名が最悪すぎる
			selectedAllyListScriptableObject.selectedCharacterList = selectedMemberList;
			SceneManager.LoadScene("BattleScene");
		});

		foreach (MemberSelectButton memberSelectButton in memberSelectButtons)
		{
			memberSelectButton.onClick(onClickMemberSelectButton);
		}
	}

	public void onClickMemberSelectButton(Character character)
	{
		Debug.Log(character.displayName);
		characterDetailPanelComponent.setCharacter(character);
	}

	private async UniTask PrepareCharacterList(CharacterManager manager, Transform area)
	{
		var columnPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/UI/SettingSceneAllyButton.prefab").Task;

		List<Character> characterList = manager.getAll();

		foreach (Character chara in characterList)
		{
			GameObject column = Instantiate(columnPrefab, area, false);
			column.GetComponent<Toggle>().group = memberSelectToggleGroup;
			MemberSelectButton memberSelectButton = column.GetComponent<MemberSelectButton>();
			memberSelectButton.setCharacter(chara);
			memberSelectButtons.Add(memberSelectButton);
		}
	}
}
