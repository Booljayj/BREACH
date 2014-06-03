using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ButtonMessage {
	Null = 0,
	Open,
	Close,
	On,
	Off,
	Lock,
	Unlock,
	Auto,
	Manual,
	Activate,
	Deactivate
}

public enum DoorStateMessage {
	Null = 0,
	Open,
	Close,
	Lock,
	Unlock,
	Auto,
	Manual,

	Unsealing,
	Unsealed,
	Opening,
	Opened,
	Closing,
	Closed,
	Sealing,
	Sealed
}

public enum ProximityMessage {
	Null = 0,
	FarEnter,
	FarExit,
	NearEnter,
	NearExit,
	ObstacleEnter,
	ObstacleExit
}

public enum EmergencyMessage {
	Null = 0,
	LockDown,
	Broken,
}

public enum AirlockMessage {
	Null = 0,
	PartialOpen,
	PartialClose,
	FullOpen,
	FullClose,
}

public class DoorController : MonoBehaviour {
	public Messenger message;

	public bool isLocked = false;
	public bool isOpening = false;
	public bool isAuto = false;
	public int obstacles = 0;

	void Start() {
		if (message == null) {
			enabled = false;
			return;
		}

		message.Register<ButtonMessage>(ProcessButton);
		message.Register<ProximityMessage>(ProcessProximity);
	}

	public void ProcessButton(ButtonMessage m) {
		if (m == ButtonMessage.Lock) {
			isLocked = true;
			message.Send<DoorStateMessage>(DoorStateMessage.Lock);
		} else if (m == ButtonMessage.Unlock) {
			isLocked = false;
			message.Send<DoorStateMessage>(DoorStateMessage.Unlock);

		} else if (m == ButtonMessage.Auto) {
			isAuto = true;
			message.Send<DoorStateMessage>(DoorStateMessage.Auto);
		} else if (m == ButtonMessage.Manual) {
			isAuto = false;
			message.Send<DoorStateMessage>(DoorStateMessage.Manual);

		} else if (m == ButtonMessage.Open && !isLocked) {
			isOpening = true;
			message.Send<DoorStateMessage>(DoorStateMessage.Open);
		} else if (m == ButtonMessage.Close && !isLocked && obstacles <= 0) {
			isOpening = false;
			message.Send<DoorStateMessage>(DoorStateMessage.Close);
		}
	}

	public void ProcessProximity(ProximityMessage m) {
		if (m == ProximityMessage.ObstacleEnter) {
			obstacles++;
			message.Send<DoorStateMessage>(DoorStateMessage.Open);
		} else if (m == ProximityMessage.ObstacleExit) {
			obstacles--;

		} else if (m == ProximityMessage.NearEnter && isAuto) {
			message.Send<ButtonMessage>(ButtonMessage.Open);
		} else if (m == ProximityMessage.NearExit && isAuto) {
			message.Send<ButtonMessage>(ButtonMessage.Close);

		} else if (m == ProximityMessage.FarExit) {
			message.Send<ButtonMessage>(ButtonMessage.Close);
		}
	}
}
