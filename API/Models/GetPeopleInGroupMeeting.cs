using System;
using System.Collections.Generic;

namespace API.Models;

public partial class GetPeopleInGroupMeeting
{
    public int? MeetingId { get; set; }

    public int GroupId { get; set; }

    public int? PersonId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Status { get; set; }
}
