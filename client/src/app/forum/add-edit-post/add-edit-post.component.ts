import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-add-edit-post',
  templateUrl: './add-edit-post.component.html',
  styleUrls: ['./add-edit-post.component.css']
})
export class AddEditPostComponent {
  operation: string="Add";

  constructor() {
  }

  onSignUpSubmit(form: NgForm) {

  }
}
