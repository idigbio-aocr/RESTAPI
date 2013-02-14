CREATE TABLE [dbo].[OCRCachedResult] (
    [ocrCachedResultId] INT              IDENTITY (1, 1) NOT NULL,
    [sourceImageId]     UNIQUEIDENTIFIER NOT NULL,
    [processEngineId]   INT              NOT NULL,
    [value]             NVARCHAR (4000)  NULL,
    CONSTRAINT [PK_OCRCachedResult] PRIMARY KEY CLUSTERED ([ocrCachedResultId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_OCRCachedResult_ProcessEngine] FOREIGN KEY ([processEngineId]) REFERENCES [dbo].[ProcessEngine] ([processEngineId]),
    CONSTRAINT [FK_OCRCachedResult_SourceImage] FOREIGN KEY ([sourceImageId]) REFERENCES [dbo].[SourceImage] ([sourceImageId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_OCRCachedResult]
    ON [dbo].[OCRCachedResult]([sourceImageId] ASC, [processEngineId] ASC) WITH (FILLFACTOR = 80);

