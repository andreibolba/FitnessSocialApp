import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Group } from 'src/model/group.model';
import { InternGroup } from 'src/model/interngroup.model';
import { Note } from 'src/model/note.model';
import { Person } from 'src/model/person.model';


@Injectable({
  providedIn: 'root',
})
export class NoteDataService {
  private baseUrl = 'https://localhost:7191/api/';

  noteAdded = new BehaviorSubject<Note | null>(null);

  constructor(private http: HttpClient) { }

  getAllNotes(token: string) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Note[]>(this.baseUrl + 'note', { headers: headers });
  }

  getNoteById(token: string, noteId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.get<Note>(this.baseUrl + 'note/' + noteId, { headers: headers });
  }

  createNote(token: string, note: Note) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Note>(this.baseUrl + 'note/add', {
      noteTitle: note.noteTitle,
      noteBody: note.noteBody,
      personId: note.personId
    }, { headers: headers });
  }

  updateNote(token: string, note: Note) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Note>(this.baseUrl + 'note/edit', {
      noteId: note.noteId,
      noteTitle: note.noteTitle,
      noteBody: note.noteBody,
      personId: note.personId
    }, { headers: headers });
  }

  deleteNote(token: string, noteId: number) {
    const headers = { Authorization: 'Bearer ' + token };
    return this.http.post<Note>(this.baseUrl + 'note/delete/' + noteId, {}, { headers: headers });
  }

}
