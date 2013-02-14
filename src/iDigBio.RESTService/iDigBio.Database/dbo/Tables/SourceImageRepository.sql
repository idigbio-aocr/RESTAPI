CREATE TABLE [dbo].[SourceImageRepository] (
    [sourceImageRepositoryId] UNIQUEIDENTIFIER CONSTRAINT [DF_SourceImageRepository_sourceImageRepositoryId] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [name]                    NVARCHAR (100)   NULL,
    [description]             NVARCHAR (1000)  NULL,
    CONSTRAINT [PK_SourceImageRepository] PRIMARY KEY CLUSTERED ([sourceImageRepositoryId] ASC) WITH (FILLFACTOR = 80)
);

