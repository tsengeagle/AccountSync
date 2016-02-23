-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION funGetMD5
(
	-- Add the parameters for the function here
	@input varchar(max)
)
RETURNS char(32)
AS
BEGIN
	-- Add the T-SQL statements to compute the return value here
	RETURN upper(right(master.sys.fn_varbintohexstr(HASHBYTES('MD5', @input )),32))

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[funGetMD5] TO [guid]
    AS [dbo];

