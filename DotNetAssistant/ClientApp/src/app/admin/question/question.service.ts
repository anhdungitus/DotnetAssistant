import { Injectable } from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";

export interface Question {
  id: number;
  text: string;
  createdOnUtc: Date;
}
@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  apiQuestionUrl = "https://localhost:7284/api/question";

  constructor(
    private http: HttpClient
  ) { }

  getQuestion(): Observable<Question[]> {
    return this.http.get<Question[]>(this.apiQuestionUrl);
  }
}
