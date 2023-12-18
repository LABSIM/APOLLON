//
// APOLLON : immersive & interactive experimental protocol engine
// Copyright (C) 2021-2023  Nawfel KINANI 
// nawfel (dot) kinani at onera (dot) fr
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program; see the files COPYING and COPYING.LESSER.
// If not, see <http://www.gnu.org/licenses/>.
//

// using System;
// using System.IO;
// using UnityEngine;
// using System.Collections.Generic;
// using System.Threading;

// avoid namespace pollution
namespace Labsim.apollon.common
{

    public class ApollonWavRecorder
    {

	    const int HEADER_SIZE = 44;

        struct ClipData
        {

            public int samples;
            public int channels;
            public float[] samplesData;

        }

	    public bool Save(string filename, UnityEngine.AudioClip clip) {
		    if (!filename.ToLower().EndsWith(".wav")) {
			    filename += ".wav";
		    }

		    var filepath = filename;

		    UnityEngine.Debug.Log(filepath);

		    // Make sure directory exists if user is saving to sub dir.
		    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filepath));
		    ClipData clipdata = new ClipData();
		    clipdata.samples = clip.samples;
		    clipdata.channels = clip.channels;
		    float[] dataFloat = new float[clip.samples*clip.channels];
		    clip.GetData (dataFloat, 0);
		    clipdata.samplesData = dataFloat;
		    using (var fileStream = CreateEmpty(filepath)) {
			    System.IO.MemoryStream memstrm = new System.IO.MemoryStream ();
			    ConvertAndWrite(memstrm, clipdata);
			    memstrm.WriteTo (fileStream);
			    WriteHeader(fileStream, clip);
		    }

		    return true; // TODO: return false if there's a failure saving the file
	    }

	    public UnityEngine.AudioClip TrimSilence(UnityEngine.AudioClip clip, float min) {
		    var samples = new float[clip.samples];

		    clip.GetData(samples, 0);

		    return TrimSilence(new System.Collections.Generic.List<float>(samples), min, clip.channels, clip.frequency);
	    }

	    public UnityEngine.AudioClip TrimSilence(System.Collections.Generic.List<float> samples, float min, int channels, int hz) {
		    return TrimSilence(samples, min, channels, hz, false, false);
	    }

	    public UnityEngine.AudioClip TrimSilence(System.Collections.Generic.List<float> samples, float min, int channels, int hz, bool _3D, bool stream) {
		    int i;

		    for (i=0; i<samples.Count; i++) {
			    if (UnityEngine.Mathf.Abs(samples[i]) > min) {
				    break;
			    }
		    }

		    samples.RemoveRange(0, i);

		    for (i=samples.Count - 1; i>0; i--) {
			    if (UnityEngine.Mathf.Abs(samples[i]) > min) {
				    break;
			    }
		    }

		    samples.RemoveRange(i, samples.Count - i);

		    var clip = UnityEngine.AudioClip.Create("TempClip", samples.Count, channels, hz, _3D, stream);

		    clip.SetData(samples.ToArray(), 0);

		    return clip;
	    }

	    System.IO.FileStream CreateEmpty(string filepath) {
		    var fileStream = new System.IO.FileStream(filepath, System.IO.FileMode.Create);
	        byte emptyByte = new byte();

	        for(int i = 0; i < HEADER_SIZE; i++) //preparing the header
	        {
	            fileStream.WriteByte(emptyByte);
	        }

		    return fileStream;
	    }

	    void ConvertAndWrite(System.IO.MemoryStream memStream, ClipData clipData)
	    {
		    float[] samples = new float[clipData.samples*clipData.channels];

		    samples = clipData.samplesData;

		    var intData = new short[samples.Length];

		    var bytesData = new byte[samples.Length * 2];

		    const float rescaleFactor = 32767; // to convert float to Int16

		    for (int i = 0; i < samples.Length; i++)
		    {
			    intData[i] = (short)(samples[i] * rescaleFactor);
			    //Debug.Log (samples [i]);
		    }
		    System.Buffer.BlockCopy(intData, 0, bytesData, 0, bytesData.Length);
		    memStream.Write(bytesData, 0, bytesData.Length);
	    }

	     void WriteHeader(System.IO.FileStream fileStream, UnityEngine.AudioClip clip) {

		    var hz = clip.frequency;
		    var channels = clip.channels;
		    var samples = clip.samples;

		    fileStream.Seek(0, System.IO.SeekOrigin.Begin);

		    var riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
		    fileStream.Write(riff, 0, 4);

		    var chunkSize = System.BitConverter.GetBytes(fileStream.Length - 8);
		    fileStream.Write(chunkSize, 0, 4);

		    var wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
		    fileStream.Write(wave, 0, 4);

		    var fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
		    fileStream.Write(fmt, 0, 4);

		    var subChunk1 = System.BitConverter.GetBytes(16);
		    fileStream.Write(subChunk1, 0, 4);

		    // System.UInt16 two = 2;
            ushort one = 1;

		    var audioFormat = System.BitConverter.GetBytes(one);
		    fileStream.Write(audioFormat, 0, 2);

		    var numChannels = System.BitConverter.GetBytes(channels);
		    fileStream.Write(numChannels, 0, 2);

		    var sampleRate = System.BitConverter.GetBytes(hz);
		    fileStream.Write(sampleRate, 0, 4);

		    var byteRate = System.BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
		    fileStream.Write(byteRate, 0, 4);

            ushort blockAlign = (ushort) (channels * 2);
		    fileStream.Write(System.BitConverter.GetBytes(blockAlign), 0, 2);

		    ushort bps = 16;
		    var bitsPerSample = System.BitConverter.GetBytes(bps);
		    fileStream.Write(bitsPerSample, 0, 2);

		    var datastring = System.Text.Encoding.UTF8.GetBytes("data");
		    fileStream.Write(datastring, 0, 4);

		    var subChunk2 = System.BitConverter.GetBytes(samples * channels * 2);
		    fileStream.Write(subChunk2, 0, 4);

    //		fileStream.Close();
	    }

    } /* class ApollonWavRecorder */

} /* } Labsim.apollon.common */
