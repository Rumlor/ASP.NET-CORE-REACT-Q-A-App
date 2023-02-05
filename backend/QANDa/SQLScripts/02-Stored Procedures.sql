CREATE PROC QandA.Answer_Delete
	(
	@AnswerId int
)
AS
BEGIN
	SET NOCOUNT ON

	DELETE
	FROM QandA.Answer
	WHERE AnswerID = @AnswerId
END
GO

CREATE PROC QandA.Answer_Get_ByQuestionId
	(
	@QuestionId int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT AnswerId, QuestionId, Content, Username, Created
	FROM QandA.Answer 
	WHERE QuestionId = @QuestionId
END
GO

CREATE PROC QandA.Answer_Post
	(
	@QuestionId int,
	@Content nvarchar(max),
	@UserId nvarchar(150),
	@UserName nvarchar(150),
	@Created datetime2
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO QandA.Answer
		(QuestionId, Content, UserId, UserName, Created)
	SELECT @QuestionId, @Content, @UserId, @UserName, @Created

	SELECT AnswerId, Content, UserName, UserId, Created
	FROM QandA.Answer
	WHERE AnswerId = SCOPE_IDENTITY()
END
GO

CREATE PROC QandA.Answer_Put
	(
	@AnswerId int,
	@Content nvarchar(max)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE QandA.Answer
	SET Content = @Content
	WHERE AnswerId = @AnswerId

	SELECT a.AnswerId, a.QuestionId, a.Content, u.UserName, a.Created
	FROM QandA.Answer a
		LEFT JOIN AspNetUsers u ON a.UserId = u.Id
	WHERE AnswerId = @AnswerId
END
GO


CREATE PROC QandA.Question_AddForLoadTest
AS
BEGIN
	DECLARE @i int = 1

	WHILE @i < 10000
	BEGIN
		INSERT INTO QandA.Question
			(Title, Content, UserId, UserName, Created)
		VALUES('Question ' + CAST(@i AS nvarchar(5)), 'Content ' + CAST(@i AS nvarchar(5)), 'User1', 'User1', GETUTCDATE())
		SET @i = @i + 1
	END
END
GO

CREATE PROC QandA.Question_Delete
	(
	@QuestionId int
)
AS
BEGIN
	SET NOCOUNT ON

	DELETE
	FROM QandA.Question
	WHERE QuestionID = @QuestionId
END
GO

CREATE PROC QandA.Question_Exists
	(
	@QuestionId int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT CASE WHEN EXISTS (SELECT QuestionId
		FROM QandA.Question
		WHERE QuestionId = @QuestionId) 
        THEN CAST (1 AS BIT) 
        ELSE CAST (0 AS BIT) END AS Result

END
GO

CREATE PROC QandA.Question_GetMany
AS
BEGIN
	SET NOCOUNT ON

	SELECT QuestionId, Title, Content, UserId, UserName, Created
	FROM QandA.Question 
END
GO

CREATE PROC QandA.Question_GetMany_BySearch
	(
	@Search nvarchar(100)
)
AS
BEGIN
	SET NOCOUNT ON

		SELECT QuestionId, Title, Content, UserId, UserName, Created
		FROM QandA.Question 
		WHERE Title LIKE '%' + @Search + '%'

	UNION

		SELECT QuestionId, Title, Content, UserId, UserName, Created
		FROM QandA.Question 
		WHERE Content LIKE '%' + @Search + '%'
END
GO

CREATE PROC QandA.Question_GetMany_BySearch_WithPaging
	(
	@Search nvarchar(100),
	@PageNumber int,
	@PageSize int
)
AS
BEGIN
	SELECT QuestionId, Title, Content, UserId, UserName, Created
	FROM
		(	SELECT QuestionId, Title, Content, UserId, UserName, Created
			FROM QandA.Question 
			WHERE Title LIKE '%' + @Search + '%'

		UNION

			SELECT QuestionId, Title, Content, UserId, UserName, Created
			FROM QandA.Question 
			WHERE Content LIKE '%' + @Search + '%') Sub
	ORDER BY QuestionId
	OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
END
GO

CREATE PROC QandA.Question_GetMany_WithAnswers
AS
BEGIN
	SET NOCOUNT ON

	SELECT q.QuestionId, q.Title, q.Content, q.UserName, q.Created,
		a.QuestionId, a.AnswerId, a.Content, a.Username, a.Created
	FROM QandA.Question q
		LEFT JOIN QandA.Answer a ON q.QuestionId = a.QuestionId
END
GO

CREATE PROC QandA.Question_GetSingle
	(
	@QuestionId int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT QuestionId, Title, Content, UserId, Username, Created
	FROM QandA.Question 
	WHERE QuestionId = @QuestionId
END
GO

CREATE PROC QandA.Question_GetUnanswered
AS
BEGIN
	SET NOCOUNT ON

	SELECT QuestionId, Title, Content, UserId, UserName, Created
	FROM QandA.Question q
	WHERE NOT EXISTS (SELECT *
	FROM QandA.Answer a
	WHERE a.QuestionId = q.QuestionId)
END
GO

CREATE PROC QandA.Question_Post
	(
	@Title nvarchar(100),
	@Content nvarchar(max),
	@UserId nvarchar(150),
	@UserName nvarchar(150),
	@Created datetime2
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO QandA.Question
		(Title, Content, UserId, UserName, Created)
	VALUES(@Title, @Content, @UserId, @UserName, @Created)

	SELECT SCOPE_IDENTITY() AS QuestionId
END
GO

CREATE PROC QandA.Question_Put
	(
	@QuestionId int,
	@Title nvarchar(100),
	@Content nvarchar(max)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE QandA.Question
	SET Title = @Title, Content = @Content
	WHERE QuestionID = @QuestionId
END
GO

CREATE PROC QandA.Answer_Get_ByAnswerId
	(
	@AnswerId int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT AnswerId, Content, Username, Created
	FROM QandA.Answer 
	WHERE AnswerId = @AnswerId
END
GO