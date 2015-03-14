using UnityEngine;
using System.Collections;
using NUnit.Framework;
using BT;

namespace BT.Test {
	[TestFixture]
	public class SelectorTest {
		BTSelector selector;
		BTDumpNode nodeA;
		BTDumpNode nodeB;
		BTDumpNode nodeC;
		BTDumpNode nodeD;
		
		[SetUp]
		public void Init () {
			selector = new BTSelector();
			nodeA = new BTDumpNode(BTResult.Failed);
			nodeB = new BTDumpNode(BTResult.Success);
			nodeC = new BTDumpNode(BTResult.Running);
			nodeD = new BTDumpNode(BTResult.Failed);
			
			selector.AddChild(nodeA);
			selector.AddChild(nodeB);
			selector.AddChild(nodeC);
			selector.AddChild(nodeD);
			
			BTDumpData.Clear();
		}
		
		[Test]
		public void TestTickDefault () {
			selector.Tick();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);	// fail & successful nodes are cleared
		}
		
		[Test]
		public void TestTickFailedAll () {
			nodeB.tickResult = BTResult.Failed;
			nodeC.tickResult = BTResult.Failed;
			selector.Tick();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(4, BTDumpData.clearCount);

			BTDumpData.Clear();
			selector.clearOpt = BTClearOpt.Selected;
			selector.Tick();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(4, BTDumpData.clearCount);

			BTDumpData.Clear();
			selector.clearOpt = BTClearOpt.DefaultAndSelected;
			selector.Tick();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(4, BTDumpData.clearCount);

			BTDumpData.Clear();
			selector.clearOpt = BTClearOpt.All;
			selector.Tick();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(4, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestTickRunning () {
			nodeB.tickResult = BTResult.Failed;
			selector.Tick();
			Assert.AreEqual(2, selector.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestTickSuccessedChildBecomeFailedAndViseVersa () {
			selector.Tick();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			nodeB.tickResult = BTResult.Failed;
			selector.Tick();
			Assert.AreEqual(2, selector.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			nodeA.tickResult = BTResult.Running;
			selector.Tick();
			Assert.AreEqual(0, selector.activeChildIndex);
			Assert.AreEqual(1, BTDumpData.clearCount);	// Clear the previous active node
			
			BTDumpData.Clear();
			
			nodeA.tickResult = BTResult.Success;
			selector.Tick();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(1, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestTickAbort () {
			selector.Tick();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			selector.Clear();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			nodeB.tickResult = BTResult.Failed;
			selector.Tick();
			Assert.AreEqual(2, selector.activeChildIndex);
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			selector.Clear();
			Assert.AreEqual(-1, selector.activeChildIndex);
			Assert.AreEqual(3, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestClearDefault () {
			Assert.AreEqual(selector.clearOpt, BTClearOpt.Default);
			
			selector.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);	// successed & failed nodes are cleared
			selector.Clear();
			Assert.AreEqual(2, BTDumpData.clearCount);	// abort causes the sequence's Clear to be called by parent
			
			BTDumpData.Clear();
			
			nodeB.tickResult = BTResult.Failed;
			selector.Tick();
			selector.Clear();
			Assert.AreEqual(3, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestClearSelected () {
			selector.clearOpt = BTClearOpt.Selected;
			selector.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);
			selector.Clear();
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			BTDumpData.Clear();

			selector = new BTSelector();
			selector.clearOpt = BTClearOpt.Selected;
			selector.AddChild(nodeA);
			selector.AddChild(nodeB);
			selector.AddChild(nodeC, true);
			selector.AddChild(nodeD, true);
			selector.Clear();
			Assert.AreEqual(2, BTDumpData.clearCount);

			BTDumpData.Clear();
			selector.Tick();
			selector.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);

			BTDumpData.Clear();
			nodeB.tickResult = BTResult.Running;
			selector.Tick();
			selector.Clear();
			Assert.AreEqual(3, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestClearDefaultAndSelected () {
			selector.clearOpt = BTClearOpt.DefaultAndSelected;
			selector.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);
			selector.Clear();
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			selector = new BTSelector();
			selector.clearOpt = BTClearOpt.DefaultAndSelected;
			selector.AddChild(nodeA, true);
			selector.AddChild(nodeB);
			selector.AddChild(nodeC, true);
			selector.AddChild(nodeD, true);
			selector.Tick();
			selector.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);

			BTDumpData.Clear();
			nodeB.tickResult = BTResult.Running;
			selector.Tick();
			selector.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			selector.Clear();
			Assert.AreEqual(3, BTDumpData.clearCount);
		}
		
		[Test]
		public void TestClearAll () {
			selector.clearOpt = BTClearOpt.All;
			selector.Tick();
			Assert.AreEqual(2, BTDumpData.clearCount);
			selector.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			
			selector = new BTSelector();
			selector.clearOpt = BTClearOpt.All;
			selector.AddChild(nodeA, true);
			selector.AddChild(nodeB);
			selector.AddChild(nodeC, true);
			selector.AddChild(nodeD, true);
			selector.Tick();
			selector.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			nodeB.tickResult = BTResult.Running;
			selector.Tick();
			selector.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			selector.Clear();
			Assert.AreEqual(4, BTDumpData.clearCount);
		}
	}
}