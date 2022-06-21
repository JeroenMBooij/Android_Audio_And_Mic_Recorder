using Android.Media;
using Android.OS;
using Java.IO;
using Soundpad.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encoding = Android.Media.Encoding;
using FileNotFoundException = Java.IO.FileNotFoundException;
using IOException = Java.IO.IOException;
using Stream = Android.Media.Stream;

namespace Soundpad.Platforms.Android.Services.Playback
{
    /// <summary>
    /// Reversed engineered from https://stackoverflow.com/questions/41541956/play-pcm-stream-in-android
    /// </summary>
    public class PCMPlayer
    {
        public string AudioPath { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsLooping { get; set; }

        public Action StopCallback { get; set; }


        private AudioTrack _audioPlayer;
        private FileInputStream _input = null;

        private int _count = 64 * 1024; // 64 kb
        private int _bytesread = 0, _ret = 0;
        private int _size;
        private byte[] _byteData = null;

        private Thread _thread;
        private static Handler _handler;

        public PCMPlayer(string audioPath)
        {
            AudioPath = audioPath;
        }

        public void Play()
        {
            InitAudio(AudioPath);

            _audioPlayer.Play();

            _thread = new Thread(() =>
            {
                while (_bytesread < _size && IsPlaying)
                {
                    try
                    {
                        _ret = _input.Read(_byteData, 0, _count);
                    }
                    catch (IOException e)
                    {
                        e.PrintStackTrace();
                    }
                    if (_ret != -1)
                    { // Write the byte array to the track
                        _audioPlayer.Write(_byteData, 0, _ret);
                        _bytesread += _ret;
                    }
                    else break;
                }

                try
                {
                    _input.Close();
                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                }
                if (_audioPlayer != null)
                {
                    if (_audioPlayer.State == AudioTrackState.Initialized)
                    {
                        Stop();

                        if (StopCallback is not null)
                            StopCallback();
                    }
                }
                if (IsLooping && IsPlaying)
                    _handler.PostDelayed(Play, 100);
            });

            _thread.Start();
        }

        private void InitAudio(string pathAudio)
        {
            _handler = new Handler();
            IsPlaying = true;
            _bytesread = 0;
            _ret = 0;
            _audioPlayer = CreateAudioPlayer(pathAudio);

        }

        private AudioTrack CreateAudioPlayer(string pathAudio)
        {
            int intSize = AudioTrack.GetMinBufferSize(RConst.DECODE_SAMPLE_RATE, ChannelOut.Stereo, Encoding.Pcm16bit);

            AudioTrack audioTrack = new AudioTrack(Stream.Music, RConst.DECODE_SAMPLE_RATE, ChannelOut.Stereo, Encoding.Pcm16bit, intSize, AudioTrackMode.Stream);
            Java.IO.File file = new Java.IO.File(pathAudio);

            _byteData = new byte[_count];
            try
            {
                _input = new FileInputStream(file);

            }
            catch (FileNotFoundException e)
            {
                e.PrintStackTrace();
            }

            _size = (int)file.Length();

            return audioTrack;
        }


        private void Stop()
        {
            IsPlaying = false;
            if (_thread != null)
            {
                _thread.Interrupt();
                _thread = null;
            }
            if (_audioPlayer != null)
            {
                _audioPlayer.Stop();
                _audioPlayer.Release();
                _audioPlayer = null;
            }
        }
    }
}
