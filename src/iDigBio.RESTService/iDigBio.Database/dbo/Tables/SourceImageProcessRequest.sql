CREATE TABLE [dbo].[SourceImageProcessRequest] (
    [sourceImageProcessRequestId] UNIQUEIDENTIFIER CONSTRAINT [DF_SourceImageProcessRequest_sourceImageProcessRequestId] DEFAULT (newid()) NOT NULL,
    [sourceImageId]               UNIQUEIDENTIFIER NOT NULL,
    [processEngineId]             INT              NOT NULL,
    [callbackUri]                 NVARCHAR (500)   NULL,
    [ipAddress]                   NVARCHAR (50)    NULL,
    [resultValue]                 NVARCHAR (4000)  NULL,
    [resultCallbackUri]           NVARCHAR (4000)  NULL,
    [resultCreatedUTCDateTime]    DATETIME         NULL,
    [createdByUserName]           NVARCHAR (256)   NOT NULL,
    [createdUTCDateTime]          DATETIME         CONSTRAINT [DF_SourceImageProcessRequest_createdUTCDateTime] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SourceImageProcessRequest] PRIMARY KEY CLUSTERED ([sourceImageProcessRequestId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_SourceImageProcessRequest_ProcessEngine] FOREIGN KEY ([processEngineId]) REFERENCES [dbo].[ProcessEngine] ([processEngineId]),
    CONSTRAINT [FK_SourceImageProcessRequest_SourceImage] FOREIGN KEY ([sourceImageId]) REFERENCES [dbo].[SourceImage] ([sourceImageId])
);

