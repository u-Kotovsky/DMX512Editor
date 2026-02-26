using System.Diagnostics;
using Melanchall.DryWetMidi.Multimedia;
using Unity_DMX;
using Unity_DMX.Core;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Core
{
    public class DmxRecorder : MonoBehaviour
    {
        public DmxController dmxController;
        
        public DmxDataContainer currentRecording = new DmxDataContainer();
        
        public Stopwatch stopwatch = new Stopwatch();
        
        private InputDevice inputDevice;

        public ulong ticks = 0;

        private static DmxRecorder _instance;
        public static DmxRecorder Instance { get { return _instance; } }
        
        public bool IsRecording => stopwatch.IsRunning;
        
        private void Start()
        {
            _instance = this;
            inputDevice = InputDevice.GetByName("MA2 MidiLoop");
            
            inputDevice.MidiTimeCodeReceived += InputDeviceOnMidiTimeCodeReceived;
            
            dmxController.dmxBuffer.OnBufferUpdate += DmxBufferOnOnBufferUpdate;
            //dmxController.OnDmxDataChanged += DmxControllerOnOnDmxDataChanged;
            
            inputDevice.StartEventsListening();
        }

        private void DmxBufferOnOnBufferUpdate(DmxData buffer)
        {
            // Ensure we have at least one universe
            buffer.EnsureCapacity(512);
            
            var universeCount = (short)(buffer.Count / 512);
            
            for (short i = 0; i < universeCount; i++)
            {
                var universeBuffer = BufferUtility.TakeUniverseFromGlobalBuffer(i, buffer);
                
                if (!stopwatch.IsRunning) return;
                ticks++;

                var data = new DmxUniverseContainer();
            
                data.Universe = i;
                data.Bytes.BlockCopy(universeBuffer.GetBufferArray(), 0, 0, 512);
            
                currentRecording.keyframes.Add(stopwatch.ElapsedMilliseconds, data);
                
                //OnDmxDataChanged?.Invoke(i, universeBuffer, nuffer);
                //SendDmxData(i);
            }
        }

        private void InputDeviceOnMidiTimeCodeReceived(object sender, MidiTimeCodeReceivedEventArgs e)
        {
            //int hours, minutes, seconds, frames;
            Debug.Log($"timecode: {e.Hours}:{e.Minutes}:{e.Seconds} {e.Frames}");
        }

        private void DmxControllerOnOnDmxDataChanged(short universe, DmxData universeBuffer, DmxData wholeBuffer)
        {
            if (!stopwatch.IsRunning) return;
            ticks++;

            var data = new DmxUniverseContainer();
            
            data.Universe = universe;
            data.Bytes.BlockCopy(universeBuffer.GetBufferArray(), 0, 0, 512);
            
            currentRecording.keyframes.Add(stopwatch.ElapsedMilliseconds, data);
        }

        public void StartRecording()
        {
            Debug.Log($"Started recording. Total frames: {currentRecording.keyframes.Count}");
            
            currentRecording.keyframes.Clear();
            stopwatch.Reset();
            stopwatch.Start();
        }

        public void StopRecording()
        {
            Debug.Log($"Stopped recording. Total frames: {currentRecording.keyframes.Count}");
            stopwatch.Stop();
        }

        public void SaveRecording()
        {
            Debug.Log($"Save recording.");
            currentRecording.Save();
        }

        public void LoadRecording()
        {
            Debug.Log($"Load recording.");
            currentRecording = DmxDataContainer.Load();
        }
        
        

        public void StartPlayback()
        {
            Debug.Log($"Started playback.");
            
        }

        public void StopPlayback()
        {
            Debug.Log($"Stopped playback.");
            
        }

        public void PausePlayback()
        {
            Debug.Log($"Pause playback.");
            
        }
    }
}