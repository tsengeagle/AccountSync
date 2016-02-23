CREATE TABLE [dbo].[GenUserProfile1FromAllHosp] (
    [chUserID]       VARCHAR (10)  NOT NULL,
    [chXData]        VARCHAR (32)  CONSTRAINT [DF_GenUserProfile1FromAllHosp_chXData] DEFAULT ('') NULL,
    [chUserName]     NVARCHAR (10) NULL,
    [chEndDate]      CHAR (7)      NULL,
    [dtLastModified] DATETIME      CONSTRAINT [DF_GenUserProfile1FromAllHosp_dtLastModified] DEFAULT (getdate()) NOT NULL,
    [chXDataHosp]    CHAR (5)      NULL,
    [chHostName]     VARCHAR (50)  NULL,
    [chAction]       VARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_GenProxyAccountTransTbl] PRIMARY KEY CLUSTERED ([chUserID] ASC, [dtLastModified] ASC, [chAction] ASC)
);


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER syncToProxyAccountByInsert
   ON  dbo.GenUserProfile1FromAllHosp
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	declare @chUserID varchar(10)
	declare @chUserName nvarchar(10)
	declare @chXData varchar(32)
	declare @chEndDate char(7)
	declare @chXDataHosp char(5)
	
	select 
		@chUserID=chUserID,
		@chUserName=chUserName,
		@chXData=chXData,
		@chEndDate=chEndDate,
		@chXDataHosp=chXDataHosp
	from inserted
		
	declare @dtEndDate datetime
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

	if exists (select * from GenProxyAccount where chUserID=@chUserID)
	begin

		--print 'Exists'
		update GenProxyAccount
		set chXData=@chXData,
			chUserName=@chUserName,
			chEndDate=@chEndDate,
			dtEndDate=@dtEndDate,
			dtLastModified=getdate(),
			chXDataHosp=@chXDataHosp
		where chUserID=@chUserID

	end
	else
	begin 

		INSERT INTO [dbo].[GenProxyAccount]
				   ([chUserID]
				   ,[chXData]
				   ,[chUserName]
				   ,[chDeptName]
				   ,[chEMail]
				   ,[chEndDate]
				   ,[dtEndDate]
				   ,[dtLastModified]
				   ,[chXDataHosp]
				   ,[chUserType])
			 VALUES
				   (@chUserID
				   ,@chXData
				   ,@chUserName
				   ,''
				   ,''
				   ,@chEndDate
				   ,@dtEndDate
				   ,getdate()
				   ,@chXDataHosp
				   ,'')

	end

END
GO
DISABLE TRIGGER [dbo].[syncToProxyAccountByInsert]
    ON [dbo].[GenUserProfile1FromAllHosp];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GenUserProfile1FromAllHosp] TO [guid]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[GenUserProfile1FromAllHosp] TO [guid]
    AS [dbo];

