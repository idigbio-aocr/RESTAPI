CREATE TABLE [dbo].[SourceImageRepositoryProcessEngineConfig] (
    [sourceImageRepositoryId] UNIQUEIDENTIFIER NOT NULL,
    [processEngineId]         INT              NOT NULL,
    [environment]             NVARCHAR (50)    NOT NULL,
    [outputDirectoryPath]     NVARCHAR (1000)  NULL,
    [useOCRSimulatedResult]   BIT              CONSTRAINT [DF_SourceImageRepositoryProcessEngineConfig_useOCRSimulatedResult] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SourceImageRepositoryProcessEngineConfig] PRIMARY KEY CLUSTERED ([sourceImageRepositoryId] ASC, [processEngineId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_SourceImageRepositoryProcessEngineConfig_ProcessEngine] FOREIGN KEY ([processEngineId]) REFERENCES [dbo].[ProcessEngine] ([processEngineId]),
    CONSTRAINT [FK_SourceImageRepositoryProcessEngineConfig_SourceImageRepository] FOREIGN KEY ([sourceImageRepositoryId]) REFERENCES [dbo].[SourceImageRepository] ([sourceImageRepositoryId])
);

