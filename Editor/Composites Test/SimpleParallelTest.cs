using UnityEngine;
using System.Collections;
using NUnit.Framework;


namespace BT.Test {

	[TestFixture]
	public class SimpleParallelTest {
		BTSimpleParallel parallel;
		BTDumpNode nodeA;
		BTDumpNode nodeB;
		BTDumpNode nodeC;

		[SetUp]
		public void Init () {
			BTDumpData.Clear();

			parallel = new BTSimpleParallel();
			nodeA = new BTDumpNode(BTResult.Running);
			nodeB = new BTDumpNode(BTResult.Running);
			nodeC = new BTDumpNode(BTResult.Running);

			parallel.SetPrimaryChild(nodeA, true);
			parallel.AddChild(nodeB);
			parallel.AddChild(nodeC, true);

			parallel.Activate(null);
		}

		[Test]
		public void TestActivate () {
			Assert.AreEqual(3, BTDumpData.activatedCount);
		}

		[Test]
		public void TestTick () {
			parallel.Tick();
			Assert.AreEqual(3, BTDumpData.tickCount);

			BTDumpData.Clear();
			nodeB.tickResult = BTResult.Success;
			parallel.Tick();
			Assert.AreEqual(3, BTDumpData.tickCount);
			BTDumpData.Clear();
			parallel.Tick();
			Assert.AreEqual(2, BTDumpData.tickCount);

			BTDumpData.Clear();
			nodeA.tickResult = BTResult.Success;
			parallel.Tick();
			Assert.AreEqual(1, BTDumpData.tickCount);
		}

		[Test]
		public void TestClearNotSelected () {
			parallel.Clear();
			Assert.AreEqual(3, BTDumpData.clearCount);

			BTDumpData.Clear();
			nodeC.tickResult = BTResult.Failed;
			parallel.Tick();
			Assert.AreEqual(3, BTDumpData.tickCount);
			Assert.AreEqual(1, BTDumpData.clearCount);
			parallel.Clear();
			Assert.AreEqual(3, BTDumpData.clearCount);

			BTDumpData.Clear();
			nodeA.tickResult = BTResult.Failed;
			parallel.Tick();
			Assert.AreEqual(1, BTDumpData.tickCount);
			Assert.AreEqual(1, BTDumpData.clearCount);
			parallel.Clear();
			Assert.AreEqual(3, BTDumpData.clearCount);
		}

		[Test]
		public void TestClearSelected () {
			parallel.clearOpt = BTClearOpt.Selected;

			parallel.Clear();
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			nodeC.tickResult = BTResult.Failed;
			parallel.Tick();
			Assert.AreEqual(3, BTDumpData.tickCount);
			Assert.AreEqual(1, BTDumpData.clearCount);
			parallel.Clear();
			Assert.AreEqual(2, BTDumpData.clearCount);
			
			BTDumpData.Clear();
			nodeA.tickResult = BTResult.Failed;
			parallel.Tick();
			Assert.AreEqual(1, BTDumpData.tickCount);
			Assert.AreEqual(1, BTDumpData.clearCount);
			parallel.Clear();
			Assert.AreEqual(2, BTDumpData.clearCount);
		}
	}

}