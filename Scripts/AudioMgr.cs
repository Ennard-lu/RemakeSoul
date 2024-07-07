using Godot;
using System;
using System.Collections.Generic;

public partial class AudioMgr : Node2D {
	private Dictionary<String, AudioStreamPlayer> audios = new();
	
	public float MasterVolume;
	public float SFXVolume;

	private static AudioMgr _instance;
	public static AudioMgr Instance {
		get { return _instance; }
	}

	public override void _Ready() {
        RenewVolume();

        _instance = this;
        int count = GetChildCount();
        for (int i = 0; i < count; i++) {
            var node = GetChild<AudioStreamPlayer>(i);
            audios[node.Name] = node;
        }
    }

    public void RenewVolume() {
        MasterVolume = AudioServer.GetBusVolumeDb(0);
        SFXVolume = AudioServer.GetBusVolumeDb(1);
    }

    public void SetVolume() {
        AudioServer.SetBusVolumeDb(0, MasterVolume);
        AudioServer.SetBusVolumeDb(1, SFXVolume);
    }

    public void Play(String audioName) {
		audios[audioName].Play();
	}

	public void CheckPlay(String audioName) {
		if (!audios[audioName].Playing) {
			audios[audioName].Play();
		}
	}

}
