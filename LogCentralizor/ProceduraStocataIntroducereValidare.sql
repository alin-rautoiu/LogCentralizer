USE [LogBD]
GO
/****** Object:  StoredProcedure [dbo].[InsertWithValidation]    Script Date: 07.01.2014 17:37:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[InsertWithValidation]
	-- Add the parameters for the stored procedure here
	@LogTime nvarchar(255), @Action nvarchar(255), @FolderPath nvarchar(255), @Filename nvarchar(255), @Username nvarchar(255), @IPADDRESS nvarchar(255), @XferSize nvarchar(255), @Duration nvarchar(255), @AgentBrand nvarchar(255), @AgentVersion nvarchar(255), @Error nvarchar(255)
AS
IF  NOT EXISTS (SELECT * FROM LogTable WHERE
	LogTime = @LogTime AND
	[Action] = @Action AND
	FolderPath = @FolderPath AND
	[Filename] = @Filename AND
	Username = @Username AND
	IPADDRESS = @IPADDRESS AND
	Duration = @Duration AND
	AgentBrand =@AgentBrand AND
	AgentVersion = @AgentVersion AND
	Error = @Error
)
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	INSERT INTO LogTable
	VALUES
	(
	@LogTime,
	@Action,
	@FolderPath,
	@Filename,	
	@Username,
	@IPADDRESS,
	@XferSize,
	@Duration,
	@AgentBrand,
	@AgentVersion,
	@Error,
	NEWID()
	)
END
ELSE
BEGIN
RETURN -1
END
