using System;
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

        public bool record = false;
        public bool save = false;
        public ulong ticks = 0;
        
        private void Start()
        {
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

        private void Update()
        {
            if (record && !stopwatch.IsRunning)
            {
                stopwatch.Restart();
                Debug.Log($"Started recording. Total frames: {currentRecording.keyframes.Count}");
            }

            if (!record && stopwatch.IsRunning)
            {
                stopwatch.Stop();
                Debug.Log($"Stopped recording. Total frames: {currentRecording.keyframes.Count}");
            }

            if (save)
            {
                save = false;
                currentRecording.Save("show.bin");
            }
        }
    }
}