using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{
    [SerializeField] SavePanelManager _savePanelManager;
    [SerializeField] InputField _exampleInputField;
    [SerializeField] Dropdown _exampleDropdown;

    private void Awake()
    {
        RefreshProfileDropDown();
    }

    void RefreshProfileDropDown()
    {
        string[] profileList = ProfileManager.GetEveryProfiles();

        if (_exampleDropdown == null) return;

        //Clear and recreate drop down list
        _exampleDropdown.ClearOptions();
        List<Dropdown.OptionData> profileDropDownList = new List<Dropdown.OptionData>();
        foreach (string profile in profileList)
        {
            profileDropDownList.Add(new Dropdown.OptionData(profile));
        }
        _exampleDropdown.AddOptions(profileDropDownList);
    }

    public void CreateNewProfile()
    {
        if (_exampleInputField == null)
        {
            Debug.LogError("No linked input field");
            return;
        }
        

        ProfileManager.CreateProfile(_exampleInputField.text, true);
        RefreshProfileDropDown();

        if(_savePanelManager != null)
            _savePanelManager.RefreshAndCreateSavePanels();
    }
    public void ChangeProfile(string newProfile)
    {

        if (_savePanelManager != null)
            _savePanelManager.RefreshAndCreateSavePanels();
    }

    public void EraseProfile()
    {
        ProfileManager.EraseProfile(ProfileManager.GetCurrentProfile());
        RefreshProfileDropDown();

        if (_savePanelManager != null)
            _savePanelManager.RefreshAndCreateSavePanels();
    }
}
