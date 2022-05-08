
/*
Check if database exists and drop
*/

IF EXISTS(

	SELECT 1 FROM master.dbo.sysdatabases
	WHERE name = 'rubric_db'

)

BEGIN
	DROP DATABASE rubric_db
	print '' print '*** dropping rubric_db'
END
GO

print '' print '*** creating rubric_db'

GO
CREATE DATABASE rubric_db
GO

print '' print '*** using rubric_db'

GO
USE [rubric_db]
GO


/*
User Table
*/
print '' print '*** creating rubric_db'
CREATE TABLE [dbo].[User] (
	[UserID]		[nvarchar](50)		NOT NULL
	,[GivenName]		[nvarchar](50)		NOT NULL
	,[FamilyName]	[nvarchar](100)		NOT NULL
	,[PasswordHash]	[nvarchar](100)		NOT NULL DEFAULT 
		'9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E'
	,[Active]		[bit]				NOT NULL DEFAULT 1

	CONSTRAINT [pk_UserID] PRIMARY KEY([UserID])
)
GO


print '' print '*** test records for User'
GO
INSERT INTO [dbo].[User] (
	[UserID]		
	, [GivenName]		
	, [FamilyName]	
)VALUES 
	('joanne@company.com', 'Joanne', 'Smith')
	,('martin@company.com', 'Martin', 'Jones')
	,('ahmed@company.com', 'Ahmed', 'Rawi')
	,('leo@company.com', 'Leo', 'Williams')
	,('maria@company.com', 'Maria', 'Perez')

GO


print '' print '*** creating Role Table'
CREATE TABLE [dbo].[Role] (
	[RoleID]		[nvarchar](50)		NOT NULL
	,[Description]	[nvarchar](255)		NULL
	CONSTRAINT [pk_RoleID] PRIMARY KEY([RoleID])
)

GO

print '' print '*** creating Role test records'
INSERT INTO [dbo].[Role] (
	[RoleID]		
	,[Description]	
) VALUES
('Creator', 'Can create new rubrics and add examples')
,('Administrator', 'Manages users, rubrics, tests, examples')
,('Assessor','Can view rubrics')
,('Norming Trainee', 'Can train and take tests for rubrics')

GO


print '' print '*** creating UserRole table'
CREATE TABLE [dbo].[UserRole](
	[UserID]		[nvarchar](50)		NOT NULL
	,[RoleID]		[nvarchar](50)		NOT NULL
	
	CONSTRAINT [fk_User_UserID] FOREIGN KEY ([UserID])
		REFERENCES [dbo].[User]([UserID]) ON UPDATE CASCADE
	, CONSTRAINT [fk_Role_RoleID] FOREIGN KEY ([RoleID])
		REFERENCES [dbo].[Role]([RoleID]) ON UPDATE CASCADE
)

GO

print '' print '*** adding test values into UserRole Table'

INSERT INTO [dbo].[UserRole](
	[UserID]		
	,[RoleID]	
)VALUES
	('joanne@company.com', 'Administrator')
	,('joanne@company.com', 'Creator')
	,('martin@company.com', 'Creator')
	,('martin@company.com', 'Assessor')
	,('ahmed@company.com', 'Norming Trainee')
	,('leo@company.com', 'Norming Trainee')
	,('maria@company.com', 'Assessor')
	,('maria@company.com', 'Norming Trainee')

GO

/*
Login related stored procedures
*/

print '' print '*** Creating sp_authenticate_user'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user](
	@UserID	[nvarchar](50)
	, @PasswordHash [nvarchar](100)
)
AS
	BEGIN
		SELECT COUNT([UserID]) AS 'Authenticated'
		FROM [User]
		WHERE @UserID = [UserID]
			AND @PasswordHash = [PasswordHash]
			AND [Active] = 1	
	END
GO




print '' print '*** Creating sp_select_user_by_userID'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_userID](
	@UserID	[nvarchar](50)
)
AS
	BEGIN
		SELECT
			[UserID]		
			,[GivenName]	
			,[FamilyName]	
			,[Active]
		FROM [User]
		WHERE @UserID = [UserID]
	
	END

GO


print '' print '*** Creating sp_select_user_roles_by_userID'
GO
CREATE PROCEDURE [dbo].[sp_select_user_roles_by_userID](
	@UserID [nvarchar](50)
)
AS
	BEGIN
		SELECT [RoleID]
		FROM [UserRole]
		WHERE @UserID = [UserID]
	END
GO


/*
Create a way for the user to update password, create an account
*/
print '' print '*** creating sp_update_passwordHash ***'
GO
CREATE PROCEDURE [dbo].[sp_update_passwordHash]
(
	@UserID 				[nvarchar](100),
	@OldPasswordHash	[nvarchar](100),
	@NewPasswordHash	[nvarchar](100)
)
AS
	BEGIN
		UPDATE [dbo].[User]
		SET [PasswordHash] = @NewPasswordHash
		WHERE @UserID = [UserID]
			AND [PasswordHash] = @OldPasswordHash
		RETURN @@ROWCOUNT
	END
GO

