using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202303182140)]
    public class GetPeopleInGroupMeetingView : Migration
    {
        public override void Down()
        {
            Execute.Sql("delete view [Dbo].[GetPeopleInGroupMeeting]");
        }

        public override void Up()
        {
            Execute.Sql(@"
                CREATE OR ALTER VIEW [Dbo].[GetPeopleInGroupMeeting]
                AS
                SELECT 
                	m.MeetingId,
                    gi.GroupId,
                    p.PersonId, 
                    p.FirstName, 
                    p.LastName, 
                    p.Username, 
                    p.Email, 
                    p.PasswordHash, 
                    p.PasswordSalt, 
                    p.BirthDate, 
                    p.[Status] 
                    from Meeting as m
                    left join MeetingInternGroup as mig on mig.MeetingId=m.MeetingId
                    right join InternGroup as gi on mig.GroupId=gi.GroupId
                    left join Person as p on p.PersonId=gi.InternId
                    WHERE m.Deleted=0 
                    AND mig.Deleted=0 
                    AND gi.Deleted=0 
                    AND p.Deleted=0
                ");
        }
    }
}