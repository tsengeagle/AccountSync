CREATE PROCEDURE [dbo].GetMD5
		@input varchar(max)
AS
	select upper(right(master.sys.fn_varbintohexstr(HASHBYTES('MD5', @input )),32))

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetMD5] TO [abcmuser]
    AS [dbo];