/*
ScoreType Table
*/
print '' print '*** creating score_type_table'
CREATE TABLE [dbo].[ScoreType] (
	[ScoreTypeID]		[nvarchar](50)				NOT NULL
	,[Description]		[nvarchar](100)				NOT NULL
	,[Active]			[bit]						NOT NULL DEFAULT 1
	CONSTRAINT [pk_ScoreTypeID] PRIMARY KEY([ScoreTypeID])
)
GO
/*
Sample ScoreType  records
*/
print '' print '*** Adding sample score type records ***'
GO
INSERT INTO [dbo].[ScoreType]
	([ScoreTypeID], [Description])
VALUES
	('Total Earned / Total Possible', '"Total Earned" / "Total Possible", e.g. "12/16"')
	,('Percentage', 'Percentage, e.g. "75%"')
	,('Avg. Facet Score Round Down', 'Average facet score rounded down')
	,('Avg. Facet Score One Dec.', 'Average facet score one decimal, e.g. "2.5"')	
GO

/*
Rubric Table
*/

print '' print '*** creating rubric_table'
CREATE TABLE [dbo].[Rubric] (
	[RubricID]			[int] IDENTITY(100000,1)	NOT NULL
	,[Name]				[nvarchar](50)				NOT NULL
	,[Description]		[nvarchar](100)				NOT NULL
	,[DateCreated]		[DateTime]					NOT NULL DEFAULT CURRENT_TIMESTAMP 
	,[DateUpdated]		[DateTime]					NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[ScoreTypeID]		[nvarchar](50)				NOT NULL
	,[RubricCreator]	[nvarchar](50)				NOT NULL
	,[Active]			[bit]						NOT NULL DEFAULT 1
	,[NumberOfCriteria]	[int]						NOT NULL DEFAULT 3

	CONSTRAINT [pk_RubricID] PRIMARY KEY([RubricID]),
	CONSTRAINT [fk_ScoreTypeID] FOREIGN KEY([ScoreTypeID])
		REFERENCES [dbo].[ScoreType]([ScoreTypeID]) ON UPDATE CASCADE,
	CONSTRAINT [fk_RubricCreator] FOREIGN KEY([RubricCreator])
		REFERENCES [dbo].[User]([UserID])
)
GO

/*
Sample Rubric records
*/
print '' print '*** Adding sample rubric records ***'
GO
INSERT INTO [dbo].[Rubric]
	([Name], [Description], [ScoreTypeID], [RubricCreator])
VALUES
	('General Information and Skills', 'A general rubric for evaluating student knowledge and skill', 'Total Earned / Total Possible', 'joanne@company.com')
	,('Module A: Narration', 'Rubric for a narration type essay for module A', 'Avg. Facet Score One Dec.', 'joanne@company.com')
	
-- 	,('Comapre and Contrast Essay Rubric', 'General rubric for compare and contrast essays', 'Percentage', 'martin@company.com')
	
-- 	, ('IELTS Academic Writing', 'The International English Language Testing System writing rubric', 'Avg. Facet Score Round Down', 'martin@company.com')
	
GO



/*
sp_select_rubric_by_rubric_id	@RubricID	int	IRubricAccessor
*/
print '' print '*** creating sp_select_rubric_by_rubric_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_rubric_by_rubric_id]
(
	@RubricID	[int]
)
AS
	BEGIN
	SELECT
		[RubricID]
		,[Rubric].[Name]	
		,[Rubric].[Description]	
		,[Rubric].[DateCreated]	
		,[Rubric].[DateUpdated]	
		,[Rubric].[ScoreTypeID]	
		,[Rubric].[RubricCreator]
		,[User].[GivenName]		
		,[User].[FamilyName]
		,[User].[Active]
		,[Rubric].[Active]
		,[Rubric].[NumberOfCriteria]

	FROM [Rubric] INNER JOIN [User] 
		ON [Rubric].[RubricCreator] = [User].[UserID]
		
	WHERE @RubricID = [RubricID]	
	END
GO

/*
sp_select_score_type_by_score_type_id	@ScoreTypeID	nvarchar(50)	IScoreTypeAccessor
*/
print '' print '*** creating sp_select_score_type_by_score_type_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_score_type_by_score_type_id]
(
	@ScoreTypeID	[nvarchar](50)
)
AS
	BEGIN
	SELECT
		[ScoreType].[Description]
	FROM [ScoreType]
	WHERE @ScoreTypeID = [ScoreType].[ScoreTypeID]
	END
GO

/*
sp_select_active_rubrics			IRubricAccessor
*/
print '' print '*** creating sp_select_active_rubrics ***'
GO
CREATE PROCEDURE [dbo].[sp_select_active_rubrics]
AS
	BEGIN
	SELECT
		[RubricID]
		,[Rubric].[Name]	
		,[Rubric].[Description]	
		,[Rubric].[DateCreated]	
		,[Rubric].[DateUpdated]	
		,[Rubric].[ScoreTypeID]	
		,[Rubric].[RubricCreator]
		,[User].[GivenName]		
		,[User].[FamilyName]
		,[Rubric].[NumberOfCriteria]
	FROM [Rubric] INNER JOIN [User] 
		ON [Rubric].[RubricCreator] = [User].[UserID]
	WHERE [Rubric].[Active] = 1
	ORDER BY [Rubric].[Name] ASC
	END
GO	



/*
FacetType Table
*/

