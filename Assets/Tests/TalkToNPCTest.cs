using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class InterviewTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void InterviewTestSimplePasses()
        {
            // Use the Assert class to test conditions
            Debug.Log("DOES THIS WORK EDITING KAREEM?!");
            TalkToNPC talkToNPC; 
            // talkToNPC.findMessage([], "weesaw");

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator InterviewTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
