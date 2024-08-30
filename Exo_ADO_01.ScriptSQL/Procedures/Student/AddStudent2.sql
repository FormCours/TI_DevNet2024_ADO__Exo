CREATE PROCEDURE [dbo].[AddStudent2]
	@FirstName VARCHAR(50), 
	@LastName VARCHAR(50),
	@BirthDate DATETIME2,
	@YearResult INT,
	@SectionId INT,
	@StudentId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Student] ([FirstName], [LastName], [SectionId], [YearResult], [BirthDate])
	 VALUES (@FirstName, @LastName, @SectionId, @YearResult, @BirthDate);

	SET @StudentId = SCOPE_IDENTITY();
END
