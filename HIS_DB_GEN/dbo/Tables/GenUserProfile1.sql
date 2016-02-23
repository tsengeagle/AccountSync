CREATE TABLE [dbo].[GenUserProfile1] (
    [chUserID]         CHAR (10)    NOT NULL,
    [chUserPass]       CHAR (6)     NULL,
    [chUserName]       CHAR (10)    NULL,
    [chUserOPD]        CHAR (1)     NULL,
    [chUserAMD]        CHAR (1)     NULL,
    [chUserMrbasic]    CHAR (1)     NULL,
    [chUserMeg]        CHAR (1)     NULL,
    [chUserGro]        CHAR (3)     NULL,
    [chStartDate]      CHAR (7)     NULL,
    [chEndDate]        CHAR (7)     NULL,
    [chLogInDT]        CHAR (13)    NULL,
    [chLogOutDT]       CHAR (13)    NULL,
    [chLogInStat]      CHAR (20)    NULL,
    [chUserSector]     CHAR (10)    NULL,
    [chUserAcc]        CHAR (1)     NULL,
    [chUserIPH]        CHAR (1)     NULL,
    [chUserRad]        CHAR (1)     NULL,
    [chOffStartDate]   CHAR (11)    NULL,
    [chOffEndDate]     CHAR (11)    NULL,
    [chXData]          VARCHAR (32) NULL,
    [chRandomKey]      VARCHAR (10) NULL,
    [chUserOtherKey]   VARCHAR (20) NULL,
    [chLastModifyID]   VARCHAR (10) NULL,
    [sdLastModifyDate] DATETIME     CONSTRAINT [DF_GenUserProfile1_sdLastModifyDate_1] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_GenUserProfile1_1__18] PRIMARY KEY CLUSTERED ([chUserID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [Index_chuserName]
    ON [dbo].[GenUserProfile1]([chUserName] ASC);


GO


CREATE TRIGGER [dbo].[GenUserProfile1_ForPACS_UPDATE] ON [dbo].[GenUserProfile1] 
FOR UPDATE 
AS

DECLARE @COLUMN varbinary(50)

SELECT @COLUMN = COLUMNS_UPDATED()

IF SUBSTRING(@COLUMN,1,1) & 4 > 0 -- chUserName = select power(2,(3-1))			
	OR SUBSTRING(@COLUMN,2,1) & 2 > 0 -- chEndDate = select power(2,(2-1))			
	OR SUBSTRING(@COLUMN,3,1) & 8 > 0 -- chXData = select power(2,(4-1))
BEGIN

	If Exists (Select Top 1 Ins.chUserID  From dbo.GenUserProfile2 P2 ,inserted Ins
				Where P2.chUserID=Ins.chUserID 
				And ((P2.chFormID ='WEBVIEW' And Left(chFormFun1,1)='Y') --有WEBVIEW查詢權限
							Or (P2.chFormID = 'PHIS' And Left(chFormFun1,1)='Y')))--有電子病歷總查詢的查詢權限
	Begin

		INSERT INTO dbo.GenUserProfileSyncToPacs (chDataFrom,chWhatKind,chUserID,chUserName,chEndDate,chXData)
		SELECT  DISTINCT  'PROFILE1','UPD',Ins.chUserID,chUserName,chEndDate,chXData
		FROM   dbo.GenUserProfile2 P2 ,inserted Ins
		WHERE P2.chUserID=Ins.chUserID 
		AND ((P2.chFormID ='WEBVIEW' And Left(chFormFun1,1)='Y')
					 OR (P2.chFormID = 'PHIS' AND LEFT(chFormFun1,1)='Y'))

	End
END
GO


CREATE TRIGGER [dbo].[GenUserProfile1_ForPACS_INSERT] ON [dbo].[GenUserProfile1] 
FOR INSERT 
AS

If Exists (Select Top 1 Ins.chUserID  From dbo.GenUserProfile2 P2 ,inserted Ins
				Where P2.chUserID=Ins.chUserID 
				And ((P2.chFormID ='WEBVIEW' And Left(chFormFun1,1)='Y') --有WEBVIEW查詢權限
							Or (P2.chFormID = 'PHIS' And Left(chFormFun1,1)='Y')))--有電子病歷總查詢的查詢權限
Begin

	INSERT INTO dbo.GenUserProfileSyncToPacs (chDataFrom,chWhatKind,chUserID,chUserName,chEndDate,chXData)
	SELECT   DISTINCT 'PROFILE1','INS',Ins.chUserID,chUserName,chEndDate,chXData
	FROM   dbo.GenUserProfile2 P2 ,inserted Ins
	WHERE P2.chUserID=Ins.chUserID 
	AND ((P2.chFormID ='WEBVIEW' And Left(chFormFun1,1)='Y')
			 OR (P2.chFormID = 'PHIS' AND LEFT(chFormFun1,1)='Y'))

End
GO

create TRIGGER [dbo].[GenUserProfile1_ForPACS_DELETE] ON [dbo].[GenUserProfile1] 
FOR DELETE 
AS
If Exists (Select Top 1 Del.chUserID  From dbo.GenUserProfile2 P2 ,deleted Del
			Where P2.chUserID=Del.chUserID 
			And ((P2.chFormID ='WEBVIEW' And Left(chFormFun1,1)='Y') --有WEBVIEW查詢權限
							Or (P2.chFormID = 'PHIS' And Left(chFormFun1,1)='Y')))--有電子病歷總查詢的查詢權限
Begin

	INSERT INTO dbo.GenUserProfileSyncToPacs (chDataFrom,chWhatKind,chUserID,chUserName,chEndDate,chXData)
	SELECT   DISTINCT   'PROFILE1','DEL',Del.chUserID,chUserName,chEndDate,chXData
	FROM   dbo.GenUserProfile2 P2 ,deleted Del
	WHERE P2.chUserID=Del.chUserID 
	AND ((P2.chFormID ='WEBVIEW' And Left(chFormFun1,1)='Y')
					 OR (P2.chFormID = 'PHIS' AND LEFT(chFormFun1,1)='Y'))

End
GO

CREATE TRIGGER [dbo].[GenUserProfile1_trg_Delete] ON [dbo].[GenUserProfile1] 
FOR DELETE 
AS

declare @chUserID varchar(10)
SELECT @chUserID = ISNULL(chUserID,'') FROM deleted
/* ----------------------------------------------------- */

insert GenUserProfile1_TransTbl
         select *, getdate(), 'DEL' from deleted

	--行政人員
	if LEN(@chUserID)=10
	begin
		insert into DB_GEN.dbo.GenUserProfile1_ICWD
		(chUserID, chXData, chUserName, chEndDate, dtLastModified, chXDataHosp, chHostName, chAction, chUserID10)
		select right(chUserID,6), chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'del', chUserID 
		from deleted  where rtrim(isnull(chRandomKey,''))<>'180day'
	end
	--醫師代碼
	if LEN(@chUserID)<=5 and  isnumeric(@chUserID)=1
	begin
		insert into DB_GEN.dbo.GenUserProfile1_ICWD
		(chUserID, chXData, chUserName, chEndDate, dtLastModified, chXDataHosp, chHostName, chAction, chUserID10)
		select right(b.chIDNo,6), chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'del', chIDNo 
		from deleted a inner join DB_GEN.dbo.GenDoctorTbl b (nolock) on a.chUserID=b.chDocNo
		where rtrim(isnull(a.chRandomKey,''))<>'180day'
	end	
	--見實習醫師
	if LEN(@chUserID)<=5 and  isnumeric(@chUserID)<>1 and  isnumeric(rtrim(substring(@chUserID,2,4)))=1
	begin
		insert into DB_GEN.dbo.GenUserProfile1_ICWD
		(chUserID, chXData, chUserName, chEndDate, dtLastModified, chXDataHosp, chHostName, chAction, chUserID10)
		select right(chIDNo,6), chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'del', chIDNo 
		from deleted a inner join DB_GEN.dbo.GenDoctorTbl b (nolock) on a.chUserID=b.chDocNo
		where rtrim(isnull(a.chRandomKey,''))<>'180day'
	end			      
   
--delete  [10.2.6.216].db222.dbo.GenUserProfile1 
--from [10.2.6.216].db222.dbo.GenUserProfile1 a, deleted b
--where b.chUserID  = a.chUserID collate Chinese_Taiwan_Stroke_BIN        

--delete  HLBKASVR.DB_GEN.dbo.GenUserProfile1 
--from HLBKASVR.DB_GEN.dbo.GenUserProfile1 a, deleted b
--where a.chUserID =b.chUserID   
GO

CREATE TRIGGER [dbo].[GenUserProfile1_trg_Insert] ON [dbo].[GenUserProfile1] 
FOR INSERT
AS 

declare @chUserID varchar(10)
SELECT @chUserID = ISNULL(chUserID,'') FROM inserted
/* ----------------------------------------------------- */

insert GenUserProfile1_TransTbl
         select *, getdate(), 'INS' from inserted

INSERT INTO GenUserProfile1AssetQuest (chUserID, chUserName, chXData, chRandomKey)
	select chUserID, chUserName, chXData, chRandomKey from inserted

	--行政人員
	if LEN(@chUserID)=10
	begin
		insert into DB_GEN.dbo.GenUserProfile1_ICWD 
		(chUserID, chXData, chUserName, chEndDate, dtLastModified, chXDataHosp, chHostName, chAction, chUserID10)
		select right(chUserID,6), chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'ins', chUserID 
		from inserted  where rtrim(isnull(chRandomKey,''))<>'180day'
	end
	--醫師代碼
	if LEN(@chUserID)<=5 and  isnumeric(@chUserID)=1
	begin
		insert into DB_GEN.dbo.GenUserProfile1_ICWD
		(chUserID, chXData, chUserName, chEndDate, dtLastModified, chXDataHosp, chHostName, chAction, chUserID10)
		select right(b.chIDNo,6), chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'ins', chIDNo 
		from inserted a inner join DB_GEN.dbo.GenDoctorTbl b (nolock) on a.chUserID=b.chDocNo
		where rtrim(isnull(a.chRandomKey,''))<>'180day'
	end	
	--見實習醫師
	if LEN(@chUserID)<=5 and  isnumeric(@chUserID)<>1 and  isnumeric(rtrim(substring(@chUserID,2,4)))=1
	begin
		insert into DB_GEN.dbo.GenUserProfile1_ICWD
		(chUserID, chXData, chUserName, chEndDate, dtLastModified, chXDataHosp, chHostName, chAction, chUserID10)
		select right(chIDNo,6), chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'ins', chIDNo 
		from inserted a inner join DB_GEN.dbo.GenDoctorTbl b (nolock) on a.chUserID=b.chDocNo
		where rtrim(isnull(a.chRandomKey,''))<>'180day'
	end			
  --insert into ICDWSVR1.DB_GEN.dbo.GenUserProfile1FromAllHosp
  --select chUserID, chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'ins' 
  --from inserted  where chXData is not null	
--Insert into [10.2.6.216].db222.dbo.GenUserProfile1
 --  select * from inserted	
GO
CREATE TRIGGER [dbo].[trg_GenUserProfile1Update] ON [dbo].[GenUserProfile1] 
FOR UPDATE 
AS

begin

declare @XData1 varchar(32)
declare @XData2 varchar(32)
declare @chUserID varchar(10)
declare @EndDate1 char(7)
declare @EndDate2 char(7)

SELECT @XData1 = ISNULL(chXData,''), @EndDate1 = ISNULL(chEndDate,'') FROM deleted
SELECT @XData2 = ISNULL(chXData,''), @EndDate2 = ISNULL(chEndDate,'') FROM inserted
SELECT @chUserID = ISNULL(chUserID,'') FROM inserted

if (@XData1 <> @XData2 or @EndDate1 <> @EndDate2)
begin
	INSERT INTO GenUserProfile1AssetQuest (chUserID, chUserName, chXData, chRandomKey)
	select chUserID, chUserName, chXData, chRandomKey from inserted
	--行政人員
	if LEN(@chUserID)=10
	begin
		insert into DB_GEN.dbo.GenUserProfile1_ICWD
		(chUserID, chXData, chUserName, chEndDate, dtLastModified, chXDataHosp, chHostName, chAction, chUserID10)
		select right(chUserID,6), chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'upd', chUserID 
		from inserted  where rtrim(isnull(chRandomKey,''))<>'180day'
	end
	--醫師代碼
	if LEN(@chUserID)<=5 and  isnumeric(@chUserID)=1
	begin
		insert into DB_GEN.dbo.GenUserProfile1_ICWD
		(chUserID, chXData, chUserName, chEndDate, dtLastModified, chXDataHosp, chHostName, chAction, chUserID10)
		select right(b.chIDNo,6), chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'upd', chIDNo 
		from inserted a inner join DB_GEN.dbo.GenDoctorTbl b (nolock) on a.chUserID=b.chDocNo
		where rtrim(isnull(a.chRandomKey,''))<>'180day'
	end	
	--見實習醫師
	if LEN(@chUserID)<=5 and  isnumeric(@chUserID)<>1 and  isnumeric(rtrim(substring(@chUserID,2,4)))=1
	begin
		insert into DB_GEN.dbo.GenUserProfile1_ICWD
		(chUserID, chXData, chUserName, chEndDate, dtLastModified, chXDataHosp, chHostName, chAction, chUserID10)
		select right(chIDNo,6), chXData, chUserName, chEndDate, GETDATE() , 'HL', 'HL', 'upd', chIDNo 
		from inserted a inner join DB_GEN.dbo.GenDoctorTbl b (nolock) on a.chUserID=b.chDocNo
		where rtrim(isnull(a.chRandomKey,''))<>'180day'
	end		
end
--insert GenUserProfile1_TransTbl
         --select *, getdate(), 'UPD' from inserted
         
--delete  [10.2.6.216].db222.dbo.GenUserProfile1  
--from [10.2.6.216].db222.dbo.GenUserProfile1 a, deleted b
--where b.chUserID  = a.chUserID collate Chinese_Taiwan_Stroke_BIN 

--insert [10.2.6.216].db222.dbo.GenUserProfile1 
--select * from inserted
end