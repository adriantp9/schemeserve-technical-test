CREATE TABLE [dbo].[ProviderLocation] (
	[ProviderId] NVARCHAR(50) NOT NULL,
	[LocationId] NVARCHAR(50) NOT NULL,
	CONSTRAINT [PK_ProviderLocation] PRIMARY KEY ( [ProviderId], [LocationId] )
)

GO
