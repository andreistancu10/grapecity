using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
	public partial class SpecialRegisterAssociations_IncrementIdStoredProcedure : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			var storedProcedure = @"
				CREATE PROCEDURE SP_SetAssociationIds 
					AS
					BEGIN
						SET NOCOUNT ON;
						DECLARE  @lastAssociationIdValue as bigint = (Select MAX(AssociationId) from DocumentMangement.DocumentSpecialRegisterAssociations);

						SELECT * from DocumentMangement.DocumentSpecialRegisterAssociations where AssociationId is null

						update DocumentMangement.DocumentSpecialRegisterAssociations
						SET @lastAssociationIdValue = AssociationId = @lastAssociationIdValue + 1 
						where AssociationId is null
					END
					GO";

			var job = @"
				use msdb;
				EXEC dbo.sp_add_job  
					@job_name = N'Set Association Ids' ;  
				GO  
				EXEC sp_add_jobstep  
					@job_name = N'Set Association Ids',  
					@step_name = N'Set Association Ids',  
					@subsystem = N'TSQL',  
					@command = N'EXEC SP_SetAssociationIds',   
					@retry_attempts = 5,  
					@retry_interval = 5 ;  
				GO  
				use msdb;
				EXEC dbo.sp_add_schedule  
					@schedule_name = N'RunEvery15s',  
					@freq_type = 4,  
					@freq_subday_type = " + "0x1" + @",   
					@freq_subday_interval = 20,  
					@active_start_time = 233000;
				GO
					EXEC sp_attach_schedule
					@job_name = N'Set Association Ids',  
					@schedule_name = N'RunEvery15s';
				GO
					EXEC dbo.sp_add_jobserver
					@job_name = N'Set Association Ids';
				GO";

			migrationBuilder.Sql(storedProcedure);
			migrationBuilder.Sql(job);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
