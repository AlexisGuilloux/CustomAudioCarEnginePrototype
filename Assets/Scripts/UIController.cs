using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _enginePresetsDropdown;
    [SerializeField] private Audio.EnginePreset[] _enginePresets;
    [SerializeField] private Button _loadBtn;
    [SerializeField] private Button _stopBtn;
    [SerializeField] private Audio.CarEngine _audioCarEngine;
    [SerializeField] private EngineSimulator _engineSimulator;

    private int _selectedPreset;

    // Start is called before the first frame update
    void Start()
    {
        _enginePresetsDropdown.options.Clear();

        for (int i = 0; i < _enginePresets.Length; i++)
        {
            
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = _enginePresets[i].name;
            _enginePresetsDropdown.options.Add(optionData);
        }

        _enginePresetsDropdown.onValueChanged.AddListener(OnValueChanged);
        _loadBtn.onClick.AddListener(OnLoadBtnClicked);
        _stopBtn.onClick.AddListener(OnStopBtnClicked);
    }

    private void OnValueChanged(int index)
    {
        _selectedPreset = index;
    }

    private void OnLoadBtnClicked()
    {
        _audioCarEngine.InitCarEngine(_enginePresets[_selectedPreset], _engineSimulator);
    }

    private void OnStopBtnClicked()
    {
        _audioCarEngine.UnloadCarEngine();
    }
}
