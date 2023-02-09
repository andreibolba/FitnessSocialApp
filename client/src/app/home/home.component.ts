import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  title = 'Fitness Social App';
  people: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('https://localhost:7191/api/people').subscribe({
      next: (response) => (this.people = response),
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log('Request has finished');
      },
    });
  }
}
