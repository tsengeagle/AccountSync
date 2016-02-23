CREATE TABLE [dbo].[ProxyAccountProcessRecord] (
    [intSerialNo]    INT           IDENTITY (1, 1) NOT NULL,
    [dtProcessStart] DATETIME      NOT NULL,
    [chProcessHosp]  CHAR (2)      NOT NULL,
    [nvProcessJob]   NVARCHAR (50) NULL,
    [dtProcessEnd]   DATETIME      NULL,
    [isSuccess]      BIT           NULL,
    CONSTRAINT [PK_ProxyAccountProcessRecord] PRIMARY KEY CLUSTERED ([dtProcessStart] ASC, [chProcessHosp] ASC)
);


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ProxyAccountProcessRecord] TO [guid]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ProxyAccountProcessRecord] TO [guid]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ProxyAccountProcessRecord] TO [guid]
    AS [dbo];

