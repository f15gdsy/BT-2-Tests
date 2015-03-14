using UnityEngine;
using System.Collections;
using NUnit.Framework;
using BT;

namespace BT.Test {

	[TestFixture]
	public class CompositeTest {

		BTDumpComposite composite;
		BTDumpNode nodeA;
		BTDumpNode nodeB;
		BTDumpNode nodeC;
		BTDumpNode nodeD;

		[SetUp]
		public void Init () {
			composite = new BTDumpComposite();
			nodeA = new BTDumpNode();
			nodeB = new BTDumpNode();
			nodeC = new BTDumpNode();
			nodeD = new BTDumpNode();
			 
			BTDumpData.Clear();
		}

		[Test]
		public void TestAddRemoveChildren () {
			// Add child
			composite.AddChild(nodeA);
			Assert.AreEqual(1, composite.children.Count);

			composite.AddChild(nodeB);
			Assert.AreEqual(2, composite.children.Count);

			// Add child & select it for clear
			composite.AddChild(nodeA, true);
			Assert.AreEqual(3, composite.children.Count);
			Assert.AreEqual(1, composite.selectedChildrenForClearCount);

			// Nothing happens if trying to add null
			composite.AddChild(null);
			Assert.AreEqual(3, composite.children.Count);

			// Nothing happens if trying to add not-added node
			composite.RemoveChild(new BTDumpNode());
			Assert.AreEqual(3, composite.children.Count);

			// Remove child
			composite.RemoveChild(nodeA);
			Assert.AreEqual(2, composite.children.Count);
		}

		[Test]
		public void TestActivated () {
			composite.AddChild(nodeA);
			composite.AddChild(nodeB);
			composite.AddChild(nodeC);
			composite.AddChild(nodeD);
			composite.Activate(null);
			Assert.AreEqual(4, BTDumpData.activatedCount);
		}
	}

}