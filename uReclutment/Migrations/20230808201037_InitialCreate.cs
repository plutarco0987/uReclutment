using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace uReclutment.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomersId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomersId);
                });

            migrationBuilder.CreateTable(
                name: "EnumType",
                columns: table => new
                {
                    EnumTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumType", x => x.EnumTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    FilesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidatesId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FilesId);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ErrorMessage = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    Error = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    ErrorDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingsId);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    StagesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.StagesId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    Password = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    Email = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Vacancy",
                columns: table => new
                {
                    VacancyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomersId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NamePosition = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Responsabilitys = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ContractType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Departament = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancy", x => x.VacancyId);
                    table.ForeignKey(
                        name: "FK_Vacancy_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "CustomersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    CandidatesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StagesId = table.Column<int>(type: "integer", nullable: false),
                    VacancyId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RecluterName = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: false),
                    ContactSource = table.Column<string>(type: "text", nullable: false),
                    RejectionEmcor = table.Column<string>(type: "text", nullable: false),
                    RejectionCandidate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.CandidatesId);
                    table.ForeignKey(
                        name: "FK_Candidates_Stages_StagesId",
                        column: x => x.StagesId,
                        principalTable: "Stages",
                        principalColumn: "StagesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidates_Vacancy_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancy",
                        principalColumn: "VacancyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnumTypeId = table.Column<int>(type: "integer", nullable: false),
                    VacancyId = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    MaxLength = table.Column<int>(type: "integer", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionsId);
                    table.ForeignKey(
                        name: "FK_Questions_EnumType_EnumTypeId",
                        column: x => x.EnumTypeId,
                        principalTable: "EnumType",
                        principalColumn: "EnumTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Vacancy_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancy",
                        principalColumn: "VacancyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requirements",
                columns: table => new
                {
                    RequirementsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VacancyId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    AgeExperience = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Benefits = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirements", x => x.RequirementsId);
                    table.ForeignKey(
                        name: "FK_Requirements_Vacancy_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancy",
                        principalColumn: "VacancyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    MeetingsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidatesId = table.Column<int>(type: "integer", nullable: false),
                    NumberMeeting = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.MeetingsId);
                    table.ForeignKey(
                        name: "FK_Meetings_Candidates_CandidatesId",
                        column: x => x.CandidatesId,
                        principalTable: "Candidates",
                        principalColumn: "CandidatesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionDetails",
                columns: table => new
                {
                    QuestionDetailsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionsId = table.Column<int>(type: "integer", nullable: false),
                    CandidatesId = table.Column<int>(type: "integer", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Answer = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDetails", x => x.QuestionDetailsId);
                    table.ForeignKey(
                        name: "FK_QuestionDetails_Candidates_CandidatesId",
                        column: x => x.CandidatesId,
                        principalTable: "Candidates",
                        principalColumn: "CandidatesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionDetails_Questions_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Questions",
                        principalColumn: "QuestionsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionDetailsId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    NameCreated = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameModified = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentsId);
                    table.ForeignKey(
                        name: "FK_Comments_QuestionDetails_QuestionDetailsId",
                        column: x => x.QuestionDetailsId,
                        principalTable: "QuestionDetails",
                        principalColumn: "QuestionDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_StagesId",
                table: "Candidates",
                column: "StagesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_VacancyId",
                table: "Candidates",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_QuestionDetailsId",
                table: "Comments",
                column: "QuestionDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_CandidatesId",
                table: "Meetings",
                column: "CandidatesId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_CandidatesId",
                table: "QuestionDetails",
                column: "CandidatesId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_QuestionsId",
                table: "QuestionDetails",
                column: "QuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_EnumTypeId",
                table: "Questions",
                column: "EnumTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_VacancyId",
                table: "Questions",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_VacancyId",
                table: "Requirements",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_CustomersId",
                table: "Vacancy",
                column: "CustomersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Requirements");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "QuestionDetails");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "EnumType");

            migrationBuilder.DropTable(
                name: "Vacancy");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