print '' print '*** creating facet_type_table'
CREATE TABLE [dbo].[FacetType] (
	[FacetTypeID]		[nvarchar](50)				NOT NULL
	,[Description]		[nvarchar](100)				NOT NULL
	,[Active]			[bit]						NOT NULL DEFAULT 1
	CONSTRAINT [pk_FacetTypeID] PRIMARY KEY([FacetTypeID])	
)
GO

/*
Sample FacetType records
*/
print '' print '*** Adding sample facet type records ***'
GO
INSERT INTO [dbo].[FacetType]
	([FacetTypeID],[Description])
VALUES
	('Explanation', 'The big idea in their own words, show work, explain reasoning, make a theory')
	, ('Interpretation', 'Make sense of it')
	, ('Application', 'Those who understand can use knowledge and skill in new situations, be authentic')
	, ('Perspective', 'See things from a different point of view, underlying assumptions, take a stance')
	, ('Empathy', 'Help students understand the diversity of thought, feel for others')
	, ('Self-Knowledge', 'Self assess past and present work')
		
GO


/*
Facet Table
*/

print '' print '*** creating facet_table'
CREATE TABLE [dbo].[Facet] (
	[FacetID]			[nvarchar](100)				NOT NULL
	,[Description]		[nvarchar](255)				NOT NULL
	,[DateCreated]		[DateTime]					NOT NULL DEFAULT CURRENT_TIMESTAMP 
	,[DateUpdated]		[DateTime]					NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Active]			[bit]						NOT NULL DEFAULT 1
	,[RubricID]			[int]						NOT NULL
	,[FacetTypeID]		[nvarchar](50)				NOT NULL

	CONSTRAINT [pk_FacetID] PRIMARY KEY([FacetID],[RubricID]),
	CONSTRAINT [fk_RubricID] FOREIGN KEY([RubricID])
		REFERENCES [dbo].[Rubric]([RubricID]) ON DELETE CASCADE,	
	CONSTRAINT [fk_FacetTypeID] FOREIGN KEY([FacetTypeID])
		REFERENCES [dbo].[FacetType]([FacetTypeID]) ON UPDATE CASCADE
)
GO

/*
Sample Facet records
*/
print '' print '*** Adding sample facet records ***'
GO
INSERT INTO [dbo].[Facet]
	([FacetID],[Description],[RubricID], [FacetTypeID])
VALUES

	('Perspective', 'Does the essay show a unique or interesting perspective?', 100000, 'Perspective')
	, ('Empathy', 'Does knowledge of the topic show empathic skills?', 100000, 'Empathy')
	, ('Reveal self knowledge', 'Does the student show that knowledge of the topic reveals knowledge of self', 100000, 'Self-Knowledge')	
	, ('Explanation', 'Is the students accurate in their knowledge and shows it?', 100000,  'Explanation')
	-- , ('Interpretation', 'Is the intrepetation of the knowledge correct or insightful?', 100000, 'Interpretation') --no criteria added to this facet for testing purposes
	
	, ('Perspective', 'Does the essay show a unique or interesting perspective?', 100001, 'Perspective')
	, ('Empathy', 'Does knowledge of the topic show empathic skills?', 100001, 'Empathy')
	, ('Reveal self knowledge', 'Does the student show that knowledge of the topic reveals knowledge of self', 100001, 'Self-Knowledge')	
	, ('Explanation', 'Is the students accurate in their knowledge and shows it?', 100001,  'Explanation')

/*	
	, ('Effective compare and contrast', 'Explains the reasoning and shows knowledge', 100002,  'Explanation')
	, ('Interpretation of the comparision and contrast', 'Shows an understanding of the comparisons and contrasts', 100002, 'Interpretation')
	, ('Appropriate application of compare and contrast essay', 'Used the knowledge of writing a compare and contrast essay appropriately', 100002, 'Application')	
	
	
	, ('Task Achievement', 'Meets requirements of the task', 100003, 'Interpretation')
	, ('Coherence and cohesion', 'Feeling of cohesion, paragraphing is appropriate', 100003, 'Application')
	, ('Lexical Resourse', 'Appropriate and wide range of vocabulary', 100003,  'Explanation')
	, ('Grammatical Range and Accuracy', 'Flexible and wide range of grammar', 100003,  'Explanation')
*/
GO

/*
sp_select_facets			IFacetAccessor
*/
print '' print '*** creating sp_select_facets ***'
GO
CREATE PROCEDURE [dbo].[sp_select_facets]
AS
	BEGIN
	SELECT
	[FacetID]		
	,[Description]	
	,[DateCreated]	
	,[DateUpdated]	
	,[Active]		
	,[RubricID]		
	,[FacetTypeID]
	FROM [Facet]		
	WHERE [Facet].[Active] = 1
	ORDER BY [Facet].[FacetID] ASC
	END
GO	


/*
sp_select_facets_by_rubric_id	@RubricID	int	IFacetAccessor
*/
print '' print '*** creating sp_select_facets_by_rubric_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_facets_by_rubric_id]
(
	@RubricID	[int]
)
AS
	BEGIN
	SELECT
	[FacetID]		
	,[Description]	
	,[DateCreated]	
	,[DateUpdated]	
	,[Active]		
	,[RubricID]
	,[FacetTypeID]
	FROM [Facet]		
	WHERE [Facet].[Active] = 1
		AND [Facet].[RubricID] = @RubricID
	ORDER BY [Facet].[FacetID] ASC
	END
GO	


/*
Criteria Table
*/

