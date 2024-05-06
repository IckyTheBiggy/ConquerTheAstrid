using Camera;
using Items.Resource;
using NnUtils.Scripts;
using Planet;
using UserInterface;

namespace Core
{
    using UnityEngine;
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null) 
                    _instance = new GameObject("Game Manager", typeof(GameManager)).GetComponent<GameManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value.gameObject);
                    return;
                }

                _instance = value;
            }
        }
        private void Awake() => _instance = GetComponent<GameManager>();
        
        private Camera _mainCam;
        public Camera MainCam => _mainCam = _mainCam == null ? Camera.main : _mainCam;
       
        private CameraScript _cameraScript;
        public CameraScript CameraScript =>
            _cameraScript = _cameraScript == null ? MainCam.GetComponent<CameraScript>() : _cameraScript;
        
        private SelectionManager _selectionManager;
        public SelectionManager SelectionManager =>
            _selectionManager ? _selectionManager : _selectionManager = gameObject.GetOrAddComponent<SelectionManager>();

        private AudioManager _audioManager;
        public AudioManager AudioManager =>
            _audioManager ? _audioManager : _audioManager = gameObject.GetOrAddComponent<AudioManager>();

        private PollutionScript _pollutionScript;
        public PollutionScript PollutionScript =>
            _pollutionScript ? _pollutionScript : _pollutionScript = gameObject.GetOrAddComponent<PollutionScript>();

        private ResourcesScript _resourcesScript;
        public ResourcesScript ResourcesScript =>
            _resourcesScript ? _resourcesScript : _resourcesScript = gameObject.GetOrAddComponent<ResourcesScript>();

        private ResourcesManagerScript _resourcesManagerScript;
        public ResourcesManagerScript ResourcesManagerScript =>
            _resourcesManagerScript ? _resourcesManagerScript : _resourcesManagerScript = gameObject.GetOrAddComponent<ResourcesManagerScript>();

        private UIManager _uiManager;
        public UIManager UIManager =>
            _uiManager ? _uiManager : _uiManager = FindFirstObjectByType<UIManager>();

        private HotbarScript _hotbarScript;

        public HotbarScript HotbarScript =>
            _hotbarScript ? _hotbarScript : _hotbarScript = FindFirstObjectByType<HotbarScript>();

        private PlanetManagerScript _planetManagerScript;

        public PlanetManagerScript PlanetManagerScript =>
            _planetManagerScript ? _planetManagerScript : _planetManagerScript = FindFirstObjectByType<PlanetManagerScript>();
    }
}
