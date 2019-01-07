using UnityEngine;
using System.Collections;

public class Lettuce : MonoBehaviour {

    [SerializeField]
    bool teleport = false;

    [SerializeField]
    float tpX = -50.36f;
    [SerializeField]
    float tpY = 4.31f;

    [SerializeField]
    float unitsToSlide = 0f;

    [SerializeField]
    float playerTpX = -55.624f;
    [SerializeField]
    float playerTpY = 0.7f;

    [SerializeField]
    bool win = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            PlayerVariables.instance.setPlayerCanMove(false);
            PlayerVariables.instance.setCutscene(true);

            if (AudioManager.instance != null)
                AudioManager.instance.PlaySound2D("Wow");

            if (win)
            {
                PlayerVariables.instance.Win();
                Destroy(gameObject);
                return;
            }

            if (teleport)
            {
                Transform cameraTransform = Camera.main.transform;
                Transform playerTransform = collision.gameObject.transform;

                cameraTransform.position = new Vector3(tpX, tpY, cameraTransform.position.z);
                playerTransform.position = new Vector3(playerTpX, playerTpY, playerTransform.position.z);

                PlayerVariables.instance.setPlayerCanMove(true);
                PlayerVariables.instance.setCheckpoint(GameObject.FindGameObjectWithTag("Player").transform.position);
                PlayerVariables.instance.setCutscene(false);
                Destroy(gameObject);
            }
            else
            {
                Transform cameraTransform = Camera.main.transform;
                Vector3 startPos = cameraTransform.position;

                StartCoroutine(SlideCamera(cameraTransform, startPos));
            }
        }
    }

    IEnumerator SlideCamera(Transform cameraTransform, Vector3 startPos)
    {
        float duration = 1f;
        float smoothness = 0.01f;
        float progress = 0f;

        Transform moveableWall = GameObject.FindGameObjectWithTag("MoveableWall").transform;
        Vector3 startPosWall = moveableWall.position;

        while (progress < 1.05)
        {
            cameraTransform.position = Vector3.Lerp(startPos, startPos + new Vector3(unitsToSlide, 0, 0), progress);
            moveableWall.position = Vector3.Lerp(startPosWall, startPosWall + new Vector3(unitsToSlide, 0, 0), progress);
            progress += smoothness / duration;
            yield return new WaitForSeconds(smoothness);
        }
        PlayerVariables.instance.setPlayerCanMove(true);
        PlayerVariables.instance.setCheckpoint(GameObject.FindGameObjectWithTag("Player").transform.position);
        PlayerVariables.instance.setCutscene(false);
        Destroy(gameObject);
        yield return null;
    }
}
