CREATE VIEW [dbo].vw_GetUserFromAllHosp
	AS 

	select chUserID,chUserName,chXData,chEndDate,sdLastModifyDate
	from HLOPDSQL.DB_GEN.dbo.GenUserProfile1
	where chUserID='A124346139'
	union all
	select chUserID,chUserName,chXData,chEndDate,sdLastModifyDate
	from DLOPDSQL.DB_GEN.dbo.GenUserProfile1
	where chUserID='A124346139'
	union all
	select chUserID collate Chinese_Taiwan_Stroke_BIN ,chUserName collate Chinese_Taiwan_Stroke_BIN ,chXData collate Chinese_Taiwan_Stroke_BIN ,chEndDate collate Chinese_Taiwan_Stroke_BIN ,sdLastModifyDate
	from OPDSQL1.DB_GEN.dbo.GenUserProfile1
	where chUserID='A124346139'
	union all
	select chUserID,chUserName,chXData,chEndDate,sdLastModifyDate
	from TCOPDSQL.DB_GEN.dbo.GenUserProfile1
	where chUserID='A124346139'
	union all
	select chUserID,chUserName,chXData,chEndDate,sdLastModifyDate
	from ULSVR.DB_GEN.dbo.GenUserProfile1
	where chUserID='A124346139'
	union all
	select chUserID,chUserName,chXData,chEndDate,sdLastModifyDate
	from GSSVR.DB_GEN.dbo.GenUserProfile1
	where chUserID='A124346139'
	union all
	select chUserID,chUserName,chXData,chEndDate,sdLastModifyDate
	from TLSVR.DB_GEN.dbo.GenUserProfile1
	where chUserID='A124346139'