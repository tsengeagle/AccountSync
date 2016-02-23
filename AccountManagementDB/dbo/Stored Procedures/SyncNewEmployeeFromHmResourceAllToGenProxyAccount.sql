-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SyncNewEmployeeFromHmResourceAllToGenProxyAccount
AS
BEGIN


declare @RowCount int
set @RowCount=0

declare @fd_ID char(10)
declare @fd_ChnName varchar(50)
declare @fd_WrkDeptFullName varchar(100)
declare @fd_EleSign varchar(100)

/*抓最後一次處理時間*/
declare @dtLastProcess datetime

select top 1
	@dtLastProcess=dtProcessStart 
from ProxyAccountProcessRecord
where isSuccess=1 and chProcessHosp='IC' and nvProcessJob like '同步新進人員%'
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
values(@dtStart,'IC','同步新進人員')
/*寫處理記錄開始時間*/

declare list cursor for

select 
	distinct fd_ID,fd_ChnName
from ictestsvr.HIS3_DB_MGT.dbo.TaxHmResourceAll
where convert(datetime,replace(replace(LastModified,'上午',''),'下午',''))>=@dtLastProcess and fd_WrkStatus='1' 
	and ltrim(rtrim(isnull(fd_ID,'')))<>''
order by fd_ID

open list

fetch next from list
into @fd_ID,@fd_ChnName

while @@FETCH_STATUS = 0
begin

	select top 1 
		fd_ID,
		fd_ChnName,
		fd_WrkDeptFullName,
		fd_EleSign
	into #tempAccount
	from ictestsvr.HIS3_DB_MGT.dbo.TaxHmResourceAll
	where fd_ID=@fd_ID 
		and convert(datetime,replace(replace(LastModified,'上午',''),'下午',''))>=@dtLastProcess
	order by LastModified desc

	select 
		@fd_ChnName=fd_ChnName,
		@fd_WrkDeptFullName=fd_WrkDeptFullName,
		@fd_EleSign=fd_EleSign
	from #tempAccount

	if exists (select * from GenProxyAccount where chUserID10=@fd_ID)
	begin

		print @fd_ID+'Exists!'
		--update GenProxyAccount
		--set 
		--	chUserName=@fd_ChnName ,
		--	chDeptName=@fd_WrkDeptFullName ,
		--	chEMail=@fd_EleSign ,
		--	dtLastModified=getdate()
		--where chUserID=@fd_ID

	end
	else
	begin

		print @fd_ID+'Not Exists!'
		--新人要寫入，但chUserID要先確認是否存在
		declare @chUserID varchar(10)
		set @chUserID=substring(ltrim(rtrim(@fd_ID)),len(ltrim(rtrim(@fd_ID)))-5,6)

		if exists(select * from GenProxyAccount where chUserID=@chUserID)
		begin
			print @fd_ID+'duplicate'
			--set @chUserID=substring(ltrim(rtrim(@fd_ID)),len(ltrim(rtrim(@fd_ID)))-6,6)

			/*6碼ID在GenProxyAccount裡面重複，寫錯誤記錄*/
			insert ProxyAccountCorrectionError
			(chUserID,chUserName,nvReason,dtCorrection)
			values(@fd_ID,@fd_ChnName,'GenProxyAccount有重複ID!',getdate())

		end
		else
		begin
			print @fd_ID+'not duplicate'

			insert GenProxyAccount
			(chUserID,chXData,chUserName,chDeptName,chEMail,chEndDate,dtEndDate,dtLastModified,chXDataHosp,chUserID10)
			values(
				@chUserID,
				'',
				@fd_ChnName,
				@fd_WrkDeptFullName,
				@fd_EleSign,
				'',
				getdate(),
				getdate(),
				'IC',
				@fd_ID
				)

		end

	end

	drop table #tempAccount

	set @RowCount=@RowCount+1

	fetch next from list
	into @fd_ID,@fd_ChnName

end

update ProxyAccountProcessRecord
set nvProcessJob=nvProcessJob+'完成，筆數:'+convert(varchar(4),@RowCount),
	dtProcessEnd=getdate(),
	isSuccess=1
where dtProcessStart=@dtStart and chProcessHosp='IC'


close list
deallocate list



END