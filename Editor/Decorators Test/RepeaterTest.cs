using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace BT.Test {

	[TestFixture]
	public class RepeaterTest {
		private BTDumpNode nodeA;

		[SetUp]
		public void Init () {
			nodeA = new BTDumpNode(BTResult.Success);

			BTDumpData.Clear();
		}

		[Test]
		public void TestCount () {
			BTRepeater repeater = new BTRepeater(0, false);
			repeater.child = nodeA;

			Assert.AreEqual(BTResult.Failed, repeater.Tick());
			Assert.AreEqual(BTResult.Failed, repeater.Tick());
			Assert.AreEqual(0, BTDumpData.tickCount);
			repeater.Clear();
			Assert.AreEqual(1, BTDumpData.clearCount);

			BTDumpData.Clear();

			repeater.count = 5;
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Success, repeater.Tick());
			Assert.AreEqual(BTResult.Failed, repeater.Tick());
			Assert.AreEqual(5, BTDumpData.tickCount);
			repeater.Clear();


			BTDumpData.Clear();

			repeater.endOnFailure = true;
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			nodeA.tickResult = BTResult.Failed;
			Assert.AreEqual(BTResult.Failed, repeater.Tick());
			Assert.AreEqual(4, BTDumpData.tickCount);
			repeater.Clear();

			BTDumpData.Clear();

			Assert.AreEqual(BTResult.Failed, repeater.Tick());
			Assert.AreEqual(1, BTDumpData.tickCount);
		}

		[Test]
		public void TestRepeaterForever () {
			BTRepeater repeater = new BTRepeater(false);
			repeater.child = nodeA;
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(6, BTDumpData.tickCount);

			repeater.Clear();
			Assert.AreEqual(1, BTDumpData.clearCount);

			BTDumpData.Clear();

			repeater.endOnFailure = true;
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			Assert.AreEqual(BTResult.Running, repeater.Tick());
			nodeA.tickResult = BTResult.Failed;
			Assert.AreEqual(BTResult.Failed, repeater.Tick());
			Assert.AreEqual(5, BTDumpData.tickCount);
			
			repeater.Clear();
			Assert.AreEqual(1, BTDumpData.clearCount);
		}
	}

}