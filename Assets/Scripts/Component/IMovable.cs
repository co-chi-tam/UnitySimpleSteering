using System;
using System.Collections;

public interface IMovable {

	string GetID();
	float GetMoveSpeed();
	void SetIsObstacle(bool value);
	bool GetIsObstacle();

}
