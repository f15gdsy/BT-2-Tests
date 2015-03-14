using UnityEngine;
using System.Collections;
using BT;
using BT.Ex;

public class TestBT : BTTree {

	public float speed;
	public Transform targetTrans;

	public override BTNode Init () {
		base.Init ();


		BTSelector root = new BTSelector();

		BTConditionEvaluator evaluator = new BTConditionEvaluator(BTLogic.And, true);
		evaluator.AddConditional(new BTCheckWithinDistance(_database.transform, 1, targetTrans));
		BTSequence catchSubtree = new BTSequence();
		{
			catchSubtree.AddChild(new BTActionLog("Haha, got you!"));
			catchSubtree.AddChild(new BTActionWait(10000));
		}
		evaluator.child = catchSubtree;
		root.AddChild(evaluator);

		BTActionMove moveToTarget = new BTActionMove(_database.transform, speed, 0.1f, targetTrans, BTDataReadOpt.ReadEveryTick);
		root.AddChild(moveToTarget);

		return root;

		// Static
//		BTSequence followSubtree = new BTSequence();
//		{
//			followSubtree.AddChild(new BTCheckWithinDistance(_database.transform, 1, targetTrans));
//			followSubtree.AddChild(new BTActionLog("Haha, got you!"));
//			followSubtree.AddChild(new BTActionWait(10000));
//		}
//		root.AddChild(followSubtree);
//
//		BTActionMove moveToTarget = new BTActionMove(_database.transform, speed, 0.1f, targetTrans, BTDataReadOpt.ReadEveryTick);
//		root.AddChild(moveToTarget);
	}
}
