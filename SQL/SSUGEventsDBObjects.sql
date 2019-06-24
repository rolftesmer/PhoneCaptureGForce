--------------------------------------------------------------------------------
drop table dbo.SSUGEvents
GO

create table dbo.SSUGEvents
(
	SSUGEventsKey		bigint		NOT NULL	identity(1, 1),
	DataTypeKey			varchar(5)	NULL,
	[Count]				int			NULL,
	X					float		NULL,
	Y					float		NULL,
	Z					float		NULL,
	G					float		NULL,
	G_MLPrediction		float		NULL,
	AEHCreatedDateTime	datetime	NULL,
	SQLCreatedDateTime	datetime	NULL	default (getdate())
)
GO


drop table dbo.SSUGEventAlerts
GO

create table dbo.SSUGEventAlerts
(
	SSUGEventsKey		bigint			NOT NULL	identity(1, 1),
	DataTypeKey			varchar(5)		NULL,
	X					float			NULL,
	Y					float			NULL,
	Z					float			NULL,
	G					float			NULL,
	G_MLPrediction		float			NULL,
	AlertToPhoneNbr		varchar(250)	NULL,
	AlertMessageTxt		varchar(250)	NULL,
	AEHCreatedDateTime	datetime		NULL,
	SQLCreatedDateTime	datetime		NULL	default (getdate())
)
GO

