using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform orientation;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;
    private bool isGrounded = true;

    [Header("Dialogue Settings")]
    public GameObject textbox;
    public TextMeshProUGUI characterText;
    public TextMeshProUGUI dialogueText;
    public float typeSpeed = 0.05f;
    public AudioSource dialogueSound;

    private Queue<(string character, string line)> dialogueQueue = new Queue<(string, string)>();
    private bool isPaused = false;
    public bool istextboxOn = true;
    private bool isTyping = false;
    private string currentFullLine = "";

    public bool IsTextboxOn => istextboxOn;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        LoadDialogue("chuong1");
        StartCoroutine(ShowNextDialogueLine());
    }

    void Update()
    {
        if (!istextboxOn)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            Vector3 forward = Vector3.ProjectOnPlane(orientation.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(orientation.right, Vector3.up).normalized;

            movement = (forward * moveZ + right * moveX).normalized;

            animator.SetFloat("Speed", movement.magnitude);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                animator.SetBool("isJump", true);
                isGrounded = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (isTyping)
                {
                    StopAllCoroutines();
                    dialogueText.text = currentFullLine;
                    isTyping = false;
                }
                else
                {
                    StartCoroutine(ShowNextDialogueLine());
                }
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJump", false);
        }
    }

    void LoadDialogue(string chapter)
    {
        TextAsset textFile = Resources.Load<TextAsset>("dialogue");
        string[] lines = textFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        bool inChapter = false;
        string currentCharacter = "";

        foreach (var raw in lines)
        {
            string line = raw.Trim();

            if (line.StartsWith("#"))
            {
                if (line.StartsWith("#pause"))
                {
                    dialogueQueue.Enqueue(("", "[pause]"));
                }
                else
                {
                    inChapter = line.Substring(1).Trim() == chapter;
                }
                continue;
            }

            if (!inChapter) continue;

            if (line.StartsWith("@"))
            {
                currentCharacter = line.Substring(1).Trim();
            }
            else
            {
                dialogueQueue.Enqueue((currentCharacter, line));
            }
        }
    }

    IEnumerator ShowNextDialogueLine()
    {
        if (dialogueQueue.Count > 0)
        {
            var line = dialogueQueue.Dequeue();

            if (line.line == "[pause]")
            {
                isPaused = true;
                dialogueSound.Stop();
                istextboxOn = false;
                textbox.SetActive(false);
                yield break;
            }

            textbox.SetActive(true);
            istextboxOn = true;
            characterText.text = line.character;
            currentFullLine = line.line;
            dialogueText.text = "";

            yield return StartCoroutine(TypeLine(line.line));
        }
        else
        {
            textbox.SetActive(false);
            istextboxOn = false;
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        foreach (char c in line)
        {
            dialogueText.text += c;
            if (dialogueSound != null && c != ' ')
                dialogueSound.Play();
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    public void ResumeDialogue()
    {
        if (isPaused)
        {
            isPaused = false;
            StartCoroutine(ShowNextDialogueLine());
        }
    }
}
