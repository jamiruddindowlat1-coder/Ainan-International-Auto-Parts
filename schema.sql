SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Loans]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Loans](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoanType] [nvarchar](max) NOT NULL,
	[LoanTitle] [nvarchar](max) NOT NULL,
	[LenderName] [nvarchar](max) NULL,
	[PrincipalAmount] [decimal](18, 2) NOT NULL,
	[InterestRate] [decimal](5, 2) NOT NULL,
	[TenureMonths] [int] NOT NULL,
	[TotalRepayable] [decimal](18, 2) NOT NULL,
	[AmountPaid] [decimal](18, 2) NOT NULL,
	[OutstandingBalance] [decimal](18, 2) NOT NULL,
	[LoanDate] [datetime2](7) NOT NULL,
	[DueDate] [datetime2](7) NULL,
	[Status] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Reference] [nvarchar](max) NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Loans] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Items]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Items](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [nvarchar](max) NULL,
	[Unit] [nvarchar](max) NULL,
	[CurrentStock] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FounderAccounts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FounderAccounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](150) NOT NULL,
	[AccountType] [nvarchar](30) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[Address] [nvarchar](200) NULL,
	[Notes] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[JoinDate] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ProfileImageUrl] [nvarchar](max) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[Designation] [nvarchar](50) NULL,
	[NationalId] [nvarchar](50) NULL,
	[SharePercentage] [decimal](5, 2) NOT NULL,
	[TermEndDate] [datetime2](7) NULL,
	[TermStartDate] [datetime2](7) NULL,
	[TinNumber] [nvarchar](30) NULL,
 CONSTRAINT [PK_FounderAccounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FinanceCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FinanceCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_FinanceCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Incomes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Incomes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Category] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Reference] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[PaymentMethod] [nvarchar](max) NULL,
 CONSTRAINT [PK_Incomes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Permissions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Module] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Departments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Departments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Code] [nvarchar](20) NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[TotalDue] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Expenses]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Expenses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Category] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Reference] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[PaymentMethod] [nvarchar](max) NULL,
 CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommunicationLogs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommunicationLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NULL,
	[TargetAudience] [nvarchar](max) NULL,
	[Recipients] [nvarchar](max) NULL,
	[Subject] [nvarchar](max) NULL,
	[Message] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[RecipientCount] [int] NOT NULL,
	[ErrorDetail] [nvarchar](max) NULL,
	[SentByUserId] [int] NULL,
	[SentByName] [nvarchar](max) NULL,
	[SentAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_CommunicationLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChartOfAccounts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ChartOfAccounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountCode] [nvarchar](450) NULL,
	[AccountName] [nvarchar](max) NULL,
	[AccountType] [nvarchar](max) NULL,
	[AccountCategory] [nvarchar](max) NULL,
	[OpeningBalance] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ChartOfAccounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Accounts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionType] [nvarchar](max) NULL,
	[Category] [nvarchar](max) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[RelatedEntity] [nvarchar](max) NULL,
	[Date] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[AccountName] [nvarchar](max) NULL,
	[AccountType] [nvarchar](max) NULL,
	[Credit] [decimal](18, 2) NOT NULL,
	[Debit] [decimal](18, 2) NOT NULL,
	[IsPosted] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[PartyName] [nvarchar](max) NULL,
	[Reference] [nvarchar](max) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[VoucherNo] [nvarchar](max) NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AcademicYears]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AcademicYears](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[YearName] [nvarchar](20) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsCurrent] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_AcademicYears] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AcademicCalendarEvents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AcademicCalendarEvents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[EventType] [nvarchar](50) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AcademicCalendarEvents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Announcements]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Announcements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[TargetAudience] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[BatchNo] [nvarchar](max) NULL,
	[RoomNo] [nvarchar](max) NULL,
	[Subject] [nvarchar](max) NULL,
	[ClassDateTime] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CourseId] [int] NULL,
 CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BiometricDevices]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BiometricDevices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[DeviceCode] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[Model] [nvarchar](max) NULL,
	[SerialNumber] [nvarchar](max) NULL,
	[IpAddress] [nvarchar](max) NULL,
	[Port] [int] NOT NULL,
	[Location] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[Status] [nvarchar](max) NULL,
	[LastSyncAt] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_BiometricDevices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](450) NULL,
	[Role] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[Expertise] [nvarchar](max) NULL,
	[JoinDate] [datetime2](7) NULL,
	[Nid] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[ProfilePicture] [nvarchar](max) NULL,
	[Salary] [decimal](18, 2) NULL,
	[Status] [nvarchar](max) NULL,
	[Batch] [nvarchar](max) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[Department] [nvarchar](max) NULL,
	[EmergencyContact] [nvarchar](max) NULL,
	[EnrollmentDate] [datetime2](7) NULL,
	[FeeStatus] [nvarchar](max) NULL,
	[Gender] [nvarchar](max) NULL,
	[GuardianName] [nvarchar](max) NULL,
	[Nationality] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Semester] [nvarchar](max) NULL,
	[StudentId] [nvarchar](max) NULL,
	[Experience] [int] NULL,
	[Qualification] [nvarchar](max) NULL,
	[TeacherId] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[BloodGroup] [nvarchar](max) NULL,
	[Designation] [nvarchar](max) NULL,
	[EmployeeType] [nvarchar](max) NULL,
	[EmploymentStatus] [nvarchar](max) NULL,
	[DeviceUserId] [nvarchar](max) NULL,
	[PassportNumber] [nvarchar](max) NULL,
	[VisaExpiryDate] [datetime2](7) NULL,
	[VisaType] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UniversityProfiles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UniversityProfiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[ShortName] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Website] [nvarchar](max) NULL,
	[LogoUrl] [nvarchar](max) NULL,
	[EstablishedYear] [nvarchar](max) NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_UniversityProfiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Suppliers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Suppliers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[CompanyName] [nvarchar](max) NULL,
	[TotalDue] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Payables]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Payables](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PayableType] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[PartyName] [nvarchar](max) NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[AmountPaid] [decimal](18, 2) NOT NULL,
	[OutstandingAmount] [decimal](18, 2) NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[InvoiceDate] [datetime2](7) NOT NULL,
	[Status] [nvarchar](max) NULL,
	[InvoiceNumber] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Reference] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Payables] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayablePayments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayablePayments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PayableId] [int] NOT NULL,
	[AmountPaid] [decimal](18, 2) NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_PayablePayments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StudentRequests]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StudentRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[RequestType] [nvarchar](max) NULL,
	[Subject] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[AdminRemarks] [nvarchar](max) NULL,
	[ResolvedByUserId] [int] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ResolvedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_StudentRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BiometricCredentials]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BiometricCredentials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CredentialId] [nvarchar](max) NULL,
	[PublicKey] [nvarchar](max) NULL,
	[UserHandle] [nvarchar](max) NULL,
	[SignatureCounter] [bigint] NOT NULL,
	[CredType] [nvarchar](max) NULL,
	[AaGuid] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_BiometricCredentials] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Alumni]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Alumni](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NULL,
	[FullName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Department] [nvarchar](max) NULL,
	[Batch] [nvarchar](max) NULL,
	[GraduationYear] [int] NULL,
	[CurrentJobTitle] [nvarchar](max) NULL,
	[CurrentCompany] [nvarchar](max) NULL,
	[LinkedInUrl] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[IsAutoGenerated] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Alumni] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CertificateRequests]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CertificateRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[CertificateType] [nvarchar](max) NULL,
	[Purpose] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[AdminRemarks] [nvarchar](max) NULL,
	[ApprovedByUserId] [int] NULL,
	[ApprovedById] [int] NULL,
	[RequestedAt] [datetime2](7) NOT NULL,
	[IssuedAt] [datetime2](7) NULL,
	[CertificateNumber] [nvarchar](max) NULL,
 CONSTRAINT [PK_CertificateRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolePermissions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RolePermissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[RoleName] [nvarchar](max) NULL,
	[PermissionId] [int] NOT NULL,
 CONSTRAINT [PK_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Purchases]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Purchases](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierId] [int] NOT NULL,
	[PurchaseDate] [datetime2](7) NOT NULL,
	[InvoiceNo] [nvarchar](max) NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[DueAmount] [decimal](18, 2) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Purchases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staff]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Staff](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[StaffId] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Gender] [nvarchar](max) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[Address] [nvarchar](max) NULL,
	[Nid] [nvarchar](max) NULL,
	[Designation] [nvarchar](max) NULL,
	[Department] [nvarchar](max) NULL,
	[JoinDate] [datetime2](7) NOT NULL,
	[Salary] [decimal](18, 2) NOT NULL,
	[Status] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[EmergencyContact] [nvarchar](max) NULL,
	[Qualification] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Semesters]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Semesters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[AcademicYearId] [int] NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsCurrent] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Semesters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Sales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[InvoiceNo] [nvarchar](max) NULL,
	[SaleDate] [datetime2](7) NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[DueAmount] [decimal](18, 2) NOT NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FounderTransactions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FounderTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FounderAccountId] [int] NOT NULL,
	[TransactionType] [nvarchar](30) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TransactionDate] [datetime2](7) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Reference] [nvarchar](100) NULL,
	[PaymentMethod] [nvarchar](30) NULL,
	[Classification] [nvarchar](20) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_FounderTransactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FounderLoans]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FounderLoans](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FounderAccountId] [int] NOT NULL,
	[LoanType] [nvarchar](30) NOT NULL,
	[LoanTitle] [nvarchar](200) NOT NULL,
	[PrincipalAmount] [decimal](18, 2) NOT NULL,
	[InterestRate] [decimal](5, 2) NOT NULL,
	[AmountRepaid] [decimal](18, 2) NOT NULL,
	[OutstandingBalance] [decimal](18, 2) NOT NULL,
	[LoanDate] [datetime2](7) NOT NULL,
	[DueDate] [datetime2](7) NULL,
	[Status] [nvarchar](20) NULL,
	[Description] [nvarchar](500) NULL,
	[Reference] [nvarchar](100) NULL,
	[PaymentMethod] [nvarchar](30) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_FounderLoans] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DeviceUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BiometricDeviceId] [int] NOT NULL,
	[UserId] [int] NULL,
	[DeviceUserId] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NULL,
	[CredentialType] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[RegisteredAt] [datetime2](7) NOT NULL,
	[LastSyncedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_DeviceUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceLogs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DeviceLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BiometricDeviceId] [int] NOT NULL,
	[UserId] [int] NULL,
	[DeviceUserId] [nvarchar](max) NULL,
	[EventType] [nvarchar](max) NULL,
	[EventDateTime] [datetime2](7) NOT NULL,
	[Status] [nvarchar](max) NULL,
	[IsSynced] [bit] NOT NULL,
	[SyncedAt] [datetime2](7) NULL,
	[RawData] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DeviceLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Fees]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Fees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[FeeType] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[PaidDate] [datetime2](7) NULL,
	[Note] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Fees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[JournalEntries]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[JournalEntries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceNumber] [nvarchar](30) NOT NULL,
	[TransactionDate] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[Status] [nvarchar](max) NULL,
	[TotalDebit] [decimal](18, 2) NOT NULL,
	[TotalCredit] [decimal](18, 2) NOT NULL,
	[CreatedByUserId] [int] NULL,
 CONSTRAINT [PK_JournalEntries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoanRepayments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LoanRepayments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoanId] [int] NOT NULL,
	[PrincipalPaid] [decimal](18, 2) NOT NULL,
	[InterestPaid] [decimal](18, 2) NOT NULL,
	[TotalPaid] [decimal](18, 2) NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_LoanRepayments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LeaveRequests]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LeaveRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StaffId] [int] NOT NULL,
	[LeaveType] [nvarchar](max) NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[AppliedDate] [datetime2](7) NOT NULL,
	[ApprovedBy] [nvarchar](max) NULL,
	[ApprovedDate] [datetime2](7) NULL,
	[Remarks] [nvarchar](max) NULL,
 CONSTRAINT [PK_LeaveRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[JournalLines]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[JournalLines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JournalEntryId] [int] NOT NULL,
	[Debit] [decimal](18, 2) NOT NULL,
	[Credit] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ChartOfAccountId] [int] NOT NULL,
 CONSTRAINT [PK_JournalLines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaleItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SaleItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SaleId] [int] NOT NULL,
	[ItemName] [nvarchar](max) NULL,
	[Unit] [nvarchar](max) NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_SaleItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PurchaseItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PurchaseItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseId] [int] NOT NULL,
	[ItemName] [nvarchar](max) NULL,
	[Unit] [nvarchar](max) NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_PurchaseItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Payrolls]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Payrolls](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StaffId] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[BasicSalary] [decimal](18, 2) NOT NULL,
	[Allowances] [decimal](18, 2) NOT NULL,
	[Deductions] [decimal](18, 2) NOT NULL,
	[NetSalary] [decimal](18, 2) NOT NULL,
	[PaymentDate] [datetime2](7) NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Payrolls] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Courses]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Courses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[InstructorId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[AllowInstallment] [bit] NOT NULL,
	[CourseCode] [nvarchar](450) NOT NULL,
	[Credits] [int] NOT NULL,
	[Duration] [int] NOT NULL,
	[Fee] [decimal](18, 2) NOT NULL,
	[NumberOfInstallments] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NOT NULL,
	[SemesterId] [int] NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffAttendances]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StaffAttendances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StaffId] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Status] [nvarchar](max) NULL,
	[CheckInTime] [time](7) NULL,
	[CheckOutTime] [time](7) NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_StaffAttendances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Enrollments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Enrollments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[EnrollmentDate] [datetime2](7) NOT NULL,
	[InstallmentCount] [int] NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[PaymentStatus] [nvarchar](max) NULL,
	[TotalFee] [decimal](18, 2) NOT NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[SemesterId] [int] NULL,
 CONSTRAINT [PK_Enrollments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Assignments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Assignments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[TotalMarks] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CourseId] [int] NOT NULL,
	[TeacherId] [int] NOT NULL,
	[StudentId] [int] NULL,
 CONSTRAINT [PK_Assignments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Attendances]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Attendances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CourseId] [int] NULL,
	[Date] [datetime2](7) NOT NULL,
	[Role] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Source] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Attendances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ClassSchedules]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ClassSchedules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NOT NULL,
	[Batch] [nvarchar](max) NULL,
	[RoomNo] [nvarchar](max) NULL,
	[ClassDateTime] [datetime2](7) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ClassSchedules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Exams]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Exams](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[CourseId] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[TotalMarks] [int] NOT NULL,
	[ExamType] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Exams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Payments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Payments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[DownPayment] [decimal](18, 2) NOT NULL,
	[EnrollmentFee] [decimal](18, 2) NOT NULL,
	[Status] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Installments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Installments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentId] [int] NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[PaidDate] [datetime2](7) NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Installments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExamResults]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ExamResults](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExamId] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
	[MarksObtained] [decimal](18, 2) NOT NULL,
	[Percentage] [decimal](5, 2) NOT NULL,
	[Grade] [nvarchar](max) NULL,
	[GradePoint] [decimal](5, 2) NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ExamResults] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AssignmentSubmissions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AssignmentSubmissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssignmentId] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
	[SubmissionUrl] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[MarksObtained] [int] NULL,
	[Feedback] [nvarchar](max) NULL,
	[SubmittedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AssignmentSubmissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ChartOfAccounts]') AND name = N'IX_ChartOfAccounts_AccountCode')
