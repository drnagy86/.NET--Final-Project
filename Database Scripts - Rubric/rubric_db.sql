
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
,('Admin', 'Manages users, rubrics, tests, examples')
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
	('joanne@company.com', 'Admin')
	,('joanne@company.com', 'Creator')
	,('martin@company.com', 'Creator')
	,('martin@company.com', 'Assessor')
	,('ahmed@company.com', 'Norming Trainee')
	,('leo@company.com', 'Norming Trainee')
	,('maria@company.com', 'Norming Trainee')
	,('maria@company.com', 'Assessor')

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

	CONSTRAINT [pk_RubricID] PRIMARY KEY([RubricID]),
	CONSTRAINT [fk_ScoreTypeID] FOREIGN KEY([ScoreTypeID])
		REFERENCES [dbo].[ScoreType]([ScoreTypeID]),
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
	,('Comapre and Contrast Essay Rubric', 'General rubric for compare and contrast essays', 'Percentage', 'martin@company.com')
	, ('IELTS Academic Writing', 'The International English Language Testing System writing rubric', 'Avg. Facet Score Round Down', 'martin@company.com')
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
		REFERENCES [dbo].[Rubric]([RubricID]),	
	CONSTRAINT [fk_FacetTypeID] FOREIGN KEY([FacetTypeID])
		REFERENCES [dbo].[FacetType]([FacetTypeID])
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
	('Explanation of the topic of the essay', 'How much knowledge of the topic is shown', 100001, 'Explanation')
	, ('Interpretation of the topic', 'Is the interpretation correct?', 100001, 'Interpretation')
	, ('Application of a narration esssay', 'Understands and can authenticly write a narration essay', 100001, 'Application')
	, ('Perspective', 'Does the essay show a unique or interesting perspective?', 100000, 'Perspective')
	, ('Empathy', 'Does knowledge of the topic show empathic skills?', 100000, 'Empathy')
	, ('Reveal self knowledge', 'Does the student show that knowledge of the topic reveals knowledge of self', 100000, 'Self-Knowledge')	
	, ('Explanation', 'Is the students accurate in their knowledge and shows it?', 100000,  'Explanation')
	-- , ('Interpretation', 'Is the intrepetation of the knowledge correct or insightful?', 100000, 'Interpretation')	
	, ('Effective compare and contrast', 'Explains the reasoning and shows knowledge', 100002,  'Explanation')
	, ('Interpretation of the comparision and contrast', 'Shows an understanding of the comparisons and contrasts', 100002, 'Interpretation')
	, ('Appropriate application of compare and contrast essay', 'Used the knowledge of writing a compare and contrast essay appropriately', 100002, 'Application')	
	, ('Task Achievement', 'Meets requirements of the task', 100003, 'Interpretation')
	, ('Coherence and cohesion', 'Feeling of cohesion, paragraphing is appropriate', 100003, 'Application')
	, ('Lexical Resourse', 'Appropriate and wide range of vocabulary', 100003,  'Explanation')
	, ('Grammatical Range and Accuracy', 'Flexible and wide range of grammar', 100003,  'Explanation')
	
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
		REFERENCES [dbo].[Facet]([FacetID],[RubricID]) ON UPDATE CASCADE
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
