print '' print '*** creating criteria_table'
CREATE TABLE [dbo].[Criteria] (
	[CriteriaID]		[nvarchar](50)				NOT NULL
	,[RubricID]			[int]						NOT NULL
	,[FacetID]			[nvarchar](100)				NOT NULL
	,[Active]			[bit]						NOT NULL DEFAULT 1	
	,[DateCreated]		[DateTime]					NOT NULL DEFAULT CURRENT_TIMESTAMP 
	,[DateUpdated]		[DateTime]					NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Content]			[nvarchar](255)				NOT NULL
	,[Score]			[int]

	CONSTRAINT [pk_CriteriaID] PRIMARY KEY([CriteriaID],[RubricID],[FacetID]),
	CONSTRAINT [fk_RubricID_Criteria] FOREIGN KEY([RubricID])
		REFERENCES [dbo].[Rubric]([RubricID]),	
	CONSTRAINT [fk_FacetID] FOREIGN KEY([FacetID],[RubricID])
		REFERENCES [dbo].[Facet]([FacetID],[RubricID]) ON UPDATE CASCADE ON DELETE CASCADE
)
GO

/*
Sample Criteria records
*/
print '' print '*** Adding sample criteria records ***'
GO
INSERT INTO [dbo].[Criteria]
	(
		[CriteriaID]	
		,[RubricID]		
		,[FacetID]		
		,[Content]		
		,[Score]		
	)
VALUES
		('Excellent', 100000, 'Perspective', 'Student shows execellent perspective about this area of knowledge ', 4)
		,('Above Average', 100000, 'Perspective', 'Student shows above average perspective about this area of knowledge', 3)
		,('Average', 100000, 'Perspective', 'Student shows some perspective about this area of knowledge but not more than what is considered typical', 2)
		,('Below Average', 100000, 'Perspective', 'Student shows little perspective about this area of knowledge or has many misunderstandings.', 1)
		,('Excellent', 100000, 'Empathy', 'Student shows execellent empathy about this area of knowledge ', 4)
		,('Above Average', 100000, 'Empathy', 'Student shows above average empathy about this area of knowledge', 3)
		,('Average', 100000, 'Empathy', 'Student shows some empathy about this area of knowledge but not more than what is considered typical', 2)
		,('Below Average', 100000, 'Empathy', 'Student shows little empathy about this area of knowledge or has many misunderstandings.', 1)
		,('Excellent', 100000, 'Reveal self knowledge', 'Student shows execellent self knowledge about this area of knowledge ', 4)
		,('Above Average', 100000, 'Reveal self knowledge', 'Student shows above average self knowledge about this area of knowledge', 3)
		,('Average', 100000, 'Reveal self knowledge', 'Student shows some self knowledge about this area of knowledge but not more than what is considered typical', 2)
		,('Below Average', 100000, 'Reveal self knowledge', 'Student reveals little self knowledge about this area of knowledge or has many misunderstandings.', 1)
		,('Excellent', 100000, 'Explanation', 'Student gives an execellent explanation about this area of knowledge ', 4)
		,('Above Average', 100000, 'Explanation', 'Student shows above average knowledge about the subject', 3)
		,('Average', 100000, 'Explanation', 'Student gives some explanation about this area of knowledge but not more than what is considered typical', 2)
		,('Below Average', 100000, 'Explanation', 'Student shows little knowledge or has many misunderstandings.', 1)
		
		,('A', 100001, 'Perspective', 'Student shows execellent perspective about this area of knowledge ', 4)
		,('B', 100001, 'Perspective', 'Student shows above average perspective about this area of knowledge', 3)
		,('C', 100001, 'Perspective', 'Student shows some perspective about this area of knowledge but not more than what is considered typical', 2)
		,('D', 100001, 'Perspective', 'Student shows little perspective about this area of knowledge or has many misunderstandings.', 1)
		,('F', 100001, 'Perspective', 'Student shows no perspective.', 0)
		,('A', 100001, 'Empathy', 'Student shows execellent empathy about this area of knowledge ', 4)
		,('B', 100001, 'Empathy', 'Student shows above average empathy about this area of knowledge', 3)
		,('C', 100001, 'Empathy', 'Student shows some empathy about this area of knowledge but not more than what is considered typical', 2)
		,('D', 100001, 'Empathy', 'Student shows little empathy about this area of knowledge or has many misunderstandings.', 1)
		,('F', 100001, 'Empathy', 'Student shows no empathy.', 0)
		,('A', 100001, 'Reveal self knowledge', 'Student shows execellent self knowledge about this area of knowledge ', 4)
		,('B', 100001, 'Reveal self knowledge', 'Student shows above average self knowledge about this area of knowledge', 3)
		,('C', 100001, 'Reveal self knowledge', 'Student shows some self knowledge about this area of knowledge but not more than what is considered typical', 2)
		,('D', 100001, 'Reveal self knowledge', 'Student reveals little self knowledge about this area of knowledge or has many misunderstandings.', 1)
		,('F', 100001, 'Reveal self knowledge', 'Student reveals no self knowledge.', 0)
		,('A', 100001, 'Explanation', 'Student gives an execellent explanation about this area of knowledge ', 4)
		,('B', 100001, 'Explanation', 'Student shows above average knowledge about the subject', 3)
		,('C', 100001, 'Explanation', 'Student gives some explanation about this area of knowledge but not more than what is considered typical', 2)
		,('D', 100001, 'Explanation', 'Student shows little knowledge or has many misunderstandings.', 1)	
		,('F', 100001, 'Explanation', 'Student shows no knowledge.', 0)
		