CREATE UNIQUE NONCLUSTERED INDEX [IX_ChartOfAccounts_AccountCode] ON [dbo].[ChartOfAccounts]
(
	[AccountCode] ASC
)
WHERE ([AccountCode] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'IX_Users_Email')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users]
(
	[Email] ASC
)
WHERE ([Email] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PayablePayments]') AND name = N'IX_PayablePayments_PayableId')
CREATE NONCLUSTERED INDEX [IX_PayablePayments_PayableId] ON [dbo].[PayablePayments]
(
	[PayableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StudentRequests]') AND name = N'IX_StudentRequests_ResolvedByUserId')
CREATE NONCLUSTERED INDEX [IX_StudentRequests_ResolvedByUserId] ON [dbo].[StudentRequests]
(
	[ResolvedByUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StudentRequests]') AND name = N'IX_StudentRequests_StudentId')
CREATE NONCLUSTERED INDEX [IX_StudentRequests_StudentId] ON [dbo].[StudentRequests]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BiometricCredentials]') AND name = N'IX_BiometricCredentials_UserId')
CREATE NONCLUSTERED INDEX [IX_BiometricCredentials_UserId] ON [dbo].[BiometricCredentials]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Alumni]') AND name = N'IX_Alumni_StudentId')
CREATE NONCLUSTERED INDEX [IX_Alumni_StudentId] ON [dbo].[Alumni]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CertificateRequests]') AND name = N'IX_CertificateRequests_ApprovedById')
CREATE NONCLUSTERED INDEX [IX_CertificateRequests_ApprovedById] ON [dbo].[CertificateRequests]
(
	[ApprovedById] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CertificateRequests]') AND name = N'IX_CertificateRequests_StudentId')
