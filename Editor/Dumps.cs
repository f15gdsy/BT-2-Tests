using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BT.Test {

	public static class BTDumpData {
		public static int clearCount;
		public static int activatedCount;
		public static int tickCount;
		public static int checkCount;

		public static void Clear () {
			clearCount = 0;
			activatedCount = 0;
			tickCount = 0;
			checkCount = 0;
		}
	}

	public class BTDumpNode : BTNode {
		public BTResult tickResult;

		public BTDumpNode (BTResult tickResult = BTResult.Failed) {
			this.tickResult = tickResult;
		}

		public override void Activate (BTDatabase database) {
			base.Activate (database);
			BTDumpData.activatedCount++;
		}

		public override BTResult Tick () {
			BTDumpData.tickCount++;
			return tickResult;
		}

		public override void Clear () {
			base.Clear ();
			BTDumpData.clearCount ++;
		}
	}

	public class BTDumpComposite : BTComposite {
		public int selectedChildrenForClearCount {
			get {return selectedChildrenForClear.Count;}
		}
	}

	public class BTDumpConditional : BTConditional {
		public bool checkResult;

		public BTDumpConditional (bool checkResult) {
			this.checkResult = checkResult;
		}

		public override bool Check () {
			BTDumpData.checkCount++;
			return checkResult;
		}
	}

}