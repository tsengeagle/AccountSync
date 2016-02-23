-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ProxyAccountCorrectionWithHmResource]
AS
BEGIN

--------------------------------------------------------
declare @RowCount int
set @RowCount=0

declare @chUserID varchar(10)
declare @chUserName nvarchar(10)

declare @dtStart datetime
set @dtStart=getdate()

insert ProxyAccountProcessRecord
(dtProcessStart,chProcessHosp,nvProcessJob)
values(@dtStart,'IC','與人資校正')

declare list cursor for

select chUserID,chUserName
from GenProxyAccount
where len(ltrim(rtrim(chUserID)))<10

open list

declare @chUserID10 varchar(10)
declare @chDeptName nvarchar(50)
declare @chEMail nvarchar(50)

fetch next from list 
into @chUserID,@chUserName

while @@FETCH_STATUS=0
begin
	set @chUserID=ltrim(rtrim(@chUserID))
	set @chUserName=ltrim(rtrim(@chUserName))

	if exists(
		select * 
		from ictestsvr.HIS3_DB_MGT.dbo.TaxHmResourceAll 
		where fd_WrkStatus='1' --and fd_OrgDeptFullName like '%法人%'
			and substring(fd_ID,len(ltrim(rtrim(fd_ID)))-5,6)=@chUserID
			and ltrim(rtrim(fd_ChnName))=@chUserName
			)
	begin
		print @chUserID+' | '+@chUserName+' exists! '
		
		select top 1
			@chUserID10=fd_ID,
			@chDeptName=fd_OrgDeptFullName,
			@chEMail=fd_EleSign
		from ictestsvr.HIS3_DB_MGT.dbo.TaxHmResourceAll 
		where fd_WrkStatus='1' --and fd_OrgDeptFullName like '%法人%'
			and substring(fd_ID,len(ltrim(rtrim(fd_ID)))-5,6)=@chUserID
			and ltrim(rtrim(fd_ChnName))=@chUserName
		order by LastModified desc

		print @chUserID10+' | '+@chDeptName+' | '+@chEMail

		if exists(
			select 
			* 
			from GenProxyAccount 
			where (chUserID=@chUserID and chUserName=@chUserName) and
				(chUserID10<>@chUserID10 or chDeptName<>@chDeptName or chEMail<>@chEMail) 
			)
		begin
			print @chUserID10+' | '+@chDeptName+' | '+@chEMail + ' 與現有資料不一致'
			 
			update GenProxyAccount
			set chUserID10=@chUserID10,
				chDeptName=@chDeptName,
				chEMail=@chEMail,
				dtLastModified=getdate()
			where chUserID=@chUserID and chUserName=@chUserName

			set @RowCount=@RowCount+1

		end
	end
	else
	begin
		/*6碼ID＋姓名，在人資裡面找不到，寫錯誤記錄*/
		print @chUserID+' | '+@chUserName+' not exists! '
		insert ProxyAccountCorrectionError
		(chUserID,chUserName,nvReason,dtCorrection)
		values(@chUserID,@chUserName,'TaxHmResourceAll not exists!',getdate())
	end


	fetch next from list 
	into @chUserID,@chUserName

end

update ProxyAccountProcessRecord
set nvProcessJob=nvProcessJob+'完成，筆數:'+convert(varchar(4),@RowCount),
	dtProcessEnd=getdate(),
	isSuccess=1
where dtProcessStart=@dtStart and chProcessHosp='IC'

close list
deallocate list




END