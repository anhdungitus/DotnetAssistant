import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {catchError} from "rxjs/operators";
import {HandleError, HttpErrorHandler} from "../../http-error-handler.service";

export interface Question {
  id: number;
  text: string;
  createdOnUtc: Date;
}

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    Authorization: 'my-auth-token'
  })
}

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  apiQuestionUrl = "https://localhost:7284/api/question";
  private readonly handleError: HandleError;

  constructor(
    private http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    this.handleError = httpErrorHandler.createHandleError('QuestionService');
  }

  getQuestion(pageIndex: number, pageSize: number): Observable<Question[]> {
    return this.http.get<Question[]>(this.apiQuestionUrl + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize).pipe(
      catchError(this.handleError<Question[]>('getQuestion'))
    );
  }

  update(question: Question) : Observable<Question> {
    return this.http.patch<Question>(this.apiQuestionUrl, question, httpOptions).pipe(
      catchError(this.handleError<Question>('update'))
    );
  }

  deleteQuestion(id: number) {
    return this.http.delete(this.apiQuestionUrl + '/delete' + "?id=" + id).pipe(
      catchError(this.handleError('delete'))
    );
  }

  add(question: Question) : Observable<Question> {
    return this.http.post<Question>(this.apiQuestionUrl, question, httpOptions).pipe(
      catchError(this.handleError<Question>('add'))
    )
  }
}
