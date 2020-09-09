using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class UserDefine : MonoBehaviour, IUserDefinedTargetEventHandler
{
	public ImageTargetBehaviour imageTargetTemplate;
	private UserDefinedTargetBuildingBehaviour targetBuildingBehaviour;
	private ObjectTracker objectTracker;
	private DataSet dataSet;
	private int targetCounter = 0;
	private ImageTargetBuilder.FrameQuality currentQuality;
	
	void Start()
	{
		targetBuildingBehaviour = GetComponent<UserDefinedTargetBuildingBehaviour>();
		targetBuildingBehaviour.RegisterEventHandler(this);
	}
	public void OnInitialized()
	{
		objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
		if (objectTracker != null)
		{
			dataSet = objectTracker.CreateDataSet();
			objectTracker.ActivateDataSet(dataSet);
		}
	}
	public void OnFrameQualityChanged(ImageTargetBuilder.FrameQuality frameQuality)
	{
		currentQuality = frameQuality;
	}
	public void OnNewTrackableSource(TrackableSource trackableSource)
	{
		targetCounter++;
		objectTracker.DeactivateDataSet(dataSet);
		ImageTargetBehaviour imageTargetCopy = Instantiate(imageTargetTemplate);
		imageTargetCopy.gameObject.name = "UserTarget-" + targetCounter;
		dataSet.CreateTrackable(trackableSource, imageTargetCopy.gameObject);
		objectTracker.ActivateDataSet(dataSet);
	}
	public void BuildNewTarget()
	{
		string targetName = string.Format(imageTargetTemplate.TrackableName + targetCounter);
		targetBuildingBehaviour.BuildNewTarget(targetName, imageTargetTemplate.GetSize().x);
	}
}
