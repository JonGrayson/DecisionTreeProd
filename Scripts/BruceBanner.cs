using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruceBanner : MonoBehaviour
{
    public Door theDoor;
    public GameObject theDoorObject;
    public GameObject theTreasure;
    public ParticleSystem particle;
    //public GameObject test;
    bool executingBehavior = false;
    Task myCurrentTask;

    void Start()
    {
        particle.Stop();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            if (!executingBehavior)
            {
                executingBehavior = true;
                myCurrentTask = BuildTask_GetTreasure();

                EventBus.StartListening(myCurrentTask.TaskFinished, OnTaskFinished);
                myCurrentTask.run();
            }
        }
    }

    void OnTaskFinished()
    {
        EventBus.StopListening(myCurrentTask.TaskFinished, OnTaskFinished);
        executingBehavior = false;
    }

    Task BuildTask_GetTreasure()
    {
        List<Task> taskList = new List<Task>();

        Task isDoorNotLocked = new IsFalse(theDoor.isLocked);
        Task waitABeat = new Wait(0.5f);
        Task openDoor = new OpenDoor(theDoor);
        Task particles = new Particles(particle);
        Task moveToTreasure = new MoveKinematicToObject(this.GetComponent<Kinematic>(), theTreasure.gameObject);
        Task hideOpenDoor = new HideOpenDoor(theDoorObject);

        taskList.Add(isDoorNotLocked);
        taskList.Add(waitABeat);
        taskList.Add(openDoor);
        taskList.Add(waitABeat);
        taskList.Add(moveToTreasure);
        taskList.Add(particles);
        Sequence openUnlockedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorClosed = new IsTrue(theDoor.isClosed);
        Task hulkOut = new HulkOut(this.gameObject);
        Task bargeDoor = new BargeDoor(theDoor.transform.GetChild(0).GetComponent<Rigidbody>());
        Task returnToNormal = new returnToNormal(this.gameObject);
        taskList.Add(isDoorClosed);
        taskList.Add(waitABeat);
        taskList.Add(hulkOut);
        taskList.Add(waitABeat);
        taskList.Add(bargeDoor);
        taskList.Add(waitABeat);
        taskList.Add(returnToNormal);
        taskList.Add(waitABeat);
        taskList.Add(moveToTreasure);
        taskList.Add(particles);
        Sequence bargeClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeClosedDoor);
        Selector openTheDoor = new Selector(taskList);

        taskList = new List<Task>();
        Task moveToDoor = new MoveKinematicToObject(this.GetComponent<Kinematic>(), theDoor.gameObject);
        taskList.Add(moveToDoor);
        taskList.Add(waitABeat);
        taskList.Add(openTheDoor);
        taskList.Add(hideOpenDoor);
        taskList.Add(waitABeat);
        taskList.Add(moveToTreasure);
        taskList.Add(waitABeat);
        taskList.Add(particles);
        Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorOpen = new IsFalse(theDoor.isClosed);
        taskList.Add(isDoorOpen);
        taskList.Add(hideOpenDoor);
        taskList.Add(moveToTreasure);
        taskList.Add(particles);
        Sequence getTreasureBehindOpenDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(getTreasureBehindOpenDoor);
        taskList.Add(getTreasureBehindClosedDoor);
        Selector getTreasure = new Selector(taskList);

        return getTreasure;
    }
}
