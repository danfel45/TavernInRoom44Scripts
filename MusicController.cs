using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> songs = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button openFolderButton; 
    [SerializeField] private Button skipButton; 
    [SerializeField] private Button previousButton;

    
    [SerializeField]private int currentIndex = 0;
    private Coroutine musicCoroutine;
    private string userSongsPath;
    private HashSet<string> loadedSongs = new HashSet<string>();

    private void Awake()
    {
        userSongsPath = Application.persistentDataPath + "/UserSongs";

        
        if (!Directory.Exists(userSongsPath))
        {
            Directory.CreateDirectory(userSongsPath);
        }

        
        if (!PlayerPrefs.HasKey("DefaultSongsCopied"))
        {
            
            CopyDefaultSongsToUserFolder();

           
            PlayerPrefs.SetInt("DefaultSongsCopied", 1);
            PlayerPrefs.Save();
        }

        
        StartCoroutine(LoadUserSongs());

       
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

       
        audioSource.volume = 0.3f;

        
        if (volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

       
        StartCoroutine(CheckForNewSongs());
    }






    private void Start()
    {
        if (songs.Count > 0)
        {
            StartMusicPlayback();
        }
    }

    private void Update()
    {
        
    }
    private void UpdateButtonState()
    {
        if (songs.Count <= 1)
        {
            skipButton.interactable = false;
            previousButton.interactable = false;
        }
        else
        {
            skipButton.interactable = true;
            previousButton.interactable = true;
        }
    }
    private void CopyDefaultSongsToUserFolder()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, "UserSongs");

        if (Directory.Exists(sourcePath))
        {
            string[] defaultFiles = Directory.GetFiles(sourcePath);

            foreach (string file in defaultFiles)
            {
                string fileName = Path.GetFileName(file);
                string destinationPath = Path.Combine(userSongsPath, fileName);

               
                if (!File.Exists(destinationPath))
                {
                    File.Copy(file, destinationPath);
                }
            }
        }
    }

    private IEnumerator LoadUserSongs()
    {
       
        if (songs == null) songs = new List<AudioClip>();
       

       

        DirectoryInfo dirInfo = new DirectoryInfo(userSongsPath);

       
        FileInfo[] files = dirInfo.GetFiles("*.mp3").Concat(dirInfo.GetFiles("*.wav")).ToArray();

        List<string> currentFiles = files.Select(f => f.Name).ToList();

      
        RemoveMissingSongs(currentFiles);

        foreach (FileInfo file in files)
        {
            if (!loadedSongs.Contains(file.Name))
            {
                string filePath = "file://" + file.FullName;

                AudioType audioType = file.Extension.ToLower() == ".wav" ? AudioType.WAV : AudioType.MPEG;

                using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, audioType))
                {
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                        clip.name = file.Name;

                       
                        songs.Add(clip);
                        
                        loadedSongs.Add(file.Name);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"Failed to load audio file: {file.Name} - {request.error}");
                    }
                }
            }
        }

       
    }




    private void RemoveMissingSongs(List<string> currentFiles)
    {
        List<AudioClip> songsToRemove = new List<AudioClip>();

       
        foreach (AudioClip song in songs)
        {
            if (!currentFiles.Contains(song.name)) 
            {
                songsToRemove.Add(song);
            }
        }

       
        foreach (AudioClip song in songsToRemove)
        {
            songs.Remove(song);
            
            loadedSongs.Remove(song.name);

           
            if (audioSource.clip == song)
            {
              
                audioSource.Stop();
                RestartPlayback(); 
            }
        }
    }

    private IEnumerator CheckForNewSongs()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            yield return StartCoroutine(LoadUserSongs());
        }
    }

    private IEnumerator PlayMusicFromPlaylist()
    {
        while (true)
        {
            audioSource.clip = songs[currentIndex];
            audioSource.Play();

            yield return new WaitForSeconds(songs[currentIndex].length);
            MoveToNextSong();
        }
    }

    private void StartMusicPlayback()
    {
        songs = new List<AudioClip>(songs);
        ShufflePlaylist(songs);
        musicCoroutine = StartCoroutine(PlayMusicFromPlaylist());
    }

    private void RestartPlayback()
    {
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
        }

        if (songs.Count > 0)
        {
            currentIndex = 0; 
            musicCoroutine = StartCoroutine(PlayMusicFromPlaylist());
        }
    }

    public void OpenSongsFolder()
    {
       
        Process.Start(userSongsPath);
    }

    public void MoveToNextSong()
    {
        if (songs.Count <= 1) return;

        currentIndex = (currentIndex + 1) % songs.Count;
        PlayCurrentSong();
    }

    public void MoveToLastSong()
    {
        if (songs.Count <= 1) return;

        currentIndex = (currentIndex - 1 + songs.Count) % songs.Count;
        PlayCurrentSong();
    }

    private void PlayCurrentSong()
    {
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
        }

        audioSource.Stop();
        audioSource.clip = songs[currentIndex];
        audioSource.Play();

       
        musicCoroutine = StartCoroutine(PlayMusicFromPlaylist());
    }

    private void ShufflePlaylist(List<AudioClip> playlist)
    {
        for (int i = 0; i < playlist.Count; i++)
        {
            int randomIndex = Random.Range(0, playlist.Count);
            AudioClip temp = playlist[i];
            playlist[i] = playlist[randomIndex];
            playlist[randomIndex] = temp;
        }
    }
    public void SetVolume(float volume) => audioSource.volume = volume;
}
