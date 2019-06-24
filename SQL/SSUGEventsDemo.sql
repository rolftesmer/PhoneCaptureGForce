-- truncate table [dbo].[SSUGEvents]
-- truncate table [dbo].[SSUGEventAlerts]

/*
select count(*) from [dbo].[SSUGEvents]
select * from [dbo].[SSUGEvents] order by [SQLCreatedDateTime] desc, SSUGEventsKey desc
select * from [dbo].[SSUGEvents] where G > 3.00 order by G desc, [SQLCreatedDateTime] desc
select * from [dbo].[SSUGEventAlerts] order by [SQLCreatedDateTime] desc, SSUGEventsKey desc
*/

select	getdate() as SQLCurrentDateTime,
		count(*) as TotEventRows,
		--cast((max([SQLCreatedDateTime]) - min([SQLCreatedDateTime])) as time) as TimeRangeinDB,
		--min([SQLCreatedDateTime]) as FirstSQLEventTime,
		cast(max([SQLCreatedDateTime]) - getdate() as time) as TimeSinceLastEvent,
		max([SQLCreatedDateTime]) as LastSQLEventTime,
		max([AEHCreatedDateTime]) as LastAEHEventTime,
		cast(max([SQLCreatedDateTime]) - max([AEHCreatedDateTime]) as time) as LastEventDelay,
		(
			(
				select	count(*)
				from	[dbo].[SSUGEvents] with (NOLOCK) 
				where	[SQLCreatedDateTime] >= dateadd(ss, -1, (select max([SQLCreatedDateTime]) from [dbo].[SSUGEvents] with (NOLOCK)))
			)
		) as [Events/Sec],
		(
			(
				select	count(*)
				from	[dbo].[SSUGEvents] with (NOLOCK) 
				where	[SQLCreatedDateTime] >= dateadd(ss, -60, (select max([SQLCreatedDateTime]) from [dbo].[SSUGEvents] with (NOLOCK)))
			)
		) as [Events/Min],
		min([G]) as MinGEvent,
		max([G]) as MaxGEvent,
		cast(avg([G]) as numeric(15,2)) as AvgGEvent,
		(
			(
				select	count(*)
				from	[dbo].[SSUGEvents] with (NOLOCK) 
				where	G >= 0
				and		G <= 1
			)
		) as GEvents0to1,
		(
			(
				select	count(*)
				from	[dbo].[SSUGEvents] with (NOLOCK) 
				where	G > 1
				and		G <= 2
			)
		) as GEvents1to2,
		(
			(
				select	count(*)
				from	[dbo].[SSUGEvents] with (NOLOCK) 
				where	G > 2
				and		G <= 3
			)
		) as GEvents2to3,
		(
			(
				select	count(*)
				from	[dbo].[SSUGEvents] with (NOLOCK) 
				where	G >= 3
			)
		) as GEvents3Plus
from	[dbo].[SSUGEvents] with (NOLOCK) 
GO

--select	top 5 * from	[dbo].[SSUGEvents] with (NOLOCK) order by [count] desc
select	top 5 * from	[dbo].[SSUGEventAlerts] with (NOLOCK) order by AEHCreatedDateTime desc
--select	max([count]) as MaxCount , min([count]) as MinCount from	[dbo].[SSUGEvents] with (NOLOCK) 
--select	* from	[dbo].[SSUGEvents] with (NOLOCK) order by [count] desc
GO
