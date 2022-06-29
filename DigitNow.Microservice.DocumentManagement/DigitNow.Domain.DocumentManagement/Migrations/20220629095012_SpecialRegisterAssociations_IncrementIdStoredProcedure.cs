using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    public partial class SpecialRegisterAssociations_IncrementIdStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createStoredProcedure = @"
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

            var createJob = @"
                use msdb;
                EXEC dbo.sp_add_job  
                    @job_name = N'JOB_Set_Association_Ids' ;  
                GO  
                EXEC sp_add_jobstep  
                    @job_name = N'JOB_Set_Association_Ids',  
                    @step_name = N'STEP_Set_Association_Ids',  
                    @subsystem = N'TSQL',  
                    @command = N'EXEC SP_SetAssociationIds',   
                    @retry_attempts = 5,  
                    @retry_interval = 5 ;  
                GO  ";

            var createSchedule = @"
                use msdb;
                EXEC dbo.sp_add_schedule  
                    @schedule_name = N'SCH_Set_Association_Ids_15s',  
                    @freq_type = 4,  
                    @freq_interval = 1,  
                    @freq_subday_type = " + "0x1" + @",   
                    @freq_subday_interval = 20,  
                    @active_start_time = 233000;
                GO
                    EXEC sp_attach_schedule
                    @job_name = N'JOB_Set_Association_Ids',  
                    @schedule_name = N'SCH_Set_Association_Ids_15s';
                GO
                    EXEC dbo.sp_add_jobserver
                    @job_name = N'JOB_Set_Association_Ids';
                GO";
            
            migrationBuilder.Sql(createStoredProcedure);
            migrationBuilder.Sql(createJob);
            migrationBuilder.Sql(createSchedule);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var deleteJob =
                @"USE msdb ;  
                GO  
                  
                EXEC sp_delete_job  
                    @job_name = N'JOB_Set_Association_Ids' ;  
                GO  ";

            var deleteStoredProcedure =
                @"IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SP_SetAssociationIds]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
                BEGIN
                    DROP PROCEDURE dbo.SP_SetAssociationIds
                END";

            var deleteSchedule = @"
                USE msdb ;  
                GO  
                  
                EXEC dbo.sp_delete_schedule  
                    @schedule_name = N'SCH_Set_Association_Ids_15s' ;  
                GO  ";

            migrationBuilder.Sql(deleteJob);
            migrationBuilder.Sql(deleteSchedule);
            migrationBuilder.Sql(deleteStoredProcedure);
        }
    }
}
