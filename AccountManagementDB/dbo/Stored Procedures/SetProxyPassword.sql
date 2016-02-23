CREATE PROCEDURE [dbo].SetProxyPassword
	@chUserID char(10),
	@chXData varchar(32),
	@chEndDate char(7)
AS
	
	if exists (select * from GenProxyAccount where chUserID=@chUserID)
	begin

		declare @dtEndDate datetime

		declare @YYYY char(4)
		set @YYYY=convert(char(4),convert(int,substring(@chEndDate,1,3))+1911)

		declare @MM char(2)
		set @MM=substring(@chEndDate,4,2)

		declare @DD char(2)
		set @DD=SUBSTRING(@chEndDate,6,2)

		set @dtEndDate=convert(datetime,@YYYY+'-'+@MM+'-'+@DD)

		update GenProxyAccount
		set chXData=@chXData,
			chEndDate=@chEndDate,
			dtEndDate=isnull(@dtEndDate,getdate()),
			dtLastModified=getdate()
		where chUserID=@chUserID

	end

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetProxyPassword] TO [guid]
    AS [dbo];

