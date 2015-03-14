using UnityEngine;
using System.Collections;
using NUnit.Framework;
using BT;

namespace BT.Test {

	[TestFixture]
	public class SequenceTest {
		BTSequence sequence;
		BTDumpNode nodeA;
		BTDumpNode nodeB;
		BTDumpNode nodeC;
		BTDumpNode nodeD;
		
		[SetUp]
		public void Init () {
			sequence = new BTSequence();
			nodeA = new BTDumpNode(BTResult.Success);
			nodeB = new BTDumpNode(BTResult.Success);
			nodeC = new BTDumpNode(BTResult.Running);
			nodeD = new BTDumpNode(BTResult.Success);
			
			sequence.AddChild(nodeA);
			sequence.AddChild(nodeB);
			sequence.AddChild(nodeC);
			sequence.AddChild(nodeD);
			
			BTDumpData.Clear();
		}
		
		[Test]
		public void TestTickDefault () {
			sequence.Tick();
			Assert.AreEqual(2, sequence.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);	// successed nodes are cleared
		}
		
		[Test]
		public void TestTickSuccessAll () {
			nodeC.tickResult = BTResult.Success;
			sequence.Tick();
			Assert.AreEqual(-1, sequence.activeChildIndex);
			Assert.AreEqual(4, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			sequence.clearOpt = BTClearOpt.All;
			sequence.Clear();
			Assert.AreEqual(0, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			sequence.clearOpt = BTClearOpt.Selected;
			sequence.Clear();
			Assert.AreEqual(0, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			sequence.clearOpt = BTClearOpt.DefaultAndSelected;
			sequence.Clear();
			Assert.AreEqual(0, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestTickFailed () {
			nodeB.tickResult = BTResult.Failed;
			sequence.Tick();
			Assert.AreEqual(-1, sequence.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestTickSuccessedChildBecomeFailed () {
			// As the current active child is B, so A's failure wont affect 
			sequence.Tick();
			Assert.AreEqual(2, sequence.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			nodeA.tickResult = BTResult.Failed;
			sequence.Tick();
			Assert.AreEqual(2, sequence.activeChildIndex);
			Assert.AreEqual(0, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestTickAbort () {
			sequence.Tick();
			Assert.AreEqual(2, sequence.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			sequence.Clear();
			Assert.AreEqual(-1, sequence.activeChildIndex);
			Assert.AreEqual(3, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestClearDefault () {
			// Default ClearOpt
			Assert.AreEqual(sequence.clearOpt, BTClearOpt.Default);
			
			sequence.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);	// successed nodes are cleared
			sequence.Clear();
			Assert.AreEqual(3, BTDumpData.clearCount);	// abort causes the sequence's Clear to be called by parent
		}
		
		[Test]
		public void TestClearSelected () {
			sequence.clearOpt = BTClearOpt.Selected;
			sequence.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);
			sequence.Clear();
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			sequence.RemoveChild(nodeB);
			sequence.RemoveChild(nodeC);
			sequence.RemoveChild(nodeD);
			sequence.AddChild(nodeB, true);
			sequence.AddChild(nodeC, true);
			sequence.AddChild(nodeD);
			sequence.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);
			sequence.Clear();
			Assert.AreEqual(3, BTDumpData.clearCount);

			BTDumpData.Clear();
			sequence.Clear();
			Assert.AreEqual(2, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestClearDefaultAndSelected () {
			sequence.clearOpt = BTClearOpt.DefaultAndSelected;
			sequence.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);
			sequence.Clear();
			Assert.AreEqual(3, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			sequence.RemoveChild(nodeB);
			sequence.RemoveChild(nodeC);
			sequence.RemoveChild(nodeD);
			sequence.AddChild(nodeB, true);
			sequence.AddChild(nodeC, true);
			sequence.AddChild(nodeD, true);
			sequence.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);
			sequence.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestClearAll () {
			sequence.clearOpt = BTClearOpt.All;
			sequence.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);
			sequence.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			sequence.RemoveChild(nodeB);
			sequence.RemoveChild(nodeC);
			sequence.RemoveChild(nodeD);
			sequence.AddChild(nodeB, true);
			sequence.AddChild(nodeC, true);
			sequence.AddChild(nodeD, true);
			sequence.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);
			sequence.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);
		}
	}

}