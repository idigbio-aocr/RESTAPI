CREATE TABLE [dbo].[ProcessEngine] (
    [processEngineId] INT             IDENTITY (1, 1) NOT NULL,
    [name]            NVARCHAR (100)  NOT NULL,
    [description]     NVARCHAR (1000) NULL,
    CONSTRAINT [PK_ProcessEngine] PRIMARY KEY CLUSTERED ([processEngineId] ASC) WITH (FILLFACTOR = 80)
);

