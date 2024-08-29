CREATE PROCEDURE [dbo].[UpdateStudent]
	@StudentId INT,
	@YearResult INT,
	@SectionId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Student]
	 SET [SectionId] = @SectionId,
		 [YearResult] = @YearResult
	 WHERE [Id] = @StudentId;
END