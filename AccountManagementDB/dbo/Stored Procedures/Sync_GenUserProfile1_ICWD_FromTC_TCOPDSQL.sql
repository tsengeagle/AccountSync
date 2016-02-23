-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sync_GenUserProfile1_ICWD_FromTC_TCOPDSQL]
AS
BEGIN

declare @RowCount int
set @RowCount=0

declare @chUserID varchar(10)
declare @chXData varchar(32)
declare @chUserName	nvarchar(10)
declare @chEndDate char(7)
declare @dtLastModified datetime
declare @chXDataHosp char(5)
declare @chAction varchar(10)
declare @chUserID10 char(10)

/*抓最後一次處理時間*/
declare @dtLastProcess datetime

select top 1
	@dtLastProcess=dtProcessStart 
from ProxyAccountProcessRecord
where isSuccess=1 and chProcessHosp='TC' and nvProcessJob like '同步%'
order by dtProcessStart desc

if ISNULL(@dtLastProcess,'')=''
begin
	print 'null'
	set @dtLastProcess='2000/1/1 00:00:00'
end
else
begin
	print 'not null'
end

select @dtLastProcess
/*抓最後一次處理時間*/


/*寫處理記錄開始時間*/
declare @dtStart datetime
set @dtStart=getdate()
select @dtStart

insert ProxyAccountProcessRecord
(dtProcessStart,chProcessHosp,nvProcessJob)
values(@dtStart,'TC','同步')
/*寫處理記錄開始時間*/

declare list cursor for

select 
	chUserID             =ltrim(rtrim(chUserID)) ,
	chXData 			 =ltrim(rtrim(chXData)) ,
	chUserName			 =ltrim(rtrim(chUserName))	,
	chEndDate 			 =ltrim(rtrim(chEndDate)) ,
	dtLastModified 		 =dtLastModified ,
	chXDataHosp 		 =ltrim(rtrim(chXDataHosp)) ,
	chAction			 =ltrim(rtrim(chAction)),
	chUserID10 			 =ltrim(rtrim(chUserID10))
from TCOPDSQL.DB_GEN.dbo.GenUserProfile1_ICWD
where dtLastModified>=dateadd(day,-1,@dtLastProcess) and not (chAction like '% ed%')
order by dtLastModified

open list

fetch next from list 
into 
		@chUserID ,
		@chXData ,
		@chUserName	,
		@chEndDate ,
		@dtLastModified ,
		@chXDataHosp ,
		@chAction ,
		@chUserID10 

while @@FETCH_STATUS = 0
begin
	/*UserID左右去空白*/
	set @chUserID=ltrim(rtrim(@chUserID))
	declare @dtEndDate datetime
	/*UserID左右去空白*/
	/*處理結束日期*/
	if len(@chEndDate)=7
	begin

		declare @YYYY char(4)
		set @YYYY=convert(char(4),convert(int,substring(@chEndDate,1,3))+1911)

		declare @MM char(2)
		set @MM=substring(@chEndDate,4,2)

		declare @DD char(2)
		set @DD=SUBSTRING(@chEndDate,6,2)

		set @dtEndDate=convert(datetime,@YYYY+'-'+@MM+'-'+@DD)

	end
	/*處理結束日期*/
	/*處理空白密碼*/
	if isnull(@chXData,'')='' or ltrim(rtrim(@chXData))=''
	begin
		print @chUserID+' 密碼空白'
		set @chXData=dbo.funGetMD5(@chUserID)
		print @chXData
	end
	/*處理空白密碼*/
	if exists(select * from GenProxyAccount where chUserID=@chUserID)
	begin
		print @chUserID+ '|exists'
		update GenProxyAccount
		set 
			chUserID        =       @chUserID ,
			chXData 		=		@chXData ,
			chUserName		=		@chUserName	,
			chEndDate 		=		@chEndDate ,
			dtEndDate		=		@dtEndDate,
			dtLastModified	=		getdate() ,
			chXDataHosp 	=		@chXDataHosp ,
			chUserID10 		=		@chUserID10 
		where chUserID=@chUserID

	end
	else
	begin
		print @chUserID+'|not exists'
		insert GenProxyAccount
			(
			chUserID,
			chXData,
			chUserName,
			chEndDate,
			dtEndDate,
			dtLastModified,
			chXDataHosp,
			chUserID10
			)
		values (
			@chUserID ,
			@chXData ,
			@chUserName	,
			@chEndDate ,
			@dtEndDate,
			getdate() ,
			@chXDataHosp ,
			@chUserID10 
			)
	end

	update TCOPDSQL.DB_GEN.dbo.GenUserProfile1_ICWD
	set chAction=chAction+' ed'
	where chUserID=@chUserID and dtLastModified=@dtLastModified and chAction=@chAction

	set @RowCount=@RowCount+1

	fetch next from list 
	into 
			@chUserID ,
			@chXData ,
			@chUserName	,
			@chEndDate ,
			@dtLastModified ,
			@chXDataHosp ,
			@chAction ,
			@chUserID10 
end

close list
deallocate list 

update ProxyAccountProcessRecord
set 
	nvProcessJob=nvProcessJob+'完成，筆數:'+convert(varchar(4),@RowCount),
	dtProcessEnd=getdate(),
	isSuccess=1
where dtProcessStart=@dtStart
	and chProcessHosp='TC'

print 'Done'



END