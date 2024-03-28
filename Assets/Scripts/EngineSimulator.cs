using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class EngineSimulator : MonoBehaviour
{
    [SerializeField] private Slider _throttleSlider;
    [SerializeField] private TextMeshProUGUI _rpmText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _selectedGearText;
    [SerializeField] private TextMeshProUGUI _targetRpmText;
    [SerializeField] private Button _gearDownBtn;
    [SerializeField] private Button _gearUpBtn;

    private float _throttlePosition;
    private float _speed;
    private float _rpm;
    private float minRpm = 1150f;
    private float maxRpm = 6000f;
    private int _selectedGear;
    private float[] gearRatios = new float[] {3.166f, 1.882f, 1.296f, 0.972f, 0.78f};
    private float gearTotalTransmissionRatio = 1.1f;
    private float defaultrpmChangeSpeed = 400f;
    private float rpmChangeSpeed = 400f;

    public float ThrottlePosition => _throttlePosition;
    public float Rpm => _rpm;

    #region Mono
    private void Start()
    {
        _throttlePosition = 0f;
        _selectedGear = 0;
        _rpm = minRpm;
        rpmChangeSpeed = defaultrpmChangeSpeed * gearRatios[0];
        _selectedGearText.text = (_selectedGear + 1).ToString();

        _gearDownBtn.onClick.AddListener(OnGearDownPressed);
        _gearUpBtn.onClick.AddListener(OnGearUpPressed);
    }

    private void Update()
    {
        _throttlePosition = _throttleSlider.value * 2 - 1;

        float targetRPM = Mathf.Lerp(minRpm, maxRpm, _throttleSlider.value);
        _targetRpmText.text = $"Target: {Mathf.RoundToInt(targetRPM)} rpm";
        _rpm = Mathf.MoveTowards(_rpm, targetRPM, rpmChangeSpeed * Time.deltaTime);

        _rpmText.text = $"{Mathf.RoundToInt(_rpm)} rpm";
        _speed = (_rpm * (2 * Mathf.PI * 0.35f)) / (gearRatios[_selectedGear] * gearTotalTransmissionRatio * 60);
        _speedText.text = $"{Mathf.RoundToInt(_speed)} Km/h";
    }

    #endregion


    private void UpdateGear()
    {
        rpmChangeSpeed = defaultrpmChangeSpeed * gearRatios[_selectedGear];
        _rpm = (_speed * gearRatios[_selectedGear] * gearTotalTransmissionRatio * 60) / (2 * Mathf.PI * 0.30f);
        _selectedGearText.text = (_selectedGear + 1).ToString();
    }

    #region ButtonsBehavior
    private void OnGearDownPressed()
    {
        if (_selectedGear == 0) return;

        _selectedGear--;
        UpdateGear();
    }

    private void OnGearUpPressed()
    {
        if (_selectedGear == gearRatios.Length - 1) return;

        _selectedGear++;
        UpdateGear();
    }

    #endregion

}
