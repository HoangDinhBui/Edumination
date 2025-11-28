using System;
using System.IO;
using NAudio.Wave;

namespace IELTS.BLL
{
    public class AudioRecorder
    {
        private WaveInEvent waveIn;
        private WaveFileWriter waveWriter;
        private string outputFilePath;

        // Event để cập nhật level meter
        public event EventHandler<float> AudioLevelChanged;

        public AudioRecorder()
        {
            // Constructor rỗng
        }

        // Lấy danh sách các recording devices
        public static string[] GetRecordingDevices()
        {
            int deviceCount = WaveInEvent.DeviceCount;
            string[] devices = new string[deviceCount];

            for (int i = 0; i < deviceCount; i++)
            {
                var caps = WaveInEvent.GetCapabilities(i);
                devices[i] = $"[{i}] {caps.ProductName}";
            }

            return devices;
        }

        public void StartRecording(string filePath, int deviceNumber = 0)
        {
            try
            {
                outputFilePath = filePath;

                // Khởi tạo WaveIn với device được chọn
                waveIn = new WaveInEvent
                {
                    DeviceNumber = deviceNumber,
                    WaveFormat = new WaveFormat(44100, 16, 1), // 44.1kHz, 16-bit, Mono
                    BufferMilliseconds = 50 // Giảm buffer để responsive hơn
                };

                // Khởi tạo WaveFileWriter
                waveWriter = new WaveFileWriter(outputFilePath, waveIn.WaveFormat);

                // Xử lý sự kiện khi có dữ liệu audio
                waveIn.DataAvailable += WaveIn_DataAvailable;

                // Xử lý sự kiện khi recording dừng
                waveIn.RecordingStopped += WaveIn_RecordingStopped;

                // Bắt đầu ghi âm
                waveIn.StartRecording();
                
                System.Diagnostics.Debug.WriteLine($"Started recording to: {filePath}");
                System.Diagnostics.Debug.WriteLine($"Device: {deviceNumber}");
                System.Diagnostics.Debug.WriteLine($"Format: {waveIn.WaveFormat}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi bắt đầu ghi âm: {ex.Message}", ex);
            }
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (waveWriter != null && e.BytesRecorded > 0)
                {
                    // Ghi dữ liệu audio vào file
                    waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
                    waveWriter.Flush();

                    // Tính toán audio level để hiển thị
                    float max = 0;
                    for (int i = 0; i < e.BytesRecorded; i += 2)
                    {
                        short sample = (short)((e.Buffer[i + 1] << 8) | e.Buffer[i]);
                        float sample32 = sample / 32768f;
                        max = Math.Max(max, Math.Abs(sample32));
                    }

                    // Trigger event để cập nhật UI
                    AudioLevelChanged?.Invoke(this, max);
                    
                    System.Diagnostics.Debug.WriteLine($"Data written: {e.BytesRecorded} bytes, Level: {max:F3}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi ghi dữ liệu: {ex.Message}");
            }
        }

        private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Recording stopped");
            
            // Dọn dẹp resources khi recording dừng
            Cleanup();

            if (e.Exception != null)
            {
                System.Diagnostics.Debug.WriteLine($"Recording stopped with error: {e.Exception.Message}");
            }
        }

        public void StopRecording()
        {
            try
            {
                if (waveIn != null)
                {
                    System.Diagnostics.Debug.WriteLine("Stopping recording...");
                    waveIn.StopRecording();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi dừng ghi âm: {ex.Message}");
                Cleanup();
            }
        }

        private void Cleanup()
        {
            try
            {
                // Dispose WaveWriter trước
                if (waveWriter != null)
                {
                    waveWriter.Dispose();
                    waveWriter = null;
                }

                // Dispose WaveIn sau
                if (waveIn != null)
                {
                    waveIn.DataAvailable -= WaveIn_DataAvailable;
                    waveIn.RecordingStopped -= WaveIn_RecordingStopped;
                    waveIn.Dispose();
                    waveIn = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi cleanup: {ex.Message}");
            }
        }

        ~AudioRecorder()
        {
            Cleanup();
        }
    }
}