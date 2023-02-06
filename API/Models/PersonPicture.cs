using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PersonPicture
{
    public int PersonPictureId { get; set; }

    public int PersonId { get; set; }

    public string Picture { get; set; } = null!;

    public DateTime DateOfPost { get; set; }

    public bool ProfilePic { get; set; }

    public bool Actual { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Person { get; set; } = null!;
}
