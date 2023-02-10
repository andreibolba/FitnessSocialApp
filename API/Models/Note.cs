using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Note
{
    public int NoteId { get; set; }

    public string NoteTitle { get; set; }

    public string NoteBody { get; set; }

    public int PersonId { get; set; }

    public DateTime PostingDate { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Person { get; set; }
}