GO



/*
sp_select_criteria_by_rubric_id	@RubricID	int	ICriteriaAccessor
*/
print '' print '*** creating sp_select_criteria_by_rubric_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_criteria_by_rubric_id]
(
	@RubricID	[int]
)
AS
	BEGIN
	SELECT
		[CriteriaID]
		,[RubricID]
		,[FacetID]	
		,[Active]	
		,[DateCreated]
		,[DateUpdated]
		,[Content]	
		,[Score]	
	FROM [Criteria]
	WHERE @RubricID = [RubricID]
	ORDER BY [FacetID], [Score] DESC
	END
	
GO



/*
sp_update_criteria_by_criteria_id	ICriteriaAccessor
*/
print '' print '*** creating sp_update_criteria_by_criteria_id ***'
GO
CREATE PROCEDURE [dbo].[sp_update_criteria_by_criteria_id]
(
	@RubricID	int
	,@FacetID	nvarchar(50)
	,@OldCriteriaID		[nvarchar](50)	
	,@OldContent		[nvarchar](255)
	,@NewCriteriaID		[nvarchar](50)
	,@NewContent		[nvarchar](255)
)
AS
	BEGIN
		UPDATE [Criteria]
		SET 
			[CriteriaID] = @NewCriteriaID
			,[DateUpdated] = CURRENT_TIMESTAMP
			,[Content] = @NewContent
		WHERE 
			[RubricID] = @RubricID
			AND [FacetID] = @FacetID
			AND [CriteriaID] = @OldCriteriaID
			AND [Content] = @OldContent
		RETURN @@ROWCOUNT
	END	
GO

/*
sp_update_criteria_content_by_criteria_id	@CriteriaID	nvarchar(50)	ICriteriaAccessor	ICriteriaAccessor
*/
print '' print '*** creating sp_update_criteria_content_by_criteria_id ***'
GO
CREATE PROCEDURE [dbo].[sp_update_criteria_content_by_criteria_id]
(
	@RubricID			int
	,@FacetID			nvarchar(50)
	,@CriteriaID		[nvarchar](50)	
	,@OldContent		[nvarchar](255)
	,@NewContent		[nvarchar](255)
)
AS
	BEGIN
		UPDATE [Criteria]
		SET 
			[DateUpdated] = CURRENT_TIMESTAMP
			,[Content] = @NewContent
		WHERE 
			[RubricID] = @RubricID
			AND [FacetID] = @FacetID
			AND [CriteriaID] = @CriteriaID
			AND [Content] = @OldContent
		RETURN @@ROWCOUNT
	END	
GO

/*
sp_create_rubric	@Name	nvarchar(50)	IRubricAccessor																					
*/
print '' print '*** creating sp_create_rubric ***'
GO
CREATE PROCEDURE [dbo].[sp_create_rubric]
(
	@Name			nvarchar(50)
	,@Description	nvarchar(100)
	,@ScoreTypeID	nvarchar(50)
	,@RubricCreator	nvarchar(50)
)
AS
	BEGIN
		INSERT INTO [dbo].[Rubric]
		(
			[Name]				
			,[Description]					
			,[ScoreTypeID]		
			,[RubricCreator]		
		)
		VALUES
		(@Name, @Description, @ScoreTypeID, @RubricCreator)		
	END	
GO


/*
sp_select_score_types			IScoreTypeAccessor
*/
print '' print '*** creating sp_select_score_types ***'
GO
CREATE PROCEDURE [dbo].[sp_select_score_types]
AS
	BEGIN
		SELECT
			[ScoreType].[ScoreTypeID]
			,[ScoreType].[Description]
		FROM [ScoreType]
	END	
GO


/*
sp_create_facet	@RubricID	int	IFacetAccessor																					
*/
print '' print '*** creating sp_create_facet ***'
GO
CREATE PROCEDURE [dbo].[sp_create_facet]
(
	@RubricID		int
	,@FacetID		nvarchar(50)
	,@Description	nvarchar(100)
	,@FacetTypeID	nvarchar(50)
)
AS
	BEGIN
		INSERT INTO [dbo].[Facet]
		(
			[RubricID]
			,[FacetID]
			,[Description]
			,[FacetTypeID]
		)
		VALUES
		(@RubricID, @FacetID, @Description, @FacetTypeID)
	END	
GO


/*
sp_select_facet_types			IFacetTypeAccessor
*/
print '' print '*** creating sp_select_facet_types ***'
GO
CREATE PROCEDURE [dbo].[sp_select_facet_types]
AS
	BEGIN
		SELECT
			[FacetTypeID]
			,[Description]
			,[Active]
		FROM [FacetType]
	END	
GO



