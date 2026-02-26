using System;
using Runtime.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public static class RecordUI
    {
        private const string Prefix = "RecordUI";

        static RecordUI()
        {
            try
            {
                // on init
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void Poke()
        {
            // Initialize static part
            OnClick_StopRecording();
        }

        private static RectTransform _root, _list;
        private static Button _startRecording, _saveRecording, _loadRecording, _stopRecording,
            _startPlayback, _stopPlayback, _pausePlayback;

        public static void BuildUI(Transform parent)
        {
            _root = UIUtility.AddRect(parent, Prefix)
                .WithVerticalLayout()
                .SetAllStretch(Vector4.zero).GetRect();

            _list = UIUtility.CreateVerticalList(_root, "TimelineList");
            
            var element0 = UIUtility.AddItemToList(_list, 1, 25);
            var element1 = UIUtility.AddItemToList(_list, 1, 25);
            
            UIUtility.AddText(element0, "Recording", Color.white);
            UIUtility.AddText(element1, "Playback", Color.white);
            
            var btnColor = Color.white * 0.3f;
            var txtColor = Color.white;

            _startRecording = UIUtility.AddButton(element0, "Start Recording", btnColor, txtColor)
                .OnClick(OnClick_StartRecording);
            _stopRecording = UIUtility.AddButton(element0, "Stop Recording", btnColor, txtColor)
                .OnClick(OnClick_StopRecording);
            _saveRecording = UIUtility.AddButton(element0, "Save Recording", btnColor, txtColor)
                .OnClick(OnClick_SaveRecording);
            _loadRecording = UIUtility.AddButton(element0, "Load Recording", btnColor, txtColor)
                .OnClick(OnClick_LoadRecording);
            
            _startPlayback = UIUtility.AddButton(element1, "Play", btnColor, txtColor)
                .OnClick(OnClick_StartPlayback);
            _stopPlayback = UIUtility.AddButton(element1, "Stop", btnColor, txtColor)
                .OnClick(OnClick_StopPlayback);
            _pausePlayback = UIUtility.AddButton(element1, "Pause", btnColor, txtColor)
                .OnClick(OnClick_PausePlayback);
            
            //MainUIController.Instance.OnUpdate += Update;
            RefreshUI();
        }
        
        public static void DeconstructUI()
        {
            //MainUIController.Instance.OnDeconstructUI -= DeconstructUI;
            //TimelineService.OnCurrentTimeChanged -= OnCurrentTimeChanged;
            //MainUIController.Instance.OnUpdate -= Update;
        }

        private static void RefreshUI()
        {
            if (DmxRecorder.Instance.IsRecording)
            {
                _startRecording.interactable = false;
                _stopRecording.interactable = true;
                _saveRecording.interactable = false;
                _loadRecording.interactable = false;
            }
            else
            {
                _startRecording.interactable = true;
                _stopRecording.interactable = false;
                _saveRecording.interactable = true;
                _loadRecording.interactable = true;
            }
        }

        private static void OnClick_StartRecording()
        {
            if (DmxRecorder.Instance.IsRecording) return;
            
            DmxRecorder.Instance.StartRecording();
            RefreshUI();
        }

        private static void OnClick_StopRecording()
        {
            if (!DmxRecorder.Instance.IsRecording) return;
            
            DmxRecorder.Instance.StopRecording();
            RefreshUI();
        }

        private static void OnClick_SaveRecording()
        {
            if (DmxRecorder.Instance.IsRecording) return;
            
            DmxRecorder.Instance.SaveRecording();
        }

        private static void OnClick_LoadRecording()
        {
            if (DmxRecorder.Instance.IsRecording) return;
            
            DmxRecorder.Instance.LoadRecording();
        }
        
        private static void OnClick_StartPlayback()
        {
            //if (DmxRecorder.Instance.IsRecording) return;
            
            //DmxRecorder.Instance.SaveRecording();
        }
        
        private static void OnClick_StopPlayback()
        {
            //if (DmxRecorder.Instance.IsRecording) return;
            
            //DmxRecorder.Instance.SaveRecording();
        }
        
        private static void OnClick_PausePlayback()
        {
            //if (DmxRecorder.Instance.IsRecording) return;
            
            //DmxRecorder.Instance.SaveRecording();
        }
    }
}