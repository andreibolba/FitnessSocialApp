import { Injectable } from '@angular/core';
import { PostDataService } from './post-data.service';
import { CommentDataService } from './comment-data.service';


@Injectable({
  providedIn: 'root',
})
export class ForumDataService {
  constructor(public postData:PostDataService, public commentData:CommentDataService) { }
}
