using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock : MonoBehaviour
{
    public AudioClip cracksound;
    public AudioClip breaksound;
    private AudioSource audioSource;

    public BgmManager bgm;

    private bool pause = true;

    public GameObject brokenrock;
    public GameObject tallrock;
    public GameObject mushroom;
    public Flashlight flashlight;

    private float breaking = 0.0f;
    private float breakspeed = 1.0f;

    private bool breakupdate = true;

    public Canvas UICanvas;
    Image progressBar;
    Vector3 bar_ori_pos;
    float bar_pos_offset = 0.23f;
    float bar_full_scale = 0.48f;
    float trigger_radius = 5.0f;

    public const RockID ID;
 
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        progressBar = UICanvas.gameObject.transform.Find("RockBar").Find("Filler").GetComponent<Image>();
        bar_ori_pos = progressBar.transform.localPosition;

        if (SceneForestSettings.rockStatus[ID])
        {
            breakupdate = false;
            brokenrock.SetActive(true);
            mushroom.SetActive(true);
            tallrock.SetActive(false);
            GameHandler.instance.lightManager.DarkenSceneLight();

            UICanvas.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (breakupdate == false)
            return;

        float displacement = (GameHandler.instance.player.transform.position - transform.position).magnitude;
        if (displacement < trigger_radius || flashlight.CheckIfInFlashlight(tallrock.GetComponent<BoxCollider>()) == true)
            UICanvas.gameObject.SetActive(true);
        else
            UICanvas.gameObject.SetActive(false);

        if (flashlight.CheckIfInFlashlight(tallrock.GetComponent<BoxCollider>()) == true)
        {
            if (pause)
            {
                audioSource.clip = cracksound;
                audioSource.Play();
                pause = false;
            }
            breaking += breakspeed * Time.deltaTime;

            progressBar.transform.localPosition = new Vector3(bar_ori_pos.x + breaking * bar_pos_offset * 0.2f, bar_ori_pos.y, bar_ori_pos.z);
            progressBar.transform.localScale = new Vector3(breaking * bar_full_scale * 0.2f, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
        }
        else
        {
            if (!pause)
            {
                audioSource.Pause();
                pause = true;
            }
        }

        //Debug.Log(breaking);

        if (breaking >= 5.0f)
        {
            audioSource.clip = breaksound;
            audioSource.Play();
            bgm.ForestBGM();

            breakupdate = false;
            brokenrock.SetActive(true);
            mushroom.SetActive(true);
            tallrock.SetActive(false);
            GameHandler.instance.lightManager.DarkenSceneLight();

            UICanvas.gameObject.SetActive(false);

            SceneForestSettings.rockStatus[ID] = true;
        }
    }
}

public enum RockID
{
    ROCK_ROBERT,
    ROCK_ROBERTIA,
    ROCK_NOONIONS,
    ROCK_JEREMY,

    NUM_TOTAL
}