/*
sp_select_rubric_by_name_description_score_type_id_rubric_creator	IRubricAccessor																				
*/
print '' print '*** creating sp_select_rubric_by_name_description_score_type_id_rubric_creator ***'
GO
CREATE PROCEDURE [dbo].[sp_select_rubric_by_name_description_score_type_id_rubric_creator]
(
	@Name			nvarchar(50)
	,@Description	nvarchar(100)
	,@ScoreTypeID	nvarchar(50)
	,@RubricCreator	nvarchar(50)
)
AS
	BEGIN
		SELECT
			[RubricID]
			,[Rubric].[Name]	
			,[Rubric].[Description]	
			,[Rubric].[DateCreated]	
			,[Rubric].[DateUpdated]	
			,[Rubric].[ScoreTypeID]	
			,[Rubric].[RubricCreator]
			,[User].[GivenName]		
			,[User].[FamilyName]
			,[User].[Active]
			,[Rubric].[Active]	
			,[Rubric].[NumberOfCriteria]
		
		FROM [Rubric] INNER JOIN [User] 
			ON [Rubric].[RubricCreator] = [User].[UserID]
		WHERE
			[Name] = @Name			
			AND [Description] = @Description
			AND [ScoreTypeID] = @ScoreTypeID
			AND [RubricCreator]	= @RubricCreatoR
		RETURN @@ROWCOUNT
		
	END	
GO



/*
sp_create_criteria_by_rubric_id_and_facet_id	ICriteriaAccessor
*/
print '' print '*** creating sp_create_criteria_by_rubric_id_and_facet_id ***'
GO
CREATE PROCEDURE [dbo].[sp_create_criteria_by_rubric_id_and_facet_id]
(
	 @CriteriaID		[nvarchar](50)
	,@RubricID			[int]		
	,@FacetID			[nvarchar](100)
	,@Content			[nvarchar](255)
	,@Score				[int]
)
AS
	BEGIN
		INSERT INTO [dbo].[Criteria]
		(
			[CriteriaID]
			,[RubricID]	
			,[FacetID]	
			,[Content]	
			,[Score]
		)
		VALUES
		(@CriteriaID, @RubricID ,@FacetID, @Content ,@Score)
	END	
GO


/*
sp_update_rubric_by_rubric_id	IRubricAccessor
*/
print '' print '*** creating sp_update_rubric_by_rubric_id ***'
GO
CREATE PROCEDURE [dbo].[sp_update_rubric_by_rubric_id]
(
	@RubricID			int
	,@OldName			nvarchar(50)
	,@OldDescription		nvarchar(100)
	,@NewName			nvarchar(50)
	,@NewDescription		nvarchar(100)
	,@OldScoreTypeID		nvarchar(50)
	,@NewScoreTypeID		nvarchar(50)
)
AS
	BEGIN
		UPDATE [Rubric]
		SET 
			[Name] = @NewName
			,[Description] = @NewDescription
			,[DateUpdated] = CURRENT_TIMESTAMP
			,[ScoreTypeID] = @NewScoreTypeID			
		WHERE 
			[Name] = @OldName
			AND [Description] = @OldDescription
			AND [ScoreTypeID] = @OldScoreTypeID
		RETURN @@ROWCOUNT
	END	
GO


/*
sp_update_facet_description_by_rubric_id_and_facet_id	IFacetAccessor
*/
print '' print '*** creating sp_update_facet_description_by_rubric_id_and_facet_id ***'
GO
CREATE PROCEDURE [dbo].[sp_update_facet_description_by_rubric_id_and_facet_id]
(
	@RubricID			int
	,@FacetID			nvarchar(50)
	,@OldDescription	nvarchar(100)
	,@NewDescription	nvarchar(100)
)
AS
	BEGIN
		UPDATE [Facet]
		SET
			[Description] = @NewDescription
			,[DateUpdated] = CURRENT_TIMESTAMP			
		WHERE
			[Description] = @OldDescription			
		RETURN @@ROWCOUNT
	END	
GO


/*
sp_deactivate_rubric_by_rubric_id	IRubricAccessor
*/
print '' print '*** creating sp_deactivate_rubric_by_rubric_id ***'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_rubric_by_rubric_id]
(
	@RubricID			int
)
AS
	BEGIN
		UPDATE [Rubric]
		SET			
			[DateUpdated] = CURRENT_TIMESTAMP
			,[Active] = 0
		WHERE
			[RubricID] = @RubricID
		RETURN @@ROWCOUNT
	END	
GO




/*
sp_delete_rubric_by_rubric_id	@RubricID	int	IRubricAccessor
*/
print '' print '*** creating sp_delete_rubric_by_rubric_id ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_rubric_by_rubric_id]
(
	@RubricID			int
)
AS
	BEGIN
		DELETE FROM [Rubric]
		WHERE
			[RubricID] = @RubricID
		RETURN @@ROWCOUNT
	END	
GO


/*
sp_delete_facet_by_rubric_id_and_facet_id	IFacetAccessor
*/
print '' print '*** creating sp_delete_facet_by_rubric_id_and_facet_id ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_facet_by_rubric_id_and_facet_id]
(
	@RubricID			int
	,@FacetID			nvarchar(50)	
)
AS
	BEGIN
		DELETE FROM [Facet]
		WHERE
			[RubricID] = @RubricID
			AND [FacetID] = @FacetID
		RETURN @@ROWCOUNT
	END	
GO


print '' print '*** creating Subject Table'
CREATE TABLE [dbo].[Subject] (

	[SubjectID]		[nvarchar](50)		NOT NULL
	,[Description]	[nvarchar](100)		NOT NULL
	,[DateCreated]	[DateTime]			NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[DateUpdated]	[DateTime]			NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Active]		[bit]				NOT NULL DEFAULT 1

	CONSTRAINT [pk_SubjectID] PRIMARY KEY([SubjectID])
)

