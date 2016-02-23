CREATE TABLE [dbo].[tempProxyAccount] (
    [chProxyID]   CHAR (10)     NOT NULL,
    [nvName]      NVARCHAR (10) NULL,
    [nvDeptName]  NVARCHAR (30) NULL,
    [password]    VARCHAR (10)  NULL,
    [passwordMD5] CHAR (32)     NULL,
    CONSTRAINT [PK_tempProxyAccount] PRIMARY KEY CLUSTERED ([chProxyID] ASC)
);

