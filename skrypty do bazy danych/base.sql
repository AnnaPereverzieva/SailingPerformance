USE [SailingManagerDB]
GO
/****** Object:  Table [dbo].[Boats]    Script Date: 2016-01-11 22:38:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Boats](
	[IdBoat] [uniqueidentifier] NOT NULL CONSTRAINT [dc_boats]  DEFAULT (newsequentialid()),
	[Name] [nvarchar](50) NULL,
	[Model] [nvarchar](50) NULL,
 CONSTRAINT [PK_Boats] PRIMARY KEY CLUSTERED 
(
	[IdBoat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GPSData]    Script Date: 2016-01-11 22:38:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GPSData](
	[IdGPSData] [uniqueidentifier] NOT NULL CONSTRAINT [dc_gpsdata]  DEFAULT (newsequentialid()),
	[IdSession] [uniqueidentifier] NULL,
	[SecondsFromStart] [datetime] NULL,
	[BoatSpeed] [float] NULL,
	[BoatDirection] [float] NULL,
	[WindSpeed] [float] NULL,
	[WindDirection] [float] NULL,
	[GeoHeight] [nvarchar](10) NULL,
	[GeoWidth] [nvarchar](10) NULL,
 CONSTRAINT [PK_GPSData] PRIMARY KEY CLUSTERED 
(
	[IdGPSData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sessions]    Script Date: 2016-01-11 22:38:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sessions](
	[IdSession] [uniqueidentifier] NOT NULL CONSTRAINT [dc_sessions]  DEFAULT (newsequentialid()),
	[IdBoat] [uniqueidentifier] NULL,
	[StartDate] [datetime] NULL,
	[StopDate] [datetime] NULL,
 CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED 
(
	[IdSession] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[GPSData]  WITH CHECK ADD  CONSTRAINT [FK_GPSData_Sessions] FOREIGN KEY([IdSession])
REFERENCES [dbo].[Sessions] ([IdSession])
GO
ALTER TABLE [dbo].[GPSData] CHECK CONSTRAINT [FK_GPSData_Sessions]
GO
ALTER TABLE [dbo].[Sessions]  WITH CHECK ADD  CONSTRAINT [FK_Sessions_Boats] FOREIGN KEY([IdBoat])
REFERENCES [dbo].[Boats] ([IdBoat])
GO
ALTER TABLE [dbo].[Sessions] CHECK CONSTRAINT [FK_Sessions_Boats]
GO