GO

print '' print '*** creating Subject test records'
INSERT INTO [dbo].[Subject] (
	[SubjectID]		
	,[Description]	
) VALUES
	('English As Second Language', 'Rubrics that could be used for ESL')
	,('Composition', 'Rubrics for Composition')
GO

print '' print '*** creating Rubric Subject Table'
CREATE TABLE [dbo].[RubricSubject] (

	[SubjectID]		[nvarchar](50)		NOT NULL
	,[RubricID]			[int] 			NOT NULL
	,[DateCreated]	[DateTime]			NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Active]		[bit]				NOT NULL DEFAULT 1

	CONSTRAINT [pk_SubjectID_Rubric_ID] PRIMARY KEY([SubjectID],[RubricID])
	, CONSTRAINT [fk_Rubric_Subect_SubjectID] FOREIGN KEY([SubjectID])
		REFERENCES [dbo].[Subject]([SubjectID]) ON UPDATE CASCADE
	, CONSTRAINT [fk_Rubric_Subect_RubricID] FOREIGN KEY([RubricID])
		REFERENCES [dbo].[Rubric]([RubricID]) ON DELETE CASCADE
)
GO


print '' print '*** creating RubricSubject test records'

INSERT INTO [dbo].[RubricSubject] (
	[SubjectID]		
	,[RubricID]	
) VALUES
	('English As Second Language', 100000)
	,('Composition', 100000)
	,('English As Second Language', 100001)
	
--	,('English As Second Language', 100002)
	,('Composition', 100001)
	
GO


/*
sp_select_subjects			ISubjectAccessor
*/
print '' print '*** creating sp_select_subjects ***'
GO
CREATE PROCEDURE [dbo].[sp_select_subjects]
AS
	BEGIN
		SELECT
			[SubjectID]		
			,[Description]	
			,[DateCreated]	
			,[DateUpdated]	
			,[Active]		
		FROM [Subject]
		ORDER BY [SubjectID] ASC
	END	
GO

/*
sp_select_rubric_subjects_by_rubric_id	@RubricID	int	IRubricSubjectAccessor
*/
print '' print '*** creating sp_select_rubric_subjects_by_rubric_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_rubric_subjects_by_rubric_id]
(
	@RubricID			int
)
AS
	BEGIN
		SELECT
		[SubjectID]	
		,[RubricID]	
		,[DateCreated]
		,[Active]	
		FROM [RubricSubject]
		WHERE [RubricID] = @RubricID
		ORDER BY [SubjectID] ASC
	END	
GO


	
/*
sp_create_rubric_subject	@SubjectID	nvarchar(50)	IRubricSubjectAccessor
	@RubricID	int	
	@Description	nvarchar(100)	
*/
print '' print '*** creating sp_create_rubric_subject ***'
GO
CREATE PROCEDURE [dbo].[sp_create_rubric_subject]
(
	@SubjectID		nvarchar(50)
	,@RubricID		int
	,@Description	nvarchar(100)
)
AS
	BEGIN 	
		BEGIN TRANSACTION [tr_Rubric_Subject]	
		
		BEGIN TRY		
			INSERT INTO [dbo].[Subject]
			(
				[SubjectID]	
				,[Description]
			)
			VALUES
			(@SubjectID, @Description)
		
		
			INSERT INTO [dbo].[RubricSubject]
			(
				[SubjectID]	
				,[RubricID]
			)
			VALUES
			(@SubjectID, @RubricID)
			
			COMMIT TRANSACTION [tr_Rubric_Subject]
		
		END TRY
		
		BEGIN CATCH
		
			ROLLBACK TRANSACTION [tr_Rubric_Subject]
		
		END CATCH
		RETURN @@ROWCOUNT		
		
	END	
GO


/*
sp_delete_rubric_subject_by_subject_id_and_rubric_id 	IRubricSubjectAccessor
	@SubjectID	nvarchar(50)	
	@RubricID	int	
*/
print '' print '*** creating sp_delete_rubric_subject_by_subject_id_and_rubric_id ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_rubric_subject_by_subject_id_and_rubric_id]
(
	@SubjectID		nvarchar(50)
	,@RubricID		int
)
AS
	BEGIN
		DELETE FROM [RubricSubject]
		WHERE
			[SubjectID] = @SubjectID
			AND
			[RubricID] = @RubricID
		RETURN @@ROWCOUNT
	END	
GO


print '' print '*** Creating sp_select_all_roles'
GO
CREATE PROCEDURE [sp_select_all_roles]
AS
BEGIN
	SELECT [RoleID]
	FROM [dbo].[Role]
	ORDER BY [RoleID]
END
GO

print '' print '*** Creating sp_select_roles_by_userID'
GO
CREATE PROCEDURE [sp_select_roles_by_userID]
(
	@UserID 	nvarchar(50)
)
AS
BEGIN
	SELECT 	[RoleID]
	FROM 	[dbo].[UserRole]
	WHERE 	[UserID] = @UserID
END
GO