CREATE NONCLUSTERED INDEX [IX_CertificateRequests_StudentId] ON [dbo].[CertificateRequests]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RolePermissions]') AND name = N'IX_RolePermissions_PermissionId')
CREATE NONCLUSTERED INDEX [IX_RolePermissions_PermissionId] ON [dbo].[RolePermissions]
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Purchases]') AND name = N'IX_Purchases_SupplierId')
CREATE NONCLUSTERED INDEX [IX_Purchases_SupplierId] ON [dbo].[Purchases]
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Staff]') AND name = N'IX_Staff_UserId')
CREATE NONCLUSTERED INDEX [IX_Staff_UserId] ON [dbo].[Staff]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Semesters]') AND name = N'IX_Semesters_AcademicYearId')
CREATE NONCLUSTERED INDEX [IX_Semesters_AcademicYearId] ON [dbo].[Semesters]
(
	[AcademicYearId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND name = N'IX_Sales_CustomerId')
CREATE NONCLUSTERED INDEX [IX_Sales_CustomerId] ON [dbo].[Sales]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FounderTransactions]') AND name = N'IX_FounderTransactions_FounderAccountId')
CREATE NONCLUSTERED INDEX [IX_FounderTransactions_FounderAccountId] ON [dbo].[FounderTransactions]
(
	[FounderAccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FounderLoans]') AND name = N'IX_FounderLoans_FounderAccountId')
CREATE NONCLUSTERED INDEX [IX_FounderLoans_FounderAccountId] ON [dbo].[FounderLoans]
(
	[FounderAccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DeviceUsers]') AND name = N'IX_DeviceUsers_BiometricDeviceId')
CREATE NONCLUSTERED INDEX [IX_DeviceUsers_BiometricDeviceId] ON [dbo].[DeviceUsers]
(
	[BiometricDeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DeviceLogs]') AND name = N'IX_DeviceLogs_BiometricDeviceId')
CREATE NONCLUSTERED INDEX [IX_DeviceLogs_BiometricDeviceId] ON [dbo].[DeviceLogs]
(
	[BiometricDeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Fees]') AND name = N'IX_Fees_StudentId')
CREATE NONCLUSTERED INDEX [IX_Fees_StudentId] ON [dbo].[Fees]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[JournalEntries]') AND name = N'IX_JournalEntries_ReferenceNumber')
CREATE UNIQUE NONCLUSTERED INDEX [IX_JournalEntries_ReferenceNumber] ON [dbo].[JournalEntries]
(
	[ReferenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LoanRepayments]') AND name = N'IX_LoanRepayments_LoanId')
CREATE NONCLUSTERED INDEX [IX_LoanRepayments_LoanId] ON [dbo].[LoanRepayments]
(
	[LoanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LeaveRequests]') AND name = N'IX_LeaveRequests_StaffId')
CREATE NONCLUSTERED INDEX [IX_LeaveRequests_StaffId] ON [dbo].[LeaveRequests]
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[JournalLines]') AND name = N'IX_JournalLines_ChartOfAccountId')
CREATE NONCLUSTERED INDEX [IX_JournalLines_ChartOfAccountId] ON [dbo].[JournalLines]
(
	[ChartOfAccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[JournalLines]') AND name = N'IX_JournalLines_JournalEntryId')
CREATE NONCLUSTERED INDEX [IX_JournalLines_JournalEntryId] ON [dbo].[JournalLines]
(
	[JournalEntryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SaleItems]') AND name = N'IX_SaleItems_SaleId')
CREATE NONCLUSTERED INDEX [IX_SaleItems_SaleId] ON [dbo].[SaleItems]
(
	[SaleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PurchaseItems]') AND name = N'IX_PurchaseItems_PurchaseId')
CREATE NONCLUSTERED INDEX [IX_PurchaseItems_PurchaseId] ON [dbo].[PurchaseItems]
(
	[PurchaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Payrolls]') AND name = N'IX_Payrolls_StaffId')
CREATE NONCLUSTERED INDEX [IX_Payrolls_StaffId] ON [dbo].[Payrolls]
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Courses]') AND name = N'IX_Courses_CategoryId')
CREATE NONCLUSTERED INDEX [IX_Courses_CategoryId] ON [dbo].[Courses]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Courses]') AND name = N'IX_Courses_CourseCode')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Courses_CourseCode] ON [dbo].[Courses]
(
	[CourseCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Courses]') AND name = N'IX_Courses_InstructorId')
CREATE NONCLUSTERED INDEX [IX_Courses_InstructorId] ON [dbo].[Courses]
(
	[InstructorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Courses]') AND name = N'IX_Courses_SemesterId')
CREATE NONCLUSTERED INDEX [IX_Courses_SemesterId] ON [dbo].[Courses]
(
	[SemesterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StaffAttendances]') AND name = N'IX_StaffAttendances_StaffId')
CREATE NONCLUSTERED INDEX [IX_StaffAttendances_StaffId] ON [dbo].[StaffAttendances]
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Enrollments]') AND name = N'IX_Enrollments_CourseId')
CREATE NONCLUSTERED INDEX [IX_Enrollments_CourseId] ON [dbo].[Enrollments]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Enrollments]') AND name = N'IX_Enrollments_SemesterId')
CREATE NONCLUSTERED INDEX [IX_Enrollments_SemesterId] ON [dbo].[Enrollments]
(
	[SemesterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Enrollments]') AND name = N'IX_Enrollments_StudentId')
CREATE NONCLUSTERED INDEX [IX_Enrollments_StudentId] ON [dbo].[Enrollments]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Assignments]') AND name = N'IX_Assignments_CourseId')
CREATE NONCLUSTERED INDEX [IX_Assignments_CourseId] ON [dbo].[Assignments]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Assignments]') AND name = N'IX_Assignments_StudentId')
CREATE NONCLUSTERED INDEX [IX_Assignments_StudentId] ON [dbo].[Assignments]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Assignments]') AND name = N'IX_Assignments_TeacherId')
CREATE NONCLUSTERED INDEX [IX_Assignments_TeacherId] ON [dbo].[Assignments]
(
	[TeacherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Attendances]') AND name = N'IX_Attendances_CourseId')
CREATE NONCLUSTERED INDEX [IX_Attendances_CourseId] ON [dbo].[Attendances]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Attendances]') AND name = N'IX_Attendances_UserId')
CREATE NONCLUSTERED INDEX [IX_Attendances_UserId] ON [dbo].[Attendances]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ClassSchedules]') AND name = N'IX_ClassSchedules_CourseId')
CREATE NONCLUSTERED INDEX [IX_ClassSchedules_CourseId] ON [dbo].[ClassSchedules]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Exams]') AND name = N'IX_Exams_CourseId')
CREATE NONCLUSTERED INDEX [IX_Exams_CourseId] ON [dbo].[Exams]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Payments]') AND name = N'IX_Payments_CourseId')
CREATE NONCLUSTERED INDEX [IX_Payments_CourseId] ON [dbo].[Payments]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Payments]') AND name = N'IX_Payments_StudentId')
CREATE NONCLUSTERED INDEX [IX_Payments_StudentId] ON [dbo].[Payments]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Installments]') AND name = N'IX_Installments_PaymentId')
CREATE NONCLUSTERED INDEX [IX_Installments_PaymentId] ON [dbo].[Installments]
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ExamResults]') AND name = N'IX_ExamResults_ExamId')
CREATE NONCLUSTERED INDEX [IX_ExamResults_ExamId] ON [dbo].[ExamResults]
(
	[ExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ExamResults]') AND name = N'IX_ExamResults_StudentId')
CREATE NONCLUSTERED INDEX [IX_ExamResults_StudentId] ON [dbo].[ExamResults]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AssignmentSubmissions]') AND name = N'IX_AssignmentSubmissions_AssignmentId')
CREATE NONCLUSTERED INDEX [IX_AssignmentSubmissions_AssignmentId] ON [dbo].[AssignmentSubmissions]
(
	[AssignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AssignmentSubmissions]') AND name = N'IX_AssignmentSubmissions_StudentId')
CREATE NONCLUSTERED INDEX [IX_AssignmentSubmissions_StudentId] ON [dbo].[AssignmentSubmissions]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__FounderAc__Share__55DFB4D9]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[FounderAccounts] ADD  DEFAULT ((0.0)) FOR [SharePercentage]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Accounts__Credit__1F2E9E6D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0.0)) FOR [Credit]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Accounts__Debit__2022C2A6]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0.0)) FOR [Debit]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Accounts__IsPost__2116E6DF]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsPosted]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__JournalEn__Total__0504B816]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[JournalEntries] ADD  DEFAULT ((0)) FOR [TotalDebit]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__JournalEn__Total__05F8DC4F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[JournalEntries] ADD  DEFAULT ((0)) FOR [TotalCredit]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Courses__Descrip__00200768]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Courses] ADD  DEFAULT (N'') FOR [Description]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Courses__Categor__6A30C649]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Courses] ADD  DEFAULT ((0)) FOR [CategoryId]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Courses__AllowIn__6E01572D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Courses] ADD  DEFAULT (CONVERT([bit],(0))) FOR [AllowInstallment]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Courses__Credits__01142BA1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Courses] ADD  DEFAULT ((0)) FOR [Credits]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Courses__Duratio__7F2BE32F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Courses] ADD  DEFAULT ((0)) FOR [Duration]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Courses__Fee__6EF57B66]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Courses] ADD  DEFAULT ((0.0)) FOR [Fee]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Courses__NumberO__6FE99F9F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Courses] ADD  DEFAULT ((0)) FOR [NumberOfInstallments]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Courses__Created__02FC7413]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Courses] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [CreatedAt]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Courses__Status__19DFD96B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Courses] ADD  DEFAULT (N'') FOR [Status]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Enrollmen__Insta__05D8E0BE]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Enrollments] ADD  DEFAULT ((0)) FOR [InstallmentCount]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Enrollmen__PaidA__06CD04F7]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Enrollments] ADD  DEFAULT ((0.0)) FOR [PaidAmount]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Enrollmen__Total__07C12930]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Enrollments] ADD  DEFAULT ((0.0)) FOR [TotalFee]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Installme__Statu__2739D489]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Installments] ADD  DEFAULT ('Pending') FOR [Status]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PayablePayments_Payables_PayableId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PayablePayments]'))
ALTER TABLE [dbo].[PayablePayments]  WITH CHECK ADD  CONSTRAINT [FK_PayablePayments_Payables_PayableId] FOREIGN KEY([PayableId])
REFERENCES [dbo].[Payables] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PayablePayments_Payables_PayableId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PayablePayments]'))
ALTER TABLE [dbo].[PayablePayments] CHECK CONSTRAINT [FK_PayablePayments_Payables_PayableId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudentRequests_Users_ResolvedByUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudentRequests]'))
ALTER TABLE [dbo].[StudentRequests]  WITH CHECK ADD  CONSTRAINT [FK_StudentRequests_Users_ResolvedByUserId] FOREIGN KEY([ResolvedByUserId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudentRequests_Users_ResolvedByUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudentRequests]'))
ALTER TABLE [dbo].[StudentRequests] CHECK CONSTRAINT [FK_StudentRequests_Users_ResolvedByUserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudentRequests_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudentRequests]'))
ALTER TABLE [dbo].[StudentRequests]  WITH CHECK ADD  CONSTRAINT [FK_StudentRequests_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudentRequests_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudentRequests]'))
ALTER TABLE [dbo].[StudentRequests] CHECK CONSTRAINT [FK_StudentRequests_Users_StudentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BiometricCredentials_Users_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[BiometricCredentials]'))
ALTER TABLE [dbo].[BiometricCredentials]  WITH CHECK ADD  CONSTRAINT [FK_BiometricCredentials_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BiometricCredentials_Users_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[BiometricCredentials]'))
ALTER TABLE [dbo].[BiometricCredentials] CHECK CONSTRAINT [FK_BiometricCredentials_Users_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Alumni_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Alumni]'))
ALTER TABLE [dbo].[Alumni]  WITH CHECK ADD  CONSTRAINT [FK_Alumni_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Alumni_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Alumni]'))
ALTER TABLE [dbo].[Alumni] CHECK CONSTRAINT [FK_Alumni_Users_StudentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CertificateRequests_Users_ApprovedById]') AND parent_object_id = OBJECT_ID(N'[dbo].[CertificateRequests]'))
ALTER TABLE [dbo].[CertificateRequests]  WITH CHECK ADD  CONSTRAINT [FK_CertificateRequests_Users_ApprovedById] FOREIGN KEY([ApprovedById])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CertificateRequests_Users_ApprovedById]') AND parent_object_id = OBJECT_ID(N'[dbo].[CertificateRequests]'))
ALTER TABLE [dbo].[CertificateRequests] CHECK CONSTRAINT [FK_CertificateRequests_Users_ApprovedById]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CertificateRequests_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[CertificateRequests]'))
ALTER TABLE [dbo].[CertificateRequests]  WITH CHECK ADD  CONSTRAINT [FK_CertificateRequests_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CertificateRequests_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[CertificateRequests]'))
ALTER TABLE [dbo].[CertificateRequests] CHECK CONSTRAINT [FK_CertificateRequests_Users_StudentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolePermissions_Permissions_PermissionId]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolePermissions]'))
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permissions] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolePermissions_Permissions_PermissionId]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolePermissions]'))
ALTER TABLE [dbo].[RolePermissions] CHECK CONSTRAINT [FK_RolePermissions_Permissions_PermissionId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Purchases_Suppliers_SupplierId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Purchases]'))
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [FK_Purchases_Suppliers_SupplierId] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Suppliers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Purchases_Suppliers_SupplierId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Purchases]'))
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [FK_Purchases_Suppliers_SupplierId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Staff_Users_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Staff]'))
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Staff_Users_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Staff]'))
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Users_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Semesters_AcademicYears_AcademicYearId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Semesters]'))
ALTER TABLE [dbo].[Semesters]  WITH CHECK ADD  CONSTRAINT [FK_Semesters_AcademicYears_AcademicYearId] FOREIGN KEY([AcademicYearId])
REFERENCES [dbo].[AcademicYears] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Semesters_AcademicYears_AcademicYearId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Semesters]'))
ALTER TABLE [dbo].[Semesters] CHECK CONSTRAINT [FK_Semesters_AcademicYears_AcademicYearId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_Customers_CustomerId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales]'))
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Customers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_Customers_CustomerId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales]'))
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Customers_CustomerId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FounderTransactions_FounderAccounts_FounderAccountId]') AND parent_object_id = OBJECT_ID(N'[dbo].[FounderTransactions]'))
ALTER TABLE [dbo].[FounderTransactions]  WITH CHECK ADD  CONSTRAINT [FK_FounderTransactions_FounderAccounts_FounderAccountId] FOREIGN KEY([FounderAccountId])
REFERENCES [dbo].[FounderAccounts] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FounderTransactions_FounderAccounts_FounderAccountId]') AND parent_object_id = OBJECT_ID(N'[dbo].[FounderTransactions]'))
ALTER TABLE [dbo].[FounderTransactions] CHECK CONSTRAINT [FK_FounderTransactions_FounderAccounts_FounderAccountId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FounderLoans_FounderAccounts_FounderAccountId]') AND parent_object_id = OBJECT_ID(N'[dbo].[FounderLoans]'))
ALTER TABLE [dbo].[FounderLoans]  WITH CHECK ADD  CONSTRAINT [FK_FounderLoans_FounderAccounts_FounderAccountId] FOREIGN KEY([FounderAccountId])
REFERENCES [dbo].[FounderAccounts] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FounderLoans_FounderAccounts_FounderAccountId]') AND parent_object_id = OBJECT_ID(N'[dbo].[FounderLoans]'))
ALTER TABLE [dbo].[FounderLoans] CHECK CONSTRAINT [FK_FounderLoans_FounderAccounts_FounderAccountId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceUsers_BiometricDevices_BiometricDeviceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceUsers]'))
ALTER TABLE [dbo].[DeviceUsers]  WITH CHECK ADD  CONSTRAINT [FK_DeviceUsers_BiometricDevices_BiometricDeviceId] FOREIGN KEY([BiometricDeviceId])
REFERENCES [dbo].[BiometricDevices] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceUsers_BiometricDevices_BiometricDeviceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceUsers]'))
ALTER TABLE [dbo].[DeviceUsers] CHECK CONSTRAINT [FK_DeviceUsers_BiometricDevices_BiometricDeviceId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceLogs_BiometricDevices_BiometricDeviceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceLogs]'))
ALTER TABLE [dbo].[DeviceLogs]  WITH CHECK ADD  CONSTRAINT [FK_DeviceLogs_BiometricDevices_BiometricDeviceId] FOREIGN KEY([BiometricDeviceId])
REFERENCES [dbo].[BiometricDevices] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceLogs_BiometricDevices_BiometricDeviceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceLogs]'))
ALTER TABLE [dbo].[DeviceLogs] CHECK CONSTRAINT [FK_DeviceLogs_BiometricDevices_BiometricDeviceId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fees_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Fees]'))
ALTER TABLE [dbo].[Fees]  WITH CHECK ADD  CONSTRAINT [FK_Fees_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fees_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Fees]'))
ALTER TABLE [dbo].[Fees] CHECK CONSTRAINT [FK_Fees_Users_StudentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JournalEntries_Users_CreatedByUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[JournalEntries]'))
ALTER TABLE [dbo].[JournalEntries]  WITH CHECK ADD  CONSTRAINT [FK_JournalEntries_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JournalEntries_Users_CreatedByUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[JournalEntries]'))
ALTER TABLE [dbo].[JournalEntries] CHECK CONSTRAINT [FK_JournalEntries_Users_CreatedByUserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LoanRepayments_Loans_LoanId]') AND parent_object_id = OBJECT_ID(N'[dbo].[LoanRepayments]'))
ALTER TABLE [dbo].[LoanRepayments]  WITH CHECK ADD  CONSTRAINT [FK_LoanRepayments_Loans_LoanId] FOREIGN KEY([LoanId])
REFERENCES [dbo].[Loans] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LoanRepayments_Loans_LoanId]') AND parent_object_id = OBJECT_ID(N'[dbo].[LoanRepayments]'))
ALTER TABLE [dbo].[LoanRepayments] CHECK CONSTRAINT [FK_LoanRepayments_Loans_LoanId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LeaveRequests_Staff_StaffId]') AND parent_object_id = OBJECT_ID(N'[dbo].[LeaveRequests]'))
ALTER TABLE [dbo].[LeaveRequests]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequests_Staff_StaffId] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LeaveRequests_Staff_StaffId]') AND parent_object_id = OBJECT_ID(N'[dbo].[LeaveRequests]'))
ALTER TABLE [dbo].[LeaveRequests] CHECK CONSTRAINT [FK_LeaveRequests_Staff_StaffId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JournalLines_ChartOfAccounts_ChartOfAccountId]') AND parent_object_id = OBJECT_ID(N'[dbo].[JournalLines]'))
ALTER TABLE [dbo].[JournalLines]  WITH CHECK ADD  CONSTRAINT [FK_JournalLines_ChartOfAccounts_ChartOfAccountId] FOREIGN KEY([ChartOfAccountId])
REFERENCES [dbo].[ChartOfAccounts] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JournalLines_ChartOfAccounts_ChartOfAccountId]') AND parent_object_id = OBJECT_ID(N'[dbo].[JournalLines]'))
ALTER TABLE [dbo].[JournalLines] CHECK CONSTRAINT [FK_JournalLines_ChartOfAccounts_ChartOfAccountId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JournalLines_JournalEntries_JournalEntryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[JournalLines]'))
ALTER TABLE [dbo].[JournalLines]  WITH CHECK ADD  CONSTRAINT [FK_JournalLines_JournalEntries_JournalEntryId] FOREIGN KEY([JournalEntryId])
REFERENCES [dbo].[JournalEntries] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JournalLines_JournalEntries_JournalEntryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[JournalLines]'))
ALTER TABLE [dbo].[JournalLines] CHECK CONSTRAINT [FK_JournalLines_JournalEntries_JournalEntryId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SaleItems_Sales_SaleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[SaleItems]'))
ALTER TABLE [dbo].[SaleItems]  WITH CHECK ADD  CONSTRAINT [FK_SaleItems_Sales_SaleId] FOREIGN KEY([SaleId])
REFERENCES [dbo].[Sales] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SaleItems_Sales_SaleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[SaleItems]'))
ALTER TABLE [dbo].[SaleItems] CHECK CONSTRAINT [FK_SaleItems_Sales_SaleId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PurchaseItems_Purchases_PurchaseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PurchaseItems]'))
ALTER TABLE [dbo].[PurchaseItems]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseItems_Purchases_PurchaseId] FOREIGN KEY([PurchaseId])
REFERENCES [dbo].[Purchases] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PurchaseItems_Purchases_PurchaseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PurchaseItems]'))
ALTER TABLE [dbo].[PurchaseItems] CHECK CONSTRAINT [FK_PurchaseItems_Purchases_PurchaseId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payrolls_Staff_StaffId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payrolls]'))
ALTER TABLE [dbo].[Payrolls]  WITH CHECK ADD  CONSTRAINT [FK_Payrolls_Staff_StaffId] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payrolls_Staff_StaffId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payrolls]'))
ALTER TABLE [dbo].[Payrolls] CHECK CONSTRAINT [FK_Payrolls_Staff_StaffId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Courses_Categories_CategoryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Courses]'))
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Courses_Categories_CategoryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Courses]'))
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_Categories_CategoryId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Courses_Semesters_SemesterId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Courses]'))
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_Semesters_SemesterId] FOREIGN KEY([SemesterId])
REFERENCES [dbo].[Semesters] ([Id])
ON DELETE SET NULL
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Courses_Semesters_SemesterId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Courses]'))
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_Semesters_SemesterId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Courses_Users_InstructorId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Courses]'))
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_Users_InstructorId] FOREIGN KEY([InstructorId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Courses_Users_InstructorId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Courses]'))
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_Users_InstructorId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StaffAttendances_Staff_StaffId]') AND parent_object_id = OBJECT_ID(N'[dbo].[StaffAttendances]'))
ALTER TABLE [dbo].[StaffAttendances]  WITH CHECK ADD  CONSTRAINT [FK_StaffAttendances_Staff_StaffId] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StaffAttendances_Staff_StaffId]') AND parent_object_id = OBJECT_ID(N'[dbo].[StaffAttendances]'))
ALTER TABLE [dbo].[StaffAttendances] CHECK CONSTRAINT [FK_StaffAttendances_Staff_StaffId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Enrollments_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Enrollments]'))
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Enrollments_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Enrollments]'))
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Courses_CourseId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Enrollments_Semesters_SemesterId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Enrollments]'))
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Semesters_SemesterId] FOREIGN KEY([SemesterId])
REFERENCES [dbo].[Semesters] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Enrollments_Semesters_SemesterId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Enrollments]'))
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Semesters_SemesterId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Enrollments_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Enrollments]'))
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Enrollments_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Enrollments]'))
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Users_StudentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Assignments_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Assignments]'))
ALTER TABLE [dbo].[Assignments]  WITH CHECK ADD  CONSTRAINT [FK_Assignments_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Assignments_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Assignments]'))
ALTER TABLE [dbo].[Assignments] CHECK CONSTRAINT [FK_Assignments_Courses_CourseId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Assignments_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Assignments]'))
ALTER TABLE [dbo].[Assignments]  WITH CHECK ADD  CONSTRAINT [FK_Assignments_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Assignments_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Assignments]'))
ALTER TABLE [dbo].[Assignments] CHECK CONSTRAINT [FK_Assignments_Users_StudentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Assignments_Users_TeacherId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Assignments]'))
ALTER TABLE [dbo].[Assignments]  WITH CHECK ADD  CONSTRAINT [FK_Assignments_Users_TeacherId] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Assignments_Users_TeacherId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Assignments]'))
ALTER TABLE [dbo].[Assignments] CHECK CONSTRAINT [FK_Assignments_Users_TeacherId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attendances_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attendances]'))
ALTER TABLE [dbo].[Attendances]  WITH CHECK ADD  CONSTRAINT [FK_Attendances_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attendances_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attendances]'))
ALTER TABLE [dbo].[Attendances] CHECK CONSTRAINT [FK_Attendances_Courses_CourseId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attendances_Users_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attendances]'))
ALTER TABLE [dbo].[Attendances]  WITH CHECK ADD  CONSTRAINT [FK_Attendances_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attendances_Users_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attendances]'))
ALTER TABLE [dbo].[Attendances] CHECK CONSTRAINT [FK_Attendances_Users_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ClassSchedules_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ClassSchedules]'))
ALTER TABLE [dbo].[ClassSchedules]  WITH CHECK ADD  CONSTRAINT [FK_ClassSchedules_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ClassSchedules_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ClassSchedules]'))
ALTER TABLE [dbo].[ClassSchedules] CHECK CONSTRAINT [FK_ClassSchedules_Courses_CourseId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Exams_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Exams]'))
ALTER TABLE [dbo].[Exams]  WITH CHECK ADD  CONSTRAINT [FK_Exams_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Exams_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Exams]'))
ALTER TABLE [dbo].[Exams] CHECK CONSTRAINT [FK_Exams_Courses_CourseId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payments_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payments]'))
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payments_Courses_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payments]'))
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Courses_CourseId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payments_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payments]'))
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payments_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payments]'))
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Users_StudentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Installments_Payments_PaymentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Installments]'))
ALTER TABLE [dbo].[Installments]  WITH CHECK ADD  CONSTRAINT [FK_Installments_Payments_PaymentId] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payments] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Installments_Payments_PaymentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Installments]'))
ALTER TABLE [dbo].[Installments] CHECK CONSTRAINT [FK_Installments_Payments_PaymentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExamResults_Exams_ExamId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExamResults]'))
ALTER TABLE [dbo].[ExamResults]  WITH CHECK ADD  CONSTRAINT [FK_ExamResults_Exams_ExamId] FOREIGN KEY([ExamId])
REFERENCES [dbo].[Exams] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExamResults_Exams_ExamId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExamResults]'))
ALTER TABLE [dbo].[ExamResults] CHECK CONSTRAINT [FK_ExamResults_Exams_ExamId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExamResults_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExamResults]'))
ALTER TABLE [dbo].[ExamResults]  WITH CHECK ADD  CONSTRAINT [FK_ExamResults_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExamResults_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExamResults]'))
ALTER TABLE [dbo].[ExamResults] CHECK CONSTRAINT [FK_ExamResults_Users_StudentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssignmentSubmissions_Assignments_AssignmentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssignmentSubmissions]'))
ALTER TABLE [dbo].[AssignmentSubmissions]  WITH CHECK ADD  CONSTRAINT [FK_AssignmentSubmissions_Assignments_AssignmentId] FOREIGN KEY([AssignmentId])
REFERENCES [dbo].[Assignments] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssignmentSubmissions_Assignments_AssignmentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssignmentSubmissions]'))
ALTER TABLE [dbo].[AssignmentSubmissions] CHECK CONSTRAINT [FK_AssignmentSubmissions_Assignments_AssignmentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssignmentSubmissions_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssignmentSubmissions]'))
ALTER TABLE [dbo].[AssignmentSubmissions]  WITH CHECK ADD  CONSTRAINT [FK_AssignmentSubmissions_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssignmentSubmissions_Users_StudentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssignmentSubmissions]'))
ALTER TABLE [dbo].[AssignmentSubmissions] CHECK CONSTRAINT [FK_AssignmentSubmissions_Users_StudentId]
GO
