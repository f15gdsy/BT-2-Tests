using UnityEngine;
using System.Collections;
using BT.Test;
using NUnit.Framework;

namespace BT.Test {

	[TestFixture]
	public class ConditionalEvaludatorTest {
		private BTConditionEvaluator evaluator;
		private BTDumpConditional conditionalA;
		private BTDumpConditional conditionalB;
		private BTDumpConditional conditionalC;
		private BTDumpNode nodeA;

		[SetUp]
		public void Init () {
			nodeA = new BTDumpNode(BTResult.Running);
			conditionalA = new BTDumpConditional(true);
			conditionalB = new BTDumpConditional(true);
			conditionalC = new BTDumpConditional(false);
			evaluator = new BTConditionEvaluator(BTLogic.And, false, nodeA);

			BTDumpData.Clear();
		}

		[Test]
		public void TestLogicAnd () {
			evaluator.AddConditional(conditionalA);
			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			nodeA.tickResult = BTResult.Failed;
			Assert.AreEqual(BTResult.Failed, evaluator.Tick());
			nodeA.tickResult = BTResult.Success;
			Assert.AreEqual(BTResult.Success, evaluator.Tick());

			evaluator.AddConditional(conditionalB);
			Assert.AreEqual(BTResult.Success, evaluator.Tick());

			evaluator.AddConditional(conditionalC);
			Assert.AreEqual(BTResult.Failed, evaluator.Tick());
		}

		[Test]
		public void TestLogicOr () {
			evaluator.logicOpt = BTLogic.Or;
			evaluator.AddConditional(conditionalA);
			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			nodeA.tickResult = BTResult.Failed;
			Assert.AreEqual(BTResult.Failed, evaluator.Tick());
			nodeA.tickResult = BTResult.Success;
			Assert.AreEqual(BTResult.Success, evaluator.Tick());
			
			evaluator.AddConditional(conditionalB);
			Assert.AreEqual(BTResult.Success, evaluator.Tick());
			
			evaluator.AddConditional(conditionalC);
			Assert.AreEqual(BTResult.Success, evaluator.Tick());
		}

		[Test]
		public void TestReevaluateEveryTick () {
			evaluator.AddConditional(conditionalA);
			evaluator.AddConditional(conditionalB);

			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			Assert.AreEqual(2, BTDumpData.checkCount);

			BTDumpData.Clear();

			evaluator.reevaludateEveryTick = true;

			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			Assert.AreEqual(BTResult.Running, evaluator.Tick());
			Assert.AreEqual(8, BTDumpData.checkCount);
		}
	}

}