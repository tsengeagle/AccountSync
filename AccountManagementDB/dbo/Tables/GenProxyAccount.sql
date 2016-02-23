CREATE TABLE [dbo].[GenProxyAccount] (
    [chUserID]       VARCHAR (10)  NOT NULL,
    [chXData]        VARCHAR (32)  NOT NULL,
    [chUserName]     NVARCHAR (10) NULL,
    [chDeptName]     NVARCHAR (50) NULL,
    [chEMail]        NVARCHAR (50) NULL,
    [chEndDate]      CHAR (7)      NULL,
    [dtEndDate]      DATETIME      NULL,
    [dtLastModified] DATETIME      NULL,
    [chXDataHosp]    CHAR (5)      NULL,
    [chUserType]     VARCHAR (50)  NULL,
    [chUserID10]     VARCHAR (10)  NULL,
    CONSTRAINT [PK_GenProxyAccount] PRIMARY KEY CLUSTERED ([chUserID] ASC)
);


GO
GRANT VIEW DEFINITION
    ON OBJECT::[dbo].[GenProxyAccount] TO [guid]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[GenProxyAccount] TO [guid]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GenProxyAccount] TO [guid]
    AS [dbo];


GO
GRANT REFERENCES
    ON OBJECT::[dbo].[GenProxyAccount] TO [guid]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[GenProxyAccount] TO [guid]
    AS [dbo];

