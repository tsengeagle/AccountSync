CREATE TABLE [dbo].[ProxyAccountCorrectionError] (
    [intSerialNo]  INT           IDENTITY (1, 1) NOT NULL,
    [chUserID]     VARCHAR (10)  NULL,
    [chUserName]   NVARCHAR (10) NULL,
    [nvReason]     NVARCHAR (50) NULL,
    [dtCorrection] DATETIME      NULL,
    CONSTRAINT [PK_ProxyAccountCorrectionError] PRIMARY KEY CLUSTERED ([intSerialNo] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ProxyAccountCorrectionError] TO [guid]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ProxyAccountCorrectionError] TO [guid]
    AS [dbo];