-- sp_create_user
print '' print '*** Creating sp_create_user'
GO
CREATE PROCEDURE [sp_create_user]
(
	@UserID 	nvarchar(50),
	@GivenName	nvarchar(50),
	@FamilyName nvarchar(50)
)
AS
	BEGIN
		INSERT INTO [dbo].[User] (
			[UserID]		
			, [GivenName]		
			, [FamilyName]	
		)VALUES 
			(@UserID, @GivenName, @FamilyName)
	END
GO


print '' print '*** Creating sp_delete_employee_role'
GO
CREATE PROCEDURE [sp_delete_employee_role]
(
	@UserID			[nvarchar](50),
	@RoleID			[nvarchar](50)
)
AS
BEGIN
	DELETE FROM [dbo].[UserRole]
	WHERE [UserID] =	@UserID
	  AND [RoleID] = 	@RoleID
END
GO

print '' print '*** Creating sp_insert_employee_role'
GO
CREATE PROCEDURE [sp_insert_employee_role]
(
	@UserID			[nvarchar](50),
	@RoleID				[nvarchar](50)
)
AS
BEGIN
INSERT INTO [dbo].[UserRole]
	([UserID], [RoleID])
	VALUES
	(@UserID, @RoleID)
END
GO




/*
sp_select_facet_by_rubric_id_and_facet_id	@FacetID	nvarchar(50)	IFacetAccessor
*/
print '' print '*** creating sp_select_facet_by_rubric_id_and_facet_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_facet_by_rubric_id_and_facet_id]
(
	@RubricID	[int],
	@FacetID	[nvarchar](50)
)
AS
	BEGIN
	SELECT
	[FacetID]		
	,[Description]	
	,[DateCreated]	
	,[DateUpdated]	
	,[Active]		
	,[RubricID]
	,[FacetTypeID]
	FROM [Facet]		
	WHERE [Facet].[FacetID] = @FacetID
		AND [Facet].[RubricID] = @RubricID
	END
GO	





/*
sp_select_criteria_by_rubric_id_and_facet_id	@FacetID	nvarchar(50)	IFacetAccessor
*/
print '' print '*** creating sp_select_criteria_by_rubric_id_and_facet_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_criteria_by_rubric_id_and_facet_id]
(
	@RubricID	[int],
	@FacetID	[nvarchar](50)
)
AS
	BEGIN
	SELECT
		[CriteriaID]
		,[RubricID]
		,[FacetID]	
		,[Active]	
		,[DateCreated]
		,[DateUpdated]
		,[Content]	
		,[Score]	
	FROM [Criteria]
	WHERE @RubricID = [RubricID]
		AND @FacetID = [FacetID]
	END
	
GO






/*
sp_update_facets_by_rubric_id	@RubricID	int	IFacetAccessor
*/
print '' print '*** creating sp_update_facets_by_rubric_id ***'
GO
CREATE PROCEDURE [dbo].[sp_update_facets_by_rubric_id]
(
	@RubricID	int
	,@OldFacetID	nvarchar(50)
	,@NewFacetID	nvarchar(50)
	,@OldDescription	nvarchar(100)
	,@NewDescription	nvarchar(100)
	,@OldFacetType	nvarchar(50)
	,@NewFacetType	nvarchar(50)
)
AS
	BEGIN
		UPDATE [Facet]
		SET
			[FacetID] = @NewFacetID
			,[Description] = @NewDescription
			,[FacetTypeID] = @NewFacetType
			,[DateUpdated] = CURRENT_TIMESTAMP
			
		WHERE
			[Description] = @OldDescription	
			AND
			[FacetID] = @OldFacetID
			AND
			[FacetTypeID] = @OldFacetType
			
		RETURN @@ROWCOUNT
	END	
GO



/*
sp_create_rubric_with_one_facet		IRubricAccessor
*/
print '' print '*** creating sp_create_rubric_with_one_facet ***'
GO
CREATE PROCEDURE [dbo].[sp_create_rubric_with_one_facet]
(
	@RubricName				nvarchar(50)
	,@RubricDescription		nvarchar(100)	
	,@ScoreTypeID			nvarchar(50)	
	,@RubricCreator			nvarchar(50)	
	,@NumberOfCriteria		int	
	,@FacetID				nvarchar(100)	
	,@FacetDescription		nvarchar(255)	
	,@FacetType				nvarchar(50)	
)
AS
	BEGIN -- SP
		BEGIN TRAN
			BEGIN TRY
				
				DECLARE @RubricID INT
		
				-- insert into rubric
				INSERT INTO [dbo].[Rubric]
				(
					[Name]				
					,[Description]
					,[ScoreTypeID]
					,[RubricCreator]
					,[NumberOfCriteria]
				)
				OUTPUT Inserted.RubricID
				VALUES
				(
					@RubricName
					, @RubricDescription
					, @ScoreTypeID
					, @RubricCreator
					, @NumberOfCriteria
				)
				
				SET @RubricID = SCOPE_IDENTITY()
				
				-- insert into facet
				
				INSERT INTO [dbo].[Facet]
				(
					[RubricID]
					,[FacetID]
					,[Description]
					,[FacetTypeID]
				)
				VALUES
				(
					@RubricID
					, @FacetID
					, @FacetDescription
					, @FacetType
				)			
				
				COMMIT TRANSACTION
			
			END TRY
			BEGIN CATCH
				ROLLBACK TRANSACTION
			END CATCH
		
		
		
	END -- SP
GO
