using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{
    [SerializeField] SavePanelManager _savePanelManager;
    [SerializeField] InputField _exampleInputField;
    [SerializeField] Dropdown _exampleDropdown;

    private void Start()
    {
        if (!SaveSettingsManager.UseProfiles())
        {
            gameObject.SetActive(false);
            return;
        }

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
            var dirName = new DirectoryInfo(profile).Name;
            profileDropDownList.Add(new Dropdown.OptionData(dirName));
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

        string newProfileName = _exampleInputField.text;

        ProfileManager.CreateProfile(newProfileName);
        ProfileManager.ChangeProfile(newProfileName);

        RefreshProfileDropDown();
        _exampleDropdown.value = _exampleDropdown.options.FindIndex((i) => { return i.text.Equals(newProfileName); });

        if(_savePanelManager != null)
            _savePanelManager.RefreshAndCreateSavePanels();
    }
    public void ChangeProfile(string newProfile)
    {
        ProfileManager.ChangeProfile(newProfile);

        if (_savePanelManager != null)
            _savePanelManager.RefreshAndCreateSavePanels();
    }

    public void ChangeProfileFromDropdown()
    {
        if (_exampleDropdown == null)
        {
            Debug.LogError("Can't find linked dropdown");
            return;
        }
        ChangeProfile(_exampleDropdown.options[_exampleDropdown.value].text);
    }

    public void EraseProfile()
    {
        ProfileManager.EraseProfile(ProfileManager.GetCurrentProfile());
        RefreshProfileDropDown();

        //Change to the first profile
        if(_exampleDropdown.options.Count > 0)
            ProfileManager.ChangeProfile(_exampleDropdown.options[0].text);

        if (_savePanelManager != null)
            _savePanelManager.RefreshAndCreateSavePanels();
    }
}
