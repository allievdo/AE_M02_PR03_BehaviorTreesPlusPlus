using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Door theDoor;
    public GameObject theTreasure;
    public GameObject theConfetti;
    public GameObject theGiveUpText;
    public GameObject theSlipAudio;

    public int randNum;

    bool executingBehavior = false;

    Task myCurrentTask;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            randNum = Random.Range(0, 2);
            Debug.Log(randNum);
            if(!executingBehavior)
            {
                Debug.Log("Started behavior");
                executingBehavior = true;
                myCurrentTask = BuildTask_GetTreasure();

                //EventBus.StartListening(myCurrentTask.TaskFinished, OnTaskFinished);
                myCurrentTask.run();
                Debug.Log("Finished");
            }
        }
    }

    void OnTaskFinished()
    {
        //EventBus.StopListening(myCurrentTask.TaskFinished, OnTaskFinished);
        executingBehavior = false;
    }

    Task BuildTask_GetTreasure()
    {
        //create the behavior tree
        //build from bottom up
        List<Task> taskList = new List<Task>();

        //if door isn't locked, open it
        Task isDoorNotLocked = new IsFalse(theDoor.isLocked);
        Task waitABeat = new Wait(0.5f);
        Task openDoor = new OpenDoor(theDoor);
        taskList.Add(isDoorNotLocked);
        taskList.Add(waitABeat);
        taskList.Add(openDoor);
        Debug.Log("Door Opened");
        Sequence openUnlockedDoor = new Sequence(taskList);

        //barge a closed door
        taskList = new List<Task>();
        Task isDoorClosed = new IsTrue(theDoor.isClosed);
        Task giveUp = new GiveUpAndGoHome(this.gameObject, theGiveUpText, theSlipAudio);
        Task changeMadColor = new ChangeMadColor(this.gameObject);
        Task changeHappyColor = new ChangeHappyColor(this.gameObject);
        Task confetti = new RainConfetti(theConfetti);
        Task bargeDoor = new BargeDoor(theDoor.transform.GetChild(0).GetComponent<Rigidbody>());
        taskList.Add(isDoorClosed);
        //check random number
        if (randNum == 0)
        {
            taskList.Add(waitABeat);
            taskList.Add(giveUp); //give up
            //taskList.Add(giveUp); //give up
            taskList.Add(waitABeat);
            taskList.Add(waitABeat);
        }

        if (randNum == 1)
        {
            taskList.Add(waitABeat);
            taskList.Add(changeMadColor);
            taskList.Add(waitABeat);
            taskList.Add(bargeDoor);
            taskList.Add(waitABeat);
            taskList.Add(changeHappyColor); //celebrate after kicking down door
            //taskList.Add(confetti);
        }
        Sequence bargeClosedDoor = new Sequence(taskList);

        //open a closed door, somehow
        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeClosedDoor);
        Selector openTheDoor = new Selector(taskList);

        //get the treasure when the door is closed
        taskList = new List<Task>();
        Task moveToDoor = new MoveKinematicToObject(this.GetComponent<Kinematic>(), theDoor.gameObject);
        Task moveToTreasure = new MoveKinematicToObject(this.GetComponent<Kinematic>(), theTreasure.gameObject);
        taskList.Add(moveToDoor);
        taskList.Add(waitABeat);
        taskList.Add(openTheDoor);
        taskList.Add(waitABeat);
        taskList.Add(moveToTreasure);
        taskList.Add(confetti);
        Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

        //get the treasure when the door is open
        taskList = new List<Task>();
        Task isDoorOpen = new IsFalse(theDoor.isClosed);
        taskList.Add(isDoorOpen);
        taskList.Add(moveToTreasure);
        taskList.Add(confetti);
        Sequence getTreasureBehindOpenDoor = new Sequence(taskList);

        //get the treasure somehow
        taskList = new List<Task>();
        taskList.Add(getTreasureBehindOpenDoor);
        taskList.Add(getTreasureBehindClosedDoor);
        Selector getTreasure = new Selector(taskList);

        return getTreasure;
    }
}
