

CREATE PROCEDURE [dbo].[SyncPasswordAndEnabledToMedProxyByUser]
	@chUserID varchar(10)
AS

	declare @select varchar(max)
	declare @where varchar(max)
	declare @openquery varchar(max)

	--temp table放MySQL的passwd
	create table #temp(
	  [user] varchar(32) NOT NULL DEFAULT '',
	  [password] varchar(35) NOT NULL DEFAULT '',
	  [enabled] tinyint NOT NULL DEFAULT '1',
	  [Dept] varchar(100) DEFAULT NULL,
	  [mail_address] varchar(50) DEFAULT NULL,
	  [fullname] varchar(60) DEFAULT NULL,
	  [comment] varchar(60) DEFAULT NULL,
	  PRIMARY KEY ([user])
	)

	--組合openquery查詢字串
	set @select='select * from hluser.passwd '
	set @where=''

	print @select + @where

	set @openquery='select * from openquery(MEDPROXY,'''+@select+@where+''')'
	print @openquery

	insert #temp
	--執行openquery
	exec (@openquery)

	--下面開始跑回圈比對兩邊資料
	declare @chXData varchar(32)
	declare @chUserName nvarchar(10)
	declare @chDeptName nvarchar(50)
	declare @chEMail nvarchar(50)
	declare @dtEndDate datetime

	declare list cursor for

	select 
		chUserID,
		chXData,
		chUserName,
		chDeptName,
		chEMail,
		dtEndDate
	from GenProxyAccount
	where ltrim(rtrim(chUserID))=@chUserID

	open list

	fetch next from list
	into @chUserID,@chXData,@chUserName,@chDeptName,@chEMail,@dtEndDate

	while @@FETCH_STATUS=0
	begin

		if exists (select * from #temp where [user]=@chUserID)
		begin

			--print @chUserID+'有資料'

			declare @OldPassword varchar(35)
			declare @enabled tinyint
			select @OldPassword=[password],@enabled=[enabled] from #temp where [user]=@chUserID

			if lower(@OldPassword)<>lower(@chXData)
			begin
				print @chUserID+'密碼有變更'
				print '舊密碼'+@OldPassword
				print '新密碼'+@chXData

				--組合openquery查詢字串
				set @select='select * from hluser.passwd '
				set @where='where user="'+@chUserID+'"'

				print @select + @where

				set @openquery='update openquery(MEDPROXY,'''+@select+@where+''' ) set password='''+lower(@chXData)+''' '
				print @openquery

				--執行openquery
				exec (@openquery)

			end

			if @dtEndDate<getdate()
			begin
				if @enabled=0
				begin
					print @chUserID+'的enalbed='+convert(varchar(1),@enabled)+';帳號已停用：'+convert(varchar(30),@dtEndDate)

					--組合openquery查詢字串
					set @select='select * from hluser.passwd '
					set @where='where user="'+@chUserID+'"'

					print @select + @where

					set @openquery='update openquery(MEDPROXY,'''+@select+@where+''' ) set enabled=''1'' '
					print @openquery

					--執行openquery
					exec (@openquery)
				
				end
			end
			else
			begin
				if @enabled=1
				begin
					print @chUserID+'的enalbed='+convert(varchar(1),@enabled)+';帳號未停用：'+convert(varchar(30),@dtEndDate)

					--組合openquery查詢字串
					set @select='select * from hluser.passwd '
					set @where='where user="'+@chUserID+'"'

					print @select + @where

					set @openquery='update openquery(MEDPROXY,'''+@select+@where+''' ) set enabled=''0'' '
					print @openquery

					--執行openquery
					exec (@openquery)

				end
			end
		end
		else
		begin

			print @chUserID+'沒有資料'

			insert openquery(MEDPROXY,'select user,password,enabled,Dept,mail_address,fullname,comment from passwd where 1=0 ')
			values
			(@chUserID,lower(@chXData),'0',@chDeptName,@chEMail,@chUserName,'')

		end
	
		fetch next from list
		into @chUserID,@chXData,@chUserName,@chDeptName,@chEMail,@dtEndDate
	
	end

	close list
	deallocate list

	drop table #temp





RETURN 0