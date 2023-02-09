import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, AfterViewInit {
  title = 'Fitness Social App';
  people: any;
  display = true;

  constructor(private http: HttpClient, private auth: AuthService) {}
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.auth.displayChanged.subscribe(res => {
        this.display = res;
      });
    }, 0);
  }

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
