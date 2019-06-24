# Mobile Phone G-Force Event Capture
This is a Windows Phone app designed to create random JSON event telemetry based on the phone accelerometer (X, Y, Z and G) and send the events as a JSON string to an Azure Event Hub for downstream processing.

Examples for the usage of this solution can be located in my blog;

https://mrfoxsql.wordpress.com/2017/05/31/making-phone-calls-from-azure-event-hub-messages/

https://mrfoxsql.files.wordpress.com/2018/06/pass-sqlsaturday-melbourne-azure-data-pipelines-v0-1.pdf


## Solution Usage
The below diagram shows the downstream solution (not part of this git repo)

![alt text](https://github.com/rolftesmer/PhoneCaptureGForce/blob/master/media/architecture.jpg)

On Deployment, the phone app will look like this draft design.

![alt text](https://github.com/rolftesmer/PhoneCaptureGForce/blob/master/media/mobile.jpg)

By app has 2 possible send rates; 
(1) 2 events/sec
(2) the maximum amount of events the phone hardware can handle (typically 30-50/sec)

The phone app will create and send a JSON event that looks like this sample;
{
	"DataTypeKey": "ACC", 
	"Count": 55, 
	"X": 0.12, 
	"Y": 1.63, 
	"Z": -0.67, 
	"G": 3.93
}

The target Azure Event Hub, or Azure IoT Hub, that the event will be sent to is defined and hard-coded in the phone app code in the variable "deviceConnectionString" in the c# file "MainPage.xaml.cs".  To chnage the send target the phone app needs to be updated, recompiled and redeployed to the mobile device.

Once the data is received in the Azure Event Hub then it can be processed any number of ways.
(1) Azure Stream Analytics
(2) Azure Functions
(3) Azure Databricks
