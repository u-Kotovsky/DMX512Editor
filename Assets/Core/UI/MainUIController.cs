using System;
using System.Collections.Generic;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class MainUIController : MonoBehaviour
    {
        public RectTransform hotbar;
        public RectTransform page;

        public RectTransform hierarchy;
        public RectTransform inspector;

        [Header("Unity Assets")]
        [Tooltip("Since Unity doesn't have a proper way of givin' us developers default assets, I steal it through this reference. Please keep it as UISprite.")]
        public Sprite defaultUISprite;
        public static Sprite DefaultUISprite;
        
        private static List<Button> hotbarButtons;

        private Camera targetCamera;

        private const string Prefix = "MainUIController";
        
        public static MainUIController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            try
            {
                RefreshCameraReferences();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError($"'{Prefix}' Failed to refresh camera references");
            }
            
            DefaultUISprite ??= defaultUISprite;
            hotbarButtons = new List<Button>();
        }
        
        public event Action OnDeconstructUI = delegate { };
        public event Action OnUpdate = delegate { };

        private void Start()
        {
            var buttonColor = Color.gray3;
            var textColor = Color.white;
            
            UIUtility.AddButton(hotbar, "Record", buttonColor, textColor, button =>
            {
                button.OnClick(() =>
                {
                    OnDeconstructUI?.Invoke();
                    //OnDeconstructUI += ConsoleUI.DeconstructUI;
                    //OnDeconstructUI += () => { OnDeconstructUI -= ConsoleUI.DeconstructUI; };
                    CleanScreen();
                    SetHotBarButtons(true);
                    button.interactable = false;
                    Debug.Log($"'{Prefix}' Open Record");
                    RecordUI.BuildUI(page);
                });
                
                hotbarButtons.Add(button);
                button.onClick?.Invoke();
            });
            
            UIUtility.AddButton(hotbar, "Settings", buttonColor, textColor, button =>
            {
                button.OnClick(() =>
                {
                    OnDeconstructUI?.Invoke();
                    //OnDeconstructUI += SettingsUI.DeconstructUI;
                    //OnDeconstructUI += () => { OnDeconstructUI -= SettingsUI.DeconstructUI; };
                    CleanScreen();
                    SetHotBarButtons(true);
                    button.interactable = false;
                    Debug.Log($"'{Prefix}' Open Settings");
                    SettingsUI.BuildUI(page);
                });
                
                hotbarButtons.Add(button);
            });
            
            //EffectUI.AddBoxSelection(page.parent.GetComponent<RectTransform>());
            //EffectUI.SetBoxSelectionActive(false);
            
            SettingsUI.Poke();
            SettingsUI.Load();
            RecordUI.Poke();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        public static void Open(int index)
        {
            if (index < 0) index = 0;
            if (index > hotbarButtons.Count - 1) index = hotbarButtons.Count - 1;
            
            hotbarButtons[index].onClick.Invoke();
        }

        private void RefreshCameraReferences()
        {
            targetCamera = Camera.main;
            //if (targetCamera != null) cameraController = targetCamera?.GetComponent<SpectatorCameraController>();
        }

        private void CleanScreen()
        {
            foreach (Transform child in page.transform)
                Destroy(child.gameObject);
        }

        private void SetHotBarButtons(bool active)
        {
            foreach (var button in hotbarButtons) // null
                button.interactable = active;
        }
    }
}