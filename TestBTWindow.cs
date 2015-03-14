using UnityEngine;
using System.Collections;
using BT;
using BT.Ex;


public class TestBTWindow : BTTree {

	public override BTNode Init () {
		base.Init ();

		var node0_0 = new BTSelector();

		var node1_0 = new BTSequence();
		{
			var node2_0 = new BTSimpleParallel();
			{
				node2_0.SetPrimaryChild(new BTActionLog("3_primary"));
				node2_0.AddChild(new BTActionLog("3_0"));
				node2_0.AddChild(new BTActionLog("3_1"));
				node2_0.AddChild(new BTActionLog("3_2"));
				node2_0.AddChild(new BTActionLog("3_3"));
			}
			node1_0.AddChild(node2_0);

			var node2_1 = new BTInverter();
			var node3_4 = new BTSelector();
			{
				node3_4.AddChild(new BTActionLog("4_0"));
				node3_4.AddChild(new BTActionLog("4_1"));
				node3_4.AddChild(new BTActionLog("4_2"));
			}
			node2_1.child = node3_4;
			node1_0.AddChild(node2_1);
		}
		node0_0.AddChild(node1_0);

		var node1_1 = new BTSimpleParallel();
		{
			node1_1.SetPrimaryChild(new BTActionLog("2_primary"));
			node1_1.AddChild(new BTActionLog("2_2"));
			node1_1.AddChild(new BTActionLog("2_3"));

			var node2_4 = new BTRepeater(true);
			var node3_5 = new BTSelector();
			{
				node3_5.AddChild(new BTActionLog("4_3"));

				var node4_4 = new BTSimpleParallel();
				{
					node4_4.SetPrimaryChild(new BTActionLog("5_primary"));
					node4_4.AddChild(new BTActionLog("5_0"));
					node4_4.AddChild(new BTActionLog("5_1"));
					node4_4.AddChild(new BTActionLog("5_2"));
					node4_4.AddChild(new BTActionLog("5_3"));
					node4_4.AddChild(new BTActionLog("5_4"));
				}
				node3_5.AddChild(node4_4);

				node3_5.AddChild(new BTTimer(3, new BTActionLog("5_5")));
			}
			node2_4.child = (node3_5);
			node1_1.AddChild(node2_4);
		}
		node0_0.AddChild(node1_1);

		return node0_0;
	}
}
